using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb.Repositories
{
    public class FinacialProfileRepository
    {

        private readonly IdempotencyDbContext _context;

        public FinacialProfileRepository(IdempotencyDbContext context)
        {
            _context = context;
        }

        public async Task Create(FinancialProfile financialProfile, UserEntity user)
        {
            var result = FinancialProfile.CreateRandom(user.Id);

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        

        public async Task<FinancialProfile> GetEntity(UserEntity userEntity)
        {
            try
            {
                var financialProdileEnity = await _context.financialProfiles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == userEntity.Id);

                return financialProdileEnity;
            }
            catch (Exception ex)
            {
                throw new Exception("Такого пользователя не сущетсвует");
            }
        }
        
        public async Task<FinancialProfile> GetEntityByAccountNumber(string accountNumber)
        {
            try
            {
                var financialProdileEnity = await _context.financialProfiles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

                return financialProdileEnity;
            }
            catch (Exception ex)
            {
                throw new Exception("Такого пользователя не сущетсвует");
            }
        }

        public async Task<FinancialProfile> GetFinancialProfile(Guid userId)
        {
            return await _context.financialProfiles
                .AsNoTracking()
                .Include(fp => fp.User)
                .FirstOrDefaultAsync(fp => fp.UserId == userId);
        }

        public async Task WriteOff(FinancialProfile profile, decimal amount)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            if (profile.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds for write-off");
            }

            try
            {
                profile.Balance -= amount;
                _context.financialProfiles.Update(profile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task AddToBalance(FinancialProfile profile, decimal amount)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }
            
            try
            {
                profile.Balance += amount;
                _context.financialProfiles.Update(profile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
