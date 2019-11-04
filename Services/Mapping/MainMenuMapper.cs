using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;
using Services.ViewModels;
using Model;

namespace Services.Mapping
{
    public static class MainMenuMapper
    {
        public static IEnumerable<MainMenuView> ConvertToMainMenuViews(
            this IEnumerable<MainMenu> mainMenus)
        {
            return Mapper.Map<IEnumerable<MainMenu>,
                IEnumerable<MainMenuView>>(mainMenus);
        }

        public static MainMenuView ConvertToMainMenuView(this MainMenu mainMenu)
        {
            return Mapper.Map<MainMenu, MainMenuView>(mainMenu);
        }
    }
}
