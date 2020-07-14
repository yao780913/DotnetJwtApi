using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetJwtApi.Models
{
    public class AuthenticateRequest
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
