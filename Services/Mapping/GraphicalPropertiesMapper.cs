using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class GraphicalPropertiesMapper
    {
        public static IEnumerable<GraphicalPropertiesView> ConvertToGraphicalPropertiesViews(
            this IEnumerable<GraphicalProperties> graphicalPropertiess)
        {
            List<GraphicalPropertiesView> returnGraphicalPropertiesViews = new List<GraphicalPropertiesView>();

            foreach (GraphicalProperties graphicalProperties in graphicalPropertiess)
            {
                returnGraphicalPropertiesViews.Add(graphicalProperties.ConvertToGraphicalPropertiesView());
            }

            return returnGraphicalPropertiesViews;
        }

        public static GraphicalPropertiesView ConvertToGraphicalPropertiesView(this GraphicalProperties graphicalProperties)
        {
            return Mapper.Map<GraphicalProperties, GraphicalPropertiesView>(graphicalProperties);
        }
    }
}
