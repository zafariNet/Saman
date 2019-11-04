using Services.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.ViewModels
{
    public class MainMenuView
    {
        public Guid ID { get; set; }
        /// <summary>
        /// نام منوی مادر
        /// </summary>
        public string ParentMenuName { get; set; }
        /// <summary>
        /// نام قابل نمایش زیرمنو
        /// </summary>
        public string SubmenuName { get; set; }
        /// <summary>
        /// یو.آر.ال زیرمنو
        /// </summary>
        public string SubmenuUrl { get; set; }
        /// <summary>
        /// xType
        /// </summary>
        public string xType { get; set; }
        /// <summary>
        /// آدرس یا کلاس سی.اس.اس مربوط به آیکون زیرمنو
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// فعال یا غیر فعال بودن زیرمنو
        /// </summary>
        public bool Disabeled { get; set; }

        public bool PreLoad { get; set; }

        public int CustomerCount { get; set; }

        public ColumnViews columns { get; set; }

        /// <summary>
        /// نمایش منو
        /// </summary>
        public bool Show { get; set; }
    }
}
