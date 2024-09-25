using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eden_Fn.Models.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Password { get; set; }
    }
}
