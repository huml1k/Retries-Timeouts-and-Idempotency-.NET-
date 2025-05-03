using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.Infrastructure;
using System.Security.Claims;

namespace APIGateway.Services
{
    public class EmailService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtProvider _jwtProvider;

        public EmailService(
            UserRepository userRepository,
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

        public async Task<string> Login(UserEntity userEntity) 
        {
            var user = await _userRepository.GetByEmail(userEntity.Email);

            var result = _passwordHasher.Verify(userEntity.Password, user.Password);

            if (!result) throw new Exception("Неверный пароль");

            if (user == null) throw new Exception("Такого пользователя с почтой не существует");

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<Guid> GetUserByToken(ClaimsPrincipal claims) 
        {
            var userId = claims.Claims.FirstOrDefault(x => x.Type == "userId");

            if (userId == null) 
            {
                throw new Exception("Не авторизованный пользователь");
            }

            return Guid.Parse(userId.Value);
        }
    }
}
