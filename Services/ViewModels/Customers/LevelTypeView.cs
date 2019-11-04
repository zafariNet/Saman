using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class LevelTypeView : BaseView
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }


        //IEnumerables

        [Display(Name = "مرحله ها")]
        public IEnumerable<LevelView> Levels { get; protected set; }
    }
}
