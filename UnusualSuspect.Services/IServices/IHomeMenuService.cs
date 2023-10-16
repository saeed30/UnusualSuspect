using UnusualSuspect.Entities.Models;
using UnusualSuspect.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.IServices;

public interface IHomeMenuService
{

    IQueryable<HomeMenu> ShowAll();

    ResultAction CreateItem(HomeMenu model);

    ResultAction DeleteItem(int homeMenuId, string UserName);

    HomeMenu DetailsMenuItem(long? homeMenuId);

    ResultAction EditItem(HomeMenu model);

    ResultAction ActiveDeactiveMenuItem(int homeMenuId);

    List<HomeMenu> GetHomeMenu();

    HomeMenu GetText(int Id);

}
