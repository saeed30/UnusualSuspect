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

public class HomeMenuService : IHomeMenuService
{


    private readonly ILogger<IHomeMenuService> _logger;
    private readonly IUnitOfWork _uow;
    private readonly DbSet<HomeMenu> _HomeMenu;
    private readonly ILogService _ILogService;
    protected readonly IUploadServise _uploadServise;

    public HomeMenuService(ILogger<HomeMenuService> logger, IUnitOfWork uow, ILogService iLogService, IUploadServise uploadServise)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _HomeMenu = uow.Set<HomeMenu>();
        _ILogService = iLogService;
        _uploadServise = uploadServise;
    }


    public IQueryable<HomeMenu> ShowAll()
    {
        var HomeMenuList = _HomeMenu.AsQueryable();
        return HomeMenuList;
    }



    public ResultAction CreateItem(HomeMenu model)
    {
        try
        {
            var CheckUploade = _uploadServise.IsUpload(model.ImageFile, false);
            if (!CheckUploade.Success)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = CheckUploade.MessageList
                };
            }
            var UploadeFile = _uploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\IconMenu", true);
            model.Icon = UploadeFile.Result.MessageList;

            model.IsActive = true;

            _HomeMenu.Add(model);
            _ILogService.AddLog(new LogObject()
            {
                NextValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(model),
                PerValue = null,
                ObjectTypeId = "HomeMenu",
                ObjectTypeName = "HomeMenu",
                Title = "Create HomeMenu",
                DateCreate = DateTime.Now,
                UserName = model.UserName
            });

            _uow.SaveChanges();

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



    public ResultAction DeleteItem(int homeMenuId, string UserName)
    {
        var Item = DetailsMenuItem(homeMenuId);
        try
        {
            _HomeMenu.Remove(Item);
            _ILogService.AddLog(new LogObject()
            {
                NextValue = null,
                PerValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(Item),
                ObjectTypeId = "HomeMenu",
                ObjectTypeName = "HomeMenu",
                Title = "Delete HomeMenu",
                DateCreate = DateTime.Now,
                UserName = UserName
            });
            _uow.SaveChanges();
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



    public HomeMenu DetailsMenuItem(long? homeMenuId)
    {
        return _HomeMenu.FirstOrDefault(x => x.Id == homeMenuId);
    }



    public ResultAction EditItem(HomeMenu model)
    {

        var CheckUploade = _uploadServise.IsUpload(model.ImageFile, false);
        if (!CheckUploade.Success)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = CheckUploade.MessageList
            };
        }
        var UploadeFile = _uploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\IconMenu", true);
        model.Icon = UploadeFile.Result.MessageList;


        var Item = DetailsMenuItem(model.Id);
        try
        {
            Item.Title = model.Title;
            Item.Link = model.Link;
            Item.Text = model.Text;
            Item.Priority = model.Priority;
            if (!string.IsNullOrEmpty(model.Icon))
                Item.Icon = model.Icon;

            _ILogService.AddLog(new LogObject()
            {
                NextValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(Item),
                PerValue = HelperCommon.ShallowCopyEntityToString<HomeMenu>(DetailsMenuItem(model.Id)),
                ObjectTypeId = "HomeMenu",
                ObjectTypeName = "HomeMenu",
                Title = "Update HomeMenu",
                DateCreate = DateTime.Now,
                UserName = model.UserName
            });
            _uow.SaveChanges();
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
            _uow.SaveChanges();

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
        return _HomeMenu.Where(a => a.IsActive).OrderBy(a => a.Priority).ToList();
    }



    public HomeMenu GetText(int Id)
    {
        return _HomeMenu.FirstOrDefault(a => a.IsActive && a.Id == Id);
    }



}
