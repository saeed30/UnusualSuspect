using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Settings;
using UnusualSuspect.DataLayer.Contracts.Repository;

namespace UnusualSuspect.Services.Services;

public interface ISoftSettingService
{
  ResultAction EditSoftSetting(SoftSetting model);
  SoftSetting GetSoftSetting();
  Task<SoftSetting> GetSoftSettingAsync(CancellationToken cancellationToken = default);
}

public class SoftSettingService(IUnitOfWork uow, ILogService iLogService, ISoftSettingRepository softSettingRepository) : ISoftSettingService
{
  public SoftSetting GetSoftSetting()
  {
    return softSettingRepository.Get() ?? new SoftSetting();
  }
  public async Task<SoftSetting> GetSoftSettingAsync(CancellationToken cancellationToken = default)
  {
    return await softSettingRepository.GetAsync(cancellationToken) ?? new SoftSetting();
  }

  public ResultAction EditSoftSetting(SoftSetting model)
  {
    try
    {
      var item = softSettingRepository.Get(true);
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
        softSettingRepository.Update(model);
      }
      else
        softSettingRepository.Add(model);
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