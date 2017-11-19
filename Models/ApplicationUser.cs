using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models{
    public class ApplicationUser : IdentityUser
    {

    }

    public class AuthRequest {
        [Required]
        public string UserName {get; set;}
        public string authUserRequest{get; set;}

    }
}
