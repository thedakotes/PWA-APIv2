using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using EventApi.Data;
using Microsoft.EntityFrameworkCore;

namespace PWAApi.ApiService.Helpers.Seeders
{
    public class CSVSeeder<T> where T : class
    {
        private readonly string _path;
        private readonly Func<T, bool> _filter;
        private readonly Func<T, object> _keySelector;

        public CSVSeeder(string url, Func<T, object> keySelector, Func<T, bool>? filter = null)
        {
            _path = url;
            _filter = filter ?? (_ => true);
            _keySelector = keySelector;
        }

        public async Task SeedAsync(AppDbContext appDbContext)
        {
            string fullPath = Path.Combine(AppContext.BaseDirectory, _path);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Seed file not found.", fullPath);

            try
            {
                using var reader = new StreamReader(fullPath);
                using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                    Delimiter = "\t",
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true,
                    TrimOptions = TrimOptions.Trim
                });

                var records = csvReader.GetRecords<T>().Where(_filter).ToList();
                var keys = records.Select(_keySelector).ToHashSet();

                var dbSet = appDbContext.Set<T>();
                var existingKeys = dbSet
                    .AsNoTracking()
                    .Select(_keySelector)
                    .ToList();

                var newRecords = records
                    .Where(record => !existingKeys.Contains(_keySelector(record)))
                    .ToList();

                if (newRecords.Any())
                {
                    dbSet.AddRange(newRecords);
                    await appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error encounter while attempting to seed data: {_path}. Error: {ex.Message}");
            }
        }
    }
}
