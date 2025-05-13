using BankAPI.BankDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.BankDb.Repositories;

public class CustomerRepository
{
    private readonly BankDbContext _context;

    public CustomerRepository(BankDbContext context)
    {
        _context = context;
    }   
    
    public async Task Create(CustomerEntity customerEntity)
    {
        await _context.Customers.AddAsync(customerEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<CustomerEntity> GetEntity(CustomerEntity customerEntity)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == customerEntity.Email && x.PassportNumber == customerEntity.PassportNumber);
    }
    
    public async Task<CustomerEntity> GetByPhoneNumber(string phoneNumber)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
    
    public async Task<CustomerEntity> GetByEmail(string email)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
    
    public async Task<CustomerEntity> GetByPassportNumber(string passportNumber)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PassportNumber == passportNumber);
    }
    
    public async Task Update(CustomerEntity customerEntity)
    {
        _context.Customers.Update(customerEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task Delete(CustomerEntity customerEntity)
    {
        _context.Customers.Remove(customerEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<CustomerEntity>> GetAll()
    {
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<CustomerEntity> GetById(int id)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}