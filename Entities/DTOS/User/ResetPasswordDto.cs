using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record ResetPasswordDto
    {
        [Required(ErrorMessage = "E-poçt vacibdir")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "E-poçt ünvanı 10 ilə 255 simvol aralığında olmalıdır")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Yanlış e-poçt ünvanı")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Yanlış e-poçt formatı")]
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        [Required(ErrorMessage = "Yeni parol daxil edilməlidir.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Yeni parol yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Yeni parol 8 ilə 25 simvol aralığında olmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
