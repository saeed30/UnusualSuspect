using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Services;

public interface ISoftSettingService
{
    ResultAction EditSoftSetting(SoftSetting model);
    SoftSetting GetSoftSetting();
}

public class SoftSettingService : ISoftSettingService
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<SoftSetting> _SoftSetting;
    private readonly ILogService _ILogService;
    protected readonly IUploadServise _uploadServise;

    public SoftSettingService(IUnitOfWork uow, ILogService iLogService)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _SoftSetting = uow.Set<SoftSetting>();
        _ILogService = iLogService;
    }
    internal SoftSetting DetailsSoftSetting()
    {
        return _SoftSetting.FirstOrDefault();
    }

    public SoftSetting GetSoftSetting()
    {
        return _SoftSetting.FirstOrDefault() ?? new SoftSetting();
    }

    public ResultAction EditSoftSetting(SoftSetting model)
    {

        try
        {
            var Item = DetailsSoftSetting();
            if (Item != null)
            {

                Item.BussinessTitle = model.BussinessTitle;
                Item.SmallTitle = model.SmallTitle;
                Item.ContactUsEmail = model.ContactUsEmail;
                Item.ContactUsPhoneNumber = model.ContactUsPhoneNumber;
                Item.ContactUsMobileNumber = model.ContactUsMobileNumber;
                Item.SMSNumber = model.SMSNumber;
                Item.FaxNumber = model.FaxNumber;
                Item.Address = model.Address;
                Item.PostalCode = model.PostalCode;
                Item.SiteAdress = model.SiteAdress;
                Item.ContentContactUsPage = model.ContentContactUsPage;
                _uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    MessageList = "ویرایش مشخصات  با موفقیت انجام گردید"
                };
            }
            else
            {
                _SoftSetting.Add(model);
                _uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    MessageList = "ویرایش مشخصات  با موفقیت انجام گردید"
                };
            }
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $"در ویرایش مشخصات  خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }

}