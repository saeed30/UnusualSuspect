using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Services;

public class HomeMenuService(ILogger<HomeMenuService> logger, IUnitOfWork uow, ILogService iLogService,
    IUploadServise uploadServise)
  : IHomeMenuService
{

    private readonly DbSet<HomeMenu> homeMenu = uow.Set<HomeMenu>();

    public IQueryable<HomeMenu> ShowAll()
    {
        var homeMenuList = homeMenu.AsQueryable();
        return homeMenuList;
    }



    public ResultAction CreateItem(HomeMenu model)
    {
        try
        {
            var checkUploade = uploadServise.IsUpload(model.ImageFile, false);
            if (!checkUploade.Success)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = checkUploade.MessageList
                };
            }
            var uploadFile = uploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\IconMenu", true);
            model.Icon = uploadFile.Result.MessageList;

            model.IsActive = true;

            homeMenu.Add(model);
            iLogService.AddLog(new LogObject()
            {
                NextValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(model),
                PerValue = null,
                ObjectTypeId = "HomeMenu",
                ObjectTypeName = "HomeMenu",
                Title = "Create HomeMenu",
                DateCreate = DateTime.Now,
                UserName = model.UserName
            });

            uow.SaveChanges();

            return new ResultAction()
            {
                Success = true,
                Id = model.Id.ToString(),
                MessageList = $"آیتم منو  {model.Id} با موفقیت ثبت گردید",
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $"در ثبت آیتم منو {model.Id} خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}",
            };
        }
    }



    public ResultAction DeleteItem(int homeMenuId, string userName)
    {
        var item = DetailsMenuItem(homeMenuId);
        try
        {
            homeMenu.Remove(item);
            iLogService.AddLog(new LogObject()
            {
                NextValue = null,
                PerValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(item),
                ObjectTypeId = "HomeMenu",
                ObjectTypeName = "HomeMenu",
                Title = "Delete HomeMenu",
                DateCreate = DateTime.Now,
                UserName = userName
            });
            uow.SaveChanges();
            return new ResultAction()
            {
                Success = true,
                TitleResult = "موفقیت آمیز",
                MessageList = $"آیتم انتخاب شده با موفقیت حذف گردید",
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                TitleResult = "خطا",
                MessageList = $"در حذف آیتم خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }



    public HomeMenu? DetailsMenuItem(long? homeMenuId)
    {
        return homeMenu.FirstOrDefault(x => x.Id == homeMenuId);
    }



    public ResultAction EditItem(HomeMenu model)
    {

        var checkUpload = uploadServise.IsUpload(model.ImageFile, false);
        if (!checkUpload.Success)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = checkUpload.MessageList
            };
        }
        var uploadFile = uploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\IconMenu", true);
        model.Icon = uploadFile.Result.MessageList;


        var item = DetailsMenuItem(model.Id);
        try
        {
            item.Title = model.Title;
            item.Link = model.Link;
            item.Text = model.Text;
            item.Priority = model.Priority;
            if (!string.IsNullOrEmpty(model.Icon))
                item.Icon = model.Icon;

            iLogService.AddLog(new LogObject()
            {
                NextValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(item),
                PerValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(DetailsMenuItem(model.Id)),
                ObjectTypeId = "HomeMenu",
                ObjectTypeName = "HomeMenu",
                Title = "Update HomeMenu",
                DateCreate = DateTime.Now,
                UserName = model.UserName
            });
            uow.SaveChanges();
            return new ResultAction()
            {
                Success = true,
                Id = model.Id.ToString(),
                MessageList = $"منو {model.Id} با موفقیت ویرایش گردید",
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $"در ویرایش آیتم منو {model.Id} خطایی رخ داده است. " + HelperCommon.ReturnMessageException(e)
            };
        }
    }



    public ResultAction ActiveDeactiveMenuItem(int homeMenuId)
    {
        var item = DetailsMenuItem(homeMenuId);
        try
        {
            if (item.IsActive)
                item.IsActive = false;
            else
                item.IsActive = true;
            uow.SaveChanges();

            return new ResultAction()
            {
                Success = true
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $" خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }



    public List<HomeMenu> GetHomeMenu()
    {
        return homeMenu.Where(a => a.IsActive).OrderBy(a => a.Priority).ToList();
    }



    public HomeMenu? GetText(int id)
    {
        return homeMenu.FirstOrDefault(a => a.IsActive && a.Id == id);
    }



}
