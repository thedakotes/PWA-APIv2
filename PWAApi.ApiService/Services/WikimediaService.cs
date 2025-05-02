using PWAApi.ApiService.DataTransferObjects.PlantID;
using System.Text.Json;

namespace PWAApi.ApiService.Services
{
    public class WikimediaService
    {
        public async Task<IEnumerable<ImageDTO>?> GetImagesFromWikimediaAsync(string searchTerm)
        {
            using var httpClient = new HttpClient();

            var requestUrl = $"https://commons.wikimedia.org/w/api.php" +
                             $"?action=query&format=json&generator=search" +
                             $"&gsrsearch={Uri.EscapeDataString(searchTerm)}" +
                             $"&gsrlimit=10&gsrnamespace=6" + // Only search file pages
                             $"&prop=imageinfo&iiprop=url|extmetadata" +
                             $"&origin=*";


            var response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("query", out var queryElement) ||
                !queryElement.TryGetProperty("pages", out var pagesElement))
            {
                return null;
            }

            List<ImageDTO> images = new List<ImageDTO>();

            foreach (var page in pagesElement.EnumerateObject())
            {
                var imageInfoArray = page.Value.GetProperty("imageinfo");

                if (imageInfoArray.GetArrayLength() == 0) continue;

                var imageInfo = imageInfoArray[0];

                var url = imageInfo.GetProperty("url").GetString() ?? string.Empty;

                if (url.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || 
                    url.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                    url.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract license data from extmetadata if available
                    var extMetadata = imageInfo.GetProperty("extmetadata");

                    var hasLicenseShortName = extMetadata.TryGetProperty("LicenseShortName", out var licenseProperty);
                    string license = hasLicenseShortName ? licenseProperty.GetProperty("value").GetString() ?? string.Empty : string.Empty;

                    var hasLicenseUrl = extMetadata.TryGetProperty("LicenseUrl", out var licenseUrlProperty);
                    string licenseUrl = hasLicenseUrl ? licenseUrlProperty.GetProperty("value").GetString() ?? string.Empty : string.Empty;

                    var hasArtist = extMetadata.TryGetProperty("Artist", out var artistProperty);
                    string attribution = hasArtist ? artistProperty.GetProperty("value").GetString() ?? string.Empty : string.Empty;

                    images.Add(new ImageDTO(url, license, licenseUrl, attribution));
                }
            }

            return images;
        }
    }
}
