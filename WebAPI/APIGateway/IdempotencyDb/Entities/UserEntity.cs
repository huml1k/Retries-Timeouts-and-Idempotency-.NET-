namespace APIGateway.IdempotencyDb.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public static UserEntity Create(UserEntity user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Email = user.Email,
            };
        }
    }
}
