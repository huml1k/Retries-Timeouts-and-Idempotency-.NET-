using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using APIGateway.Infrastructure;
using System.Security.Claims;

namespace APIGateway.Services
{
    public class EmailService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtProvider _jwtProvider;

        public EmailService(
            IUserRepository userRepository,
            JwtProvider jwtProvider,
            PasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task Register(UserEntity userEntity)
        {
            var hashedPassword = _passwordHasher.GenerateTokenSHA(userEntity.Password);

            var user = UserEntity.Create(userEntity);

            await _userRepository.Create(user);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            var result = _passwordHasher.Verify(password, user.Password);

            if (!result) throw new Exception("Неверный пароль");

            if (user == null) throw new Exception("Такого пользователя с почтой не существует");

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<Guid?> GetUserByToken(ClaimsPrincipal claims)
        {
            var userIdClaim = claims.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return null; // Возвращаем null, если нет userId или он некорректен
            }

            return userId; // Возвращаем корректный userId
        }

        public async Task<UserEntity> GetByEmail(string email) 
        {
            var result = await _userRepository.GetByEmail(email);

            return result;
        }

        public async Task<UserEntity> GetById(Guid id)
        {
            var result = await _userRepository.GetById(id);

            return result;
        }
    }
}
