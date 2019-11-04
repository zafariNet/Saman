#region Usings



#endregion

namespace Services.ViewModels.Customers
{
    /// <summary>
    /// خصوصیات قرارگیری حالت گرافیکی مراحل در صفحه
    /// </summary>
    public class GraphicalPropertiesView
    {
        public GraphicalPropertiesView()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            EnableDragging = true;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool EnableDragging { get; set; }

    }
}
