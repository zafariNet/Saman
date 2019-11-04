using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Domain
{
    /// <summary>
    /// General repository for Aggregate root entities for Save, Add, Remove or Retrive Data
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface
        IRepository<T> : IReadOnlyRepository<T> where T : IAggregateRoot
    {
        /// <summary>
        /// ذخیره کردن تغییرات
        /// </summary>
        /// <param name="entity">موجودیت</param>
        void Save(T entity);
        /// <summary>
        /// اضافه کردن موجودیت
        /// </summary>
        /// <param name="entity">موجودیت</param>
        void Add(T entity);
        /// <summary>
        /// حذف یک موجودیت
        /// </summary>
        /// <param name="entity">موجودیت</param>
        void Remove(T entity);

        /// <summary>
        /// حذف با استفاده ا ای دی
        /// </summary>
        /// <param name="id"></param>
        void RemoveById(Guid id);

        /// <summary>
        /// حذف چند موجودیت
        /// </summary>
        /// <param name="entities"> موجودیت ها</param>
        void Remove(IEnumerable<T> entities);
    }
}
