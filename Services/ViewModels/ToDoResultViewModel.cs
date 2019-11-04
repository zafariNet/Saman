using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels
{
    public class ToDoResultViewModel
    {
        /// <summary>
        /// توضیحات مربوط به انجام دهنده وظیفه
        /// </summary>
        public virtual string ToDoResultDescription { get; set; }
        /// <summary>
        /// بستن ارجاع
        /// </summary>
        public virtual bool SecondaryClosed { get; set; }
        /// <summary>
        /// زمان یادآوری
        /// </summary>
        public virtual string RemindeTime { get; set; }
    }
}
