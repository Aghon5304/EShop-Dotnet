using Microsoft.EntityFrameworkCore;
using User.Application.Producer;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;
using User.Domain.Exceptions.Login;
using User.Domain.Repositories;
using User.Domain.Models.Response;
namespace User.Application.Services
{
	public class LoginService : ILoginService
	{
        protected IJwtTokenService _jwtTokenService;
        protected Queue<int> _userLoggedIdsQueue;
        protected IKafkaProducer _kafkaProducer;
        protected IRepository _userRepository;

        public LoginService(IJwtTokenService jwtTokenService, IKafkaProducer kafkaProducer, IRepository userRepository, DataContext context)
        {
            _jwtTokenService = jwtTokenService;
            _userLoggedIdsQueue = new Queue<int>();
            _kafkaProducer = kafkaProducer;
            _userRepository = userRepository;
        }
        public async Task<string> Login(string email, string password)
        {
            var userLogin = await _userRepository.GetUserLoginAsync(email);
            if (userLogin == null)
            {
                throw new InvalidCredentialsException();
            }
            else if (email == userLogin.Email && password == userLogin.PasswordHash)
            {
                var roles = userLogin.Roles.Select(role => role.Name).ToList();
                var token = _jwtTokenService.GenerateToken(userLogin.Id, roles);
                _userLoggedIdsQueue.Enqueue(userLogin.Id);
                await _kafkaProducer.SendMessageAsync("after-login-email-topic", userLogin.Email);
                await _userRepository.UpdateUserLastLogIn(new UserUpdateLoginAtDTO { Id = userLogin.Id, LastLoginAt = DateTime.UtcNow });

                return token;
            }
            else
            {
                throw new InvalidCredentialsException();
            }
        }
    }
}