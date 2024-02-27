using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record UpdateDto
    {
        [Required(ErrorMessage = "E-poçt vacibdir")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "E-poçt ünvanı 10 ilə 255 simvol aralığında olmalıdır")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Yanlış e-poçt ünvanı")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Yanlış e-poçt formatı")]
        public string Email { get; set; }

        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "İstifadəçi adı 2 ilə 25 simvol aralığında olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "İstifadəçi adı yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        public string UserName { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrə vacibdir")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Şifrə yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrə vacibdir")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Şifrə yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
        public string? NewPassword { get; set; }

        public bool IsActivated { get; set; } = true;



    }
}
