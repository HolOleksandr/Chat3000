﻿using Chat.BLL.DTO;
using Chat.BLL.Models.Paging;
using Chat.BLL.Services.Interfaces;
using Chat.BLL.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatApp.API.Controllers
{
    //[Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDTO> _userDtoValidator;

        public UserController(IUserService userService, IValidator<UserDTO> userDtoValidator)
        {
            _userService = userService;
            _userDtoValidator = userDtoValidator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] SearchParameters searchParameters)
        {
            var users = await _userService.GetAllUsersAsync(searchParameters);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userUpdateModel)
        {
            var validation = await _userDtoValidator.ValidateAsync(userUpdateModel);
            if (!validation.IsValid)
                return StatusCode(400, validation.Errors.Select(e => e.ErrorMessage));

            var users = userUpdateModel;
            await _userService.UpdateUserInfoAsync(userUpdateModel);
           
            return Ok();
        }



    }
}
