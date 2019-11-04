#region Usings


#endregion

namespace Services.ViewModels.Customers
{
    /// <summary>
    /// خصوصیات قرارگیری حالت گرافیکی مراحل در صفحه
    /// </summary>
    public class LevelOptionsView
    {
        public LevelOptionsView()
        {
            CanSale = false;
            CanChangeNetwork = false;
            CanPersenceSupport = false;
            CanAddProblem = false;
            CanDocumentsOperation = false;
        }

        public bool CanSale { get; set; }

        public bool CanChangeNetwork { get; set; }

        public bool CanPersenceSupport { get; set; }

        public bool CanAddProblem { get; set; }

        public bool CanDocumentsOperation { get; set; }

    }
}
