using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Eden_Fn.Models.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$",ErrorMessage ="Comprimento mínimo 6 e deve conter 1 maiúscula, 1 minúscula, 1 caractere especial e 1 dígito")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [Compare("Password")]
        [DisplayName("Confirmação de senha")]
        public string PasswordConfirm { get; set; }

        [Url]
        public string FrameLink { get; set; }

        public string? Role { get; set; }
    }
}