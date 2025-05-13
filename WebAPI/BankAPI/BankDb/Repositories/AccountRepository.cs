using BankAPI.BankDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.BankDb.Repositories;

public class AccountRepository
{
    private readonly BankDbContext _context;
    
    public AccountRepository(BankDbContext context)
    {
        _context = context;
    }
    
    public async Task Create(AccountEntity accountEntity)
    {
        await _context.Accounts.AddAsync(accountEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<AccountEntity> GetEntity(AccountEntity accountEntity)
    {
        return await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountNumber == accountEntity.AccountNumber);
    }
    
    public async Task<AccountEntity> GetByAccountNumber(string accountNumber)
    {
        return await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
    }
    
    public async Task Update(AccountEntity accountEntity)
    {
        _context.Accounts.Update(accountEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task Delete(AccountEntity accountEntity)
    {
        _context.Accounts.Remove(accountEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<AccountEntity>> GetAccountsByCustomer(int customerId)
    {
        return await _context.Accounts
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }
}