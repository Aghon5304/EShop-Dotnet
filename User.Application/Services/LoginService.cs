﻿using User.Application.Producer;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Exceptions.Login;
namespace User.Application.Services
{
	public class LoginService : ILoginService
	{
        protected IJwtTokenService _jwtTokenService;
        protected Queue<int> _userLoggedIdsQueue;
        protected IKafkaProducer _kafkaProducer;

        public LoginService(IJwtTokenService jwtTokenService, IKafkaProducer kafkaProducer)
        {
            _jwtTokenService = jwtTokenService;
            _userLoggedIdsQueue = new Queue<int>();
            _kafkaProducer = kafkaProducer;
        }
        public string Login(string email, string password)
        {
            if (email == "admin@admin" && password == "password")
            {
                var roles = new List<string> { "Client", "Employee", "Administrator" };
                var token = _jwtTokenService.GenerateToken(123, roles);
                _userLoggedIdsQueue.Enqueue(123);
                _kafkaProducer.SendMessageAsync("after-login-email-topic", "email");
                return token;
            }
            else
            {
                throw new InvalidCredentialsException();
            }

        }
    }
}