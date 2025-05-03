using APIGateway.IdempotencyDb.Entities;

namespace APIGateway.IdempotencyDb.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task Create(UserEntity userEntity);

        public Task<UserEntity> GetEntity(UserEntity userEntity);
    }
}
