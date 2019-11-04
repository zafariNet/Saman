using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Controllers.ViewModels
{
    public class LogOnPageView
    {
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        [Display(Name = "نام کاربری")]
        public string LoginName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
