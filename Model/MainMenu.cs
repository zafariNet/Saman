#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

#endregion

namespace Model
{
    public class MainMenu : IAggregateRoot
    {
        public virtual Guid ID { get; set; }
        /// <summary>
        /// نام منوی مادر
        /// </summary>
        public virtual string ParentMenuName { get; set; }
        /// <summary>
        /// نام قابل نمایش زیرمنو
        /// </summary>
        public virtual string SubmenuName { get; set; }
        /// <summary>
        /// یو.آر.ال زیرمنو
        /// </summary>
        public virtual string SubmenuUrl { get; set; }
        /// <summary>
        /// xType
        /// </summary>
        public virtual string xType { get; set; }
        /// <summary>
        /// آدرس یا کلاس سی.اس.اس مربوط به آیکون زیرمنو
        /// </summary>
        public virtual string Icon { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public virtual int SortOrder { get; set; }
        /// <summary>
        /// کلید مربوط به پرمیشن این منو
        /// </summary>
        public virtual string PermissionKey { get; set; }

        public virtual bool Show { get; set; }

        public virtual bool PreLoad { get; set; }


    }
}
