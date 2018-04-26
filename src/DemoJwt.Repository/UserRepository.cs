﻿using DemoJwt.Application.Contracts;
using DemoJwt.Application.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DemoJwt.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly InMemoryDatabaseContext _context;

        public UserRepository(InMemoryDatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> Get(string email)
        {
            var user = _context.Users
                .FirstOrDefault(a => a.Email.Equals(email));

            return await Task.FromResult(user);
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await Get(email);

            if (!user.Password.Encoded.Equals(password))
            {
                return await Task.FromResult<User>(null);
            }

            return await Task.FromResult(user);
        }

        public async Task<bool> ExistsUser(string email)
        {
            var exists = await Get(email) != null;
            return await Task.FromResult(exists);
        }

        public async Task Save(User user)
        {
            _context.Users.Remove(user);
            _context.Users.Add(user);

            await Task.CompletedTask;
        }
    }
}