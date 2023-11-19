using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnusualSuspect.Services.Services;

public class LogService(IUnitOfWork uow) : ILogService
{
    private readonly DbSet<LogObject> logObject = uow.Set<LogObject>();
    private readonly DbSet<ObjectType> objectTypes = uow.Set<ObjectType>();
    private readonly DbSet<ApplicationUser> user = uow.Set<ApplicationUser>();
    private readonly DbSet<AdminPanleUser> adminPanleUser = uow.Set<AdminPanleUser>();


    /// <summary>
    /// ثبت لاگ فعالیت کاربران
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public bool AddLog(LogObject model)
    {
        try
        {
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var UserOB = user.FirstOrDefault(x => x.UserName == model.UserName);
                model.UserId = UserOB.Id;
                if (!objectTypes.Any(x => x.ObjectKey == model.ObjectTypeId))
                    objectTypes.Add(new ObjectType()
                    {
                        ObjectKey = model.ObjectTypeId,
                        Name = model.ObjectTypeName
                    });
                logObject.Add(model);
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }

    }



    public IQueryable<LogObject> CombinedAllLogObjectSearch(LogObjectSearchViewModel model)
    {
        model.StartDate = model.StartDate ?? DateTime.MinValue;
        model.EndDate = model.EndDate ?? DateTime.MaxValue;

        var logObjectList = logObject.Where(x => (model.ObjectType == null || x.ObjectTypeId == model.ObjectType)
        && (model.AdminUser == null || x.UserId.ToString() == model.AdminUser)
        && ((model.StartDate <= x.DateCreate && model.EndDate >= x.DateCreate)));

        if (!string.IsNullOrEmpty(model.KeyWord))
        {
            var searchTerms = model.KeyWord.Split(' ');
            var term = searchTerms[0];
            var LogObjectList2 = logObjectList.Where(x =>
                         (x.ApplicationUser.LastName ?? "").Contains(term)
                      || (x.ApplicationUser.FirstName ?? "").Contains(term)
                      || (x.Title).Contains(term)
                      || (x.Id.ToString() == term));
            foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
            {
                LogObjectList2 = LogObjectList2.Union(logObjectList.Where(x =>
                         (x.ApplicationUser.LastName ?? "").Contains(term)
                      || (x.ApplicationUser.FirstName ?? "").Contains(term)
                      || (x.Title).Contains(term)
                      || (x.Id.ToString() == term)));
            }
            logObjectList = LogObjectList2;
        }
        return logObjectList.Select(x => new LogObject()
        {
            Id = x.Id,
            Title = x.Title,
            UserId = x.UserId,
            ObjectType = x.ObjectType,
            ObjectTypeId = x.ObjectTypeId,
            DateCreate = x.DateCreate,
            ObjectTypeName = x.ObjectTypeName,
            NextValue = x.NextValue,
            PerValue = x.PerValue,
            ApplicationUser = new ApplicationUser()
            {
                FirstName = x.ApplicationUser.FirstName,
                LastName = x.ApplicationUser.LastName
            }
        });
    }



    public List<SelectListItem> DropDownListAdminUser()
    {
        List<SelectListItem> AdminUserList = new List<SelectListItem>();
        AdminUserList.Add(new SelectListItem() { Text = "جستجو براساس نام کاربر", Value = "" });
        AdminUserList.AddRange(adminPanleUser.Select(u => new SelectListItem
        {
            Text = u.ApplicationUser.FirstName + "  " + u.ApplicationUser.LastName,
            Value = u.ApplicationUser.Id.ToString()
        }).ToList());

        return AdminUserList;
    }



    public List<SelectListItem> DropDownListObjectType()
    {
        List<SelectListItem> ObjectTypeList = new List<SelectListItem>();
        ObjectTypeList.Add(new SelectListItem() { Text = "جستجو براساس عنوان فعالیت", Value = "" });
        ObjectTypeList.AddRange(objectTypes.Select(u => new SelectListItem
        {
            Text = u.ObjectKey,
        }).ToList());

        return ObjectTypeList;
    }



    public LogObject DetailsLogObject(int? LogObjectId)
    {
        return logObject.FirstOrDefault(x => x.Id == LogObjectId);
    }



}