using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;

namespace Infrastructure.Domain
{
    /// <summary>
    /// General repository for Aggregate root entities for retrive data, with generic entity type
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IReadOnlyRepository<T> where T: IAggregateRoot
    {
        /// <summary>
        /// جستجو با استفاده از شناسه
        /// </summary>
        /// <param name="id">شناسه</param>
        /// <returns></returns>
        T FindBy(Guid id);
        /// <summary>
        /// بازیابی همه
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> FindAll();
        /// <summary>
        /// بازیابی همه
        /// </summary>
        /// <param name="index">رکورد ابتدایی شروع از صفر</param>
        /// <param name="count">تعداد رکوردها</param>
        /// <returns></returns>
        Response<T> FindAll(int index, int count);

        /// <summary>
        /// بازیابی همه
        /// </summary>
        /// <param name="SortByCreateDate">مرتب سازی بر اساس تاریخ ایجاد</param>
        /// <param name="index">رکورد ابتدایی شروع از صفر</param>
        /// <param name="count">تعداد رکوردها</param>
        /// <returns></returns>
        Response<T> FindAllWithSort(int index, int count, IList<Sort> Sorts);
        /// <summary>
        /// بازیابی با استفاده از
        /// </summary>
        /// <param name="query">کوئری جستجو</param>
        /// <returns></returns>
        IEnumerable<T> FindBy(Query query);
        /// <summary>
        /// بازیابی با استفاده از
        /// </summary>
        /// <param name="query">کوئری جستجو</param>
        /// <returns></returns>
        Response<T> FindByQuery(Query query);

        //Response<T> FindByQuery(Query query, IEnumerable<Sort> sorts);

        Response<T> FindByQuery(Query query, IList<Sort> sorts);
        /// <summary>
        /// بازیابی با استفاده از
        /// </summary>
        /// <param name="query">کوئری جستجو</param>
        /// <param name="index">رکورد ابتدایی شروع از صفر</param>
        /// <param name="count">تعداد رکوردها</param>
        /// <returns></returns>
        Response<T> FindBy(Query query, int index, int count);
        Response<T> FindBy(Query query, int index, int count, IList<Sort> sort);
        
        // Using Hql Query
        Response<T> FindBy(string hqlQuery);

        /// <summary>
        /// بازیابی با استفاده از HQL‏‏‏
        /// </summary>
        /// <param name="query"></param>
        /// <param name="index">رکورد ابتدایی شروع از صفر</param>
        /// <param name="count">تعداد رکوردها</param>
        /// <returns></returns>
        Response<T> FindAll(string hqlQuery, int index, int count);
        /// <summary>
        /// بازیابی با استفاده از HQL‏‏‏
        /// </summary>
        /// <param name="hqlQuery">کوئری جستجو</param>
        /// <returns></returns>
        Response<T> FindAll(string hqlQuery);

        IQueryable<T> Queryable();
    }
}
