﻿using System.ComponentModel.DataAnnotations;

namespace HrSystem.DTO
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
