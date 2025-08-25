using System;
using System.Threading.Tasks;
using Core.entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext _context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<Product>> GetPorductsAsync(string? brand, string? type)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(b => b.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(b => b.Type == type);

        return await query.ToListAsync();
    }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public bool ProductExists(int id)
    {
        return  _context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
    }
}
