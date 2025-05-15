public interface IRepository<T> where T : class
{
    // Get all items
     public Task<IEnumerable<T>> GetAllAsync();

    // Get a single item by ID
    public Task<T?> GetByIdAsync(int id);

    // Create a new item
    public Task AddAsync(T entity);

    // Update an existing item
    public Task UpdateAsync(T entity);

    // Delete an item by ID
    public Task DeleteAsync(int id);

    // Delete an item by reference
    public Task DeleteAsync(T entity);

    // Save changes to the database
    public Task SaveChangesAsync();
}