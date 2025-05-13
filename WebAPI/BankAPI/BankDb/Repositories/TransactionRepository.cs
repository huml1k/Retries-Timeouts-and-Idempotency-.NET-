using BankAPI.BankDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.BankDb.Repositories;

public class TransactionRepository
{
    private readonly BankDbContext _context;
    
    public TransactionRepository(BankDbContext context)
    {
        _context = context;
    }
    
    public async Task Create(TransactionEntity transactionEntity)
    {
        await _context.Transactions.AddAsync(transactionEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<TransactionEntity> GetEntity(TransactionEntity transactionEntity)
    {
        return await _context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FromAccountNumber == transactionEntity.FromAccountNumber && x.ToAccountNumber == transactionEntity.ToAccountNumber);
    }
    
    public async Task<TransactionEntity> GetByAccountNumber(string accountNumber)
    {
        return await _context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FromAccountNumber == accountNumber || x.ToAccountNumber == accountNumber);
    }
    
    public async Task Update(TransactionEntity transactionEntity)
    {
        _context.Transactions.Update(transactionEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task Delete(TransactionEntity transactionEntity)
    {
        _context.Transactions.Remove(transactionEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<TransactionEntity>> GetTransactionsByAccountNumber(string accountNumber)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(x => x.FromAccountNumber == accountNumber || x.ToAccountNumber == accountNumber)
            .ToListAsync();
    }
}