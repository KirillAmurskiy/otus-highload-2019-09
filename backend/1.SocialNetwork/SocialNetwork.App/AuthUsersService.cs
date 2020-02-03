﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amursoft.PasswordHasher;
using AutoMapper;
using SocialNetwork.App.Dtos;
using SocialNetwork.Model;

namespace SocialNetwork.App
{
    public class AuthUsersService:IAuthUsersService
    {
        private readonly IUsersRepo repo;
        private readonly IPasswordHasher passwordHasher;
        private readonly IMapper mapper;

        private readonly List<UserDto> usersCache = new List<UserDto>();
        
        public AuthUsersService(IUsersRepo repo, IPasswordHasher passwordHasher, IMapper mapper)
        {
            this.repo = repo;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
        }
        
        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await FindByUsername(username);
            if (user != null)
            {
                return passwordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Success;
            }

            return false;
        }

        public async Task<UserDto> FindByUsername(string username)
        {
            var authUser = usersCache.FirstOrDefault(x => x.Email == username);
            if (authUser == null)
            {
                var user = await repo.GetUserByEmail(username);
                authUser = mapper.Map<UserDto>(user);
                usersCache.Add(authUser);
            }
            return authUser;
        }

        public async Task<UserDto> FindById(long id)
        {
            var authUser = usersCache.FirstOrDefault(x => x.Id == id);
            if (authUser == null)
            {
                var user = await repo.GetUser(id);
                authUser = mapper.Map<UserDto>(user);
                usersCache.Add(authUser);
            }
            return authUser;
        }
    }
}