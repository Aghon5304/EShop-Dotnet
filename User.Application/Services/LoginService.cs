using User.Application.Producer;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Exceptions.Login;
using User.Domain.Models.Entities;
using User.Domain.Repositories;
namespace User.Application.Services
{
	public class LoginService : ILoginService
	{
        protected IJwtTokenService _jwtTokenService;
        protected Queue<int> _userLoggedIdsQueue;
        protected IKafkaProducer _kafkaProducer;
        protected IRepository _userRepository;

        public LoginService(IJwtTokenService jwtTokenService, IKafkaProducer kafkaProducer, IRepository userRepository)
        {
            _jwtTokenService = jwtTokenService;
            _userLoggedIdsQueue = new Queue<int>();
            _kafkaProducer = kafkaProducer;
            _userRepository = userRepository;
        }
        public string Login(string email, string password)
        {
            var userLogin = _userRepository.GetUserLoginAsync(email).Result;
            if (userLogin==null)
            {
                throw new InvalidCredentialsException();
            }
            else if(email == userLogin.Email && password == userLogin.PasswordHash)
            {
                var roles = userLogin.Roles.Select(role => role.Name).ToList();
                var token = _jwtTokenService.GenerateToken(123, roles);
                _userLoggedIdsQueue.Enqueue(123);
                _kafkaProducer.SendMessageAsync("after-login-email-topic", userLogin.Email);
                return token;
            }
            else
            {
                throw new InvalidCredentialsException();
            }



        }
    }
}