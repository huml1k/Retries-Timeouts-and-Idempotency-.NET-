using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdempotencyDbContext _context;

        public UserRepository(IdempotencyDbContext context)
        {
            _context = context;
        }

        public async Task Create(UserEntity userEntity)
        {
            var result = new UserEntity
            {
                Email = userEntity.Email,
                Login = userEntity.Login,
                Password = userEntity.Password
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task<UserEntity> GetByEmail(string email)
        {
            return await _context.userEntities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserEntity> GetEntity(UserEntity userEntity)
        {
            try
            {
                var userInEnity = await _context.userEntities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == userEntity.Email && x.Password == userEntity.Password);

                return userEntity;
            }
            catch (Exception ex)
            {
                throw new Exception("Такого пользователя не сущетсвует или введены неверные данные");
            }
        }
    }
}
