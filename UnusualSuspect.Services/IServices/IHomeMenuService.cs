using UnusualSuspect.Entities.Models;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.IServices;

public interface IHomeMenuService
{

    IQueryable<HomeMenu> ShowAll();

    ResultAction CreateItem(HomeMenu model);

    ResultAction DeleteItem(int homeMenuId, string userName);

    HomeMenu? DetailsMenuItem(long? homeMenuId);

    ResultAction EditItem(HomeMenu model);

    ResultAction ActiveDeactiveMenuItem(int homeMenuId);

    List<HomeMenu> GetHomeMenu();

    HomeMenu? GetText(int id);

}
