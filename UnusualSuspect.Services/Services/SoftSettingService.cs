using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;

namespace UnusualSuspect.Services.Services;

public interface ISoftSettingService
{
    ResultAction EditSoftSetting(SoftSetting model);
    SoftSetting GetSoftSetting();
}

public class SoftSettingService(IUnitOfWork uow, ILogService iLogService) : ISoftSettingService
{
    private readonly DbSet<SoftSetting> softSetting = uow.Set<SoftSetting>();

    internal SoftSetting? DetailsSoftSetting()
    {
        return softSetting.FirstOrDefault();
    }

    public SoftSetting GetSoftSetting()
    {
        return softSetting.FirstOrDefault() ?? new SoftSetting();
    }

    public ResultAction EditSoftSetting(SoftSetting model)
    {

        try
        {
            var item = DetailsSoftSetting();
            if (item != null)
            {

                item.BussinessTitle = model.BussinessTitle;
                item.SmallTitle = model.SmallTitle;
                item.ContactUsEmail = model.ContactUsEmail;
                item.ContactUsPhoneNumber = model.ContactUsPhoneNumber;
                item.ContactUsMobileNumber = model.ContactUsMobileNumber;
                item.SMSNumber = model.SMSNumber;
                item.FaxNumber = model.FaxNumber;
                item.Address = model.Address;
                item.PostalCode = model.PostalCode;
                item.SiteAdress = model.SiteAdress;
                item.ContentContactUsPage = model.ContentContactUsPage;
                uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    MessageList = "ویرایش مشخصات  با موفقیت انجام گردید"
                };
            }

            softSetting.Add(model);
            uow.SaveChanges();
            return new ResultAction()
            {
              Success = true,
              MessageList = "ویرایش مشخصات  با موفقیت انجام گردید"
            };
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