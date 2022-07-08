using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Post;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage ="Debe escribir su nombre")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage ="Debe escribir su apellido")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage ="Debe proporcionar un telefono")]
        public string Phone { get; set; }

        public string ProfilePicture { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Debe escribir su Email")]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage ="Debe escribir un nombre de usuario")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Debe escribir una contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debe repetir la contraseña")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        public string RepPassword { get; set; }
        public bool Active { get; set; }

        //[DataType(DataType.Upload)]
        //[Required(ErrorMessage ="Debe seleccionar una imagen para su perfil")]
        //public IFormFile Picture { get; set; }

        //Relations
        public ICollection<PostViewModel> Posts { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
