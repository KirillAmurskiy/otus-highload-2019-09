﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Model;

namespace UsersService.AspNet.Controllers
{
    /// <summary>
    /// Users service
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo repo;

        public UsersController(IUsersRepo repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Get User's count.
        /// </summary>
        /// <returns>User</returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<int> GetUsersCount()
        {
            return await repo.GetUsersCount();
        }
        
        /// <summary>
        /// Get Users.
        /// </summary>
        /// <param name="skip">Pagination skip count</param>
        /// <param name="take">Pagination take count</param>
        /// <returns>User</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<User>> GetUsers(
            [Range(0, int.MaxValue)]
            int skip,
            [Range(0, 100)]
            int take)
        {
            return await repo.GetUsers(skip, take);
        }
        
        /// <summary>
        /// Get User.
        /// </summary>
        /// <param name="userId">Идентификатор</param>
        /// <returns>User</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUser(long userId)
        {
            return await repo.GetUser(userId);
        }
    }
}