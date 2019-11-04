namespace Model.Sales
{
    public enum SaleDetailStatus
    {
        Nothing, // اقدام نشده
        Delivered, // تحویل شده
        Rollbacked, // برگشت شده
        DeliveredAndRollbacked // تحویل سپس برگشت شده
    }

}
