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

        public async Task<FinancialProfile> GetFinancialProfile(Guid userId)
        {
            return await _context.financialProfiles
                .AsNoTracking()
                .Include(fp => fp.User)
                .FirstOrDefaultAsync(fp => fp.UserId == userId);
        }
    }
}
