using MassProducers.Common;
using MassProducers.DataLayer;
using MassProducers.DataLayer.Context;
using MassProducers.Entities.Identity;
using MassProducers.Entities.Models;
using MassProducers.Services.Contracts.Identity;
using MassProducers.Services.IServices;
using MassProducers.ViewModels.Models;
using MassProducers.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassProducers.Services.Services
{ 

    public class ApplicationUserService 
    {

        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CustomerAddress> _CustomerAddress;
        private readonly DbSet<ApplicationUser> _ApplicationUser;
        private readonly ILogService _ILogService;
        private readonly DbSet<Role> _Role;
        private readonly IApplicationUserManager _IApplicationUserManager;
        protected readonly IUploadServise _uploadServise;
        public ApplicationUserService(ILogger<DocumentService> logger, ApplicationDbContext context, IUnitOfWork uow, ILogService iLogService, IApplicationUserManager iApplicationUserManager, IUploadServise uploadServise)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _CustomerAddress = uow.Set<CustomerAddress>();
            _ApplicationUser = uow.Set<ApplicationUser>();
            _Role = uow.Set<Role>();
            _ILogService = iLogService;
            _IApplicationUserManager = iApplicationUserManager;
            _uploadServise = uploadServise;

        }

        public IQueryable<UsersInRoleViewModel> CombinedAllApplicationUserSearch(CustomerSearchViewModel model)
        {

            var usersWithRoles = (from user in _ApplicationUser
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      PhoneNumber = user.PhoneNumber,

                                      RoleNamesFa = (from userRole in _context.UserRoles
                                                     where userRole.UserId == user.Id
                                                     join role in _Role on userRole.RoleId
                                                     equals role.Id
                                                     select role.Title
                                                    ).ToList(),
                                      RoleNames = (from userRole in _context.UserRoles
                                                   where userRole.UserId == user.Id
                                                   join role in _Role on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name
                                                    ).ToList()
                                  }).ToList().Select(p => new UsersInRoleViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      FirstName = p.FirstName,
                                      LastName = p.LastName,
                                      PhoneNumber = p.PhoneNumber,
                                      Email = p.Email,

                                      Role = string.Join(",", p.RoleNames),
                                      RoleNamesFa = string.Join(",", p.RoleNamesFa)
                                  });


            //var ApplicationUserList = _ApplicationUser.AsQueryable();
            if (!string.IsNullOrEmpty(model.KeyWord))
            {
                var searchTerms = model.KeyWord.Split(' ');
                var term = searchTerms[0];
                var ApplicationUserList2 = usersWithRoles.Where(x =>
                          (x.FirstName ?? "").Contains(term)
                          || (x.LastName ?? "").Contains(term)
                          || (x.Username ?? "").Contains(term)
                          || (x.PhoneNumber ?? "").Contains(term));
                foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
                {
                    ApplicationUserList2 = ApplicationUserList2.Union(usersWithRoles.Where(x =>
                              (x.FirstName ?? "").Contains(tempTerm)
                             || (x.LastName ?? "").Contains(tempTerm)
                              || (x.Username ?? "").Contains(tempTerm)
                              || (x.PhoneNumber ?? "").Contains(tempTerm));
                }
                usersWithRoles = ApplicationUserList2;
            }
            return usersWithRoles.AsQueryable();
        }


        public IQueryable<UsersInRoleViewModel> CombinedApplicantApplicationUserSearch(CustomerSearchViewModel model)
        {

            var usersWithRoles = (from user in _ApplicationUser
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      PhoneNumber = user.PhoneNumber,

                                      RoleNamesFa = (from userRole in _context.UserRoles
                                                     where userRole.UserId == user.Id
                                                     join role in _Role on userRole.RoleId
                                                     equals role.Id
                                                     select role.Title
                                                    ).ToList(),
                                      RoleNames = (from userRole in _context.UserRoles
                                                   where userRole.UserId == user.Id
                                                   join role in _Role on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name
                                                    ).ToList()
                                  }).ToList().Select(p => new UsersInRoleViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      FirstName = p.FirstName,
                                      LastName = p.LastName,
                                      PhoneNumber = p.PhoneNumber,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames),
                                      RoleNamesFa = string.Join(",", p.RoleNamesFa)
                                  });

            usersWithRoles = usersWithRoles.Where(a => a.Role.Contains("Applicant"));

            //var ApplicationUserList = _ApplicationUser.AsQueryable();
            if (!string.IsNullOrEmpty(model.KeyWord))
            {
                var searchTerms = model.KeyWord.Split(' ');
                var term = searchTerms[0];
                var ApplicationUserList2 = usersWithRoles.Where(x =>
                          (x.FirstName ?? "").Contains(term)
                          || (x.LastName ?? "").Contains(term)
                          || (x.Username ?? "").Contains(term)
                          || (x.PhoneNumber ?? "").Contains(term);
                foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
                {
                    ApplicationUserList2 = ApplicationUserList2.Union(usersWithRoles.Where(x =>
                              (x.FirstName ?? "").Contains(tempTerm)
                             || (x.LastName ?? "").Contains(tempTerm)
                              || (x.Username ?? "").Contains(tempTerm)
                              || (x.PhoneNumber ?? "").Contains(tempTerm);
                }
                usersWithRoles = ApplicationUserList2;
            }
            return usersWithRoles.AsQueryable();
        }



        public ApplicationUser DetailsApplicationUser(long ApplicationUserId)
        {
            return _ApplicationUser.FirstOrDefault(x => x.Id == ApplicationUserId);
        }



        /// <summary>
        /// پیدا کردن مشتری با نام کاربری
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>

        public ApplicationUser DetailsApplicationUserWhitUserName(string UserName)
        {
            return _ApplicationUser.FirstOrDefault(x => x.UserName == UserName);
        }



        public async Task<ResultAction> CreateApplicationUser(ApplicationUser model, bool IsLoginRegister)
        {
            try
            {
                if (_ApplicationUser.Any(x => x.PhoneNumber == model.PhoneNumber))
                    return new ResultAction()
                    {
                        Success = false,
                        MessageList = "این شماره همراه قبلا ثبت شده است . لطفا شماره همراه دیگری را وارد کنید"
                    };


                string Password = model.Password;

                //model.ApplicationUser = new ApplicationUser()
                //{
                //    FirstName = model.FirstName,
                //    LastName = model.LastName,
                //    PhoneNumber = model.PhoneNumber,
                //    UserName = model.ApplicationUser.PhoneNumber,
                //    Password = model.ApplicationUser.Password,
                //    IsActive = true,
                //    DateCreate = DateTime.Now,
                //    PhoneNumberConfirmed = true,
                //    EmailConfirmed = true,
                //    SecurityStamp = Guid.NewGuid().ToString(),
                //};


                await _ApplicationUser.AddAsync(model);
                _uow.SaveChanges();

                _ILogService.AddLog(new LogObject()
                {
                    NextValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(model),
                    PerValue = null,
                    ObjectTypeId = "ApplicationUser",
                    ObjectTypeName = " کاربر",
                    DateCreate = DateTime.Now,
                    UserName = model.UserName,
                    UserId = model.Id,
                    Title = "ایجاد کاربر"
                });

                _uow.SaveChanges();

                if (!IsLoginRegister && model.SoftwarerRoleList.Length != 0)
                {
                    var ApplicationUserRoleIds = model.SoftwarerRoleList.Split(',');
                    foreach (var ApplicationUserRoleId in ApplicationUserRoleIds.Where(x => !string.IsNullOrEmpty(x)))
                    {

                        var RoleId = int.Parse(ApplicationUserRoleId);
                        var AmApplicationUserRoleOB = _Role.FirstOrDefault(x => x.Id == RoleId);
                        _context.UserRoles.Add(new IdentityUserRole<int>() { UserId = model.Id, RoleId = RoleId });


                    }
                    _context.SaveChanges();
                }
                else
                {
                    var currentUser = _ApplicationUser.FirstOrDefault(x => x.UserName == model.UserName);
                    await _IApplicationUserManager.AddPasswordAsync(currentUser, Password);
                    await _IApplicationUserManager.AddUserToRoleAsync(currentUser, "PublicUser");

                }

                return new ResultAction()
                {
                    Success = true,
                    Id = model.Id.ToString(),
                    MessageList = $"کاربر با موفقیت ثبت گردید",

                };
            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = " در ثبت مشتری خطایی رخ داده است " + HelperCommon.ReturnMessageException(e)
                };
            }
        }



        public async Task<ResultAction> EditApplicationUser(ApplicationUser model)
        {

            if (!_uploadServise.DeletePictureUser(model).Success)
                return new ResultAction()
                {
                    Success = false,
                    MessageList = "در ویرایش تصویر کاربر خطایی به وجود آمده است"
                };


            var CheckUploade = _uploadServise.IsUpload(model.ImageFile, false);
            if (!CheckUploade.Success)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = CheckUploade.MessageList
                };
            }
            var UploadeFile = _uploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\UserImage", true);
            model.PatchImage = UploadeFile.Result.MessageList;


            var Item = DetailsApplicationUser(model.Id);
            try
            {
                Item.FirstName = model.FirstName;
                Item.LastName = model.LastName;
                Item.Email = model.Email;
                if (!string.IsNullOrEmpty(model.PatchImage))
                    Item.PatchImage = model.PatchImage;
                var ItemTemp = DetailsApplicationUser(model.Id);

                _ILogService.AddLog(new LogObject()
                {
                    NextValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(Item),
                    PerValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(ItemTemp),
                    ObjectTypeId = "ApplicationUser",
                    ObjectTypeName = "کاربر",
                    DateCreate = DateTime.Now,
                    UserName = model.UserName,
                    Title = "ویرایش مشخصات کاربر"
                });
                await _uow.SaveChangesAsync();

                _context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == model.Id));
                //applicationRoleService.(_ApplicationUserRoles.Where(x => x.UserId == UsrOB.Id).ToList());
                _context.SaveChanges();
                if (model.SoftwarerRoleList.Length != 0)
                {



                    var ApplicationUserRoleIds = model.SoftwarerRoleList.Split(',');
                    foreach (var ApplicationUserRoleId in ApplicationUserRoleIds.Where(x => !string.IsNullOrEmpty(x)))
                    {

                        var RoleId = int.Parse(ApplicationUserRoleId);
                        var AmApplicationUserRoleOB = _Role.FirstOrDefault(x => x.Id == RoleId);
                        _context.UserRoles.Add(new IdentityUserRole<int>() { UserId = model.Id, RoleId = RoleId });


                    }
                    _context.SaveChanges();
                }

                return new ResultAction()
                {
                    Success = true,
                    MessageList = $"ویرایش با موفقیت انجام شد",
                    Id = model.Id.ToString()

                };
            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = HelperCommon.ReturnMessageException(e)
                };
            }
        }



        public async Task<ResultAction> DeleteApplicationUser(int ApplicationUserId, string UserName)
        {
            var Item = DetailsApplicationUser(ApplicationUserId);
            try
            {
                var currentUser = _IApplicationUserManager.FindByName(Item.UserName);
                _ILogService.AddLog(new LogObject()
                {
                    NextValue = null,
                    PerValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(Item),
                    ObjectTypeId = "ApplicationUser",
                    ObjectTypeName = "کاربر عادی",
                    DateCreate = DateTime.Now,
                    UserName = UserName,
                    Title = "حذف کاربر عادی"
                });
                //var Result = await _IApplicationUserManager.DeleteAsync(currentUser);
                var Result = _context.Set<ApplicationUser>().Remove(currentUser);

                return new ResultAction()
                {
                    Success = false,
                };
            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    TitleResult = "خطا",
                    MessageList = $"در حذف کاربر خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
                };
            }
        }



        public bool ActiveDeactiveUser(int userId, bool check)
        {
            try
            {
                var item = _ApplicationUser.FirstOrDefault(x => x.Id == userId);
                item.IsActive = check;
                _uow.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        #region CustomerAddress

        public CustomerAddress DetailsCustomerAddress(int CustomerAddressId)
        {
            return _CustomerAddress.FirstOrDefault(x => x.Id == CustomerAddressId);
        }


        public async Task<ResultAction> CreateCustomerAddress(CustomerAddress model)
        {
            try
            {
                await _CustomerAddress.AddAsync(model);
                _uow.SaveChanges();

                _ILogService.AddLog(new LogObject()
                {
                    NextValue = HelperCommon.ShallowCopyEntityToString<CustomerAddress>(model),
                    PerValue = null,
                    ObjectTypeId = "CustomerAddress",
                    ObjectTypeName = " آدرس",
                    DateCreate = DateTime.Now,
                    UserName = model.UserName,
                    UserId = model.UserId,
                    Title = "ایجاد آدرس"
                });

                _uow.SaveChanges();

                return new ResultAction()
                {
                    Success = true,
                    MessageList = $"آدرس با موفقیت ثبت گردید",
                    Id = model.Id.ToString()

                };


            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = " در ثبت آدرس خطایی رخ داده است " + HelperCommon.ReturnMessageException(e)
                };
            }
        }



        public async Task<ResultAction> EditCustomerAddress(CustomerAddress model)
        {

            var Item = DetailsCustomerAddress(model.Id);
            try
            {
                Item.Lname = model.Lname;
                Item.Fname = model.Fname;
                Item.CodePost = model.CodePost;
                Item.CityId = model.CityId;
                Item.MobileNumber = model.MobileNumber;
                Item.NationalNumber = model.NationalNumber;
                Item.PhoneNumber = model.PhoneNumber;
                Item.StateId = model.StateId;
                Item.Address = model.Address;

                var ItemTemp = DetailsCustomerAddress(model.Id);

                _ILogService.AddLog(new LogObject()
                {
                    NextValue = HelperCommon.ShallowCopyEntityToString<CustomerAddress>(Item),
                    PerValue = HelperCommon.ShallowCopyEntityToString<CustomerAddress>(ItemTemp),
                    ObjectTypeId = "CustomerAddress",
                    ObjectTypeName = "آدرس",
                    DateCreate = DateTime.Now,
                    UserName = model.UserName,
                    Title = "ویرایش مشخصات آدرس"
                });
                await _uow.SaveChangesAsync();
                return new ResultAction()
                {
                    Success = true,
                    MessageList = $"ویرایش با موفقیت انجام شد"

                };
            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    MessageList = HelperCommon.ReturnMessageException(e)
                };
            }
        }



        public async Task<ResultAction> DeleteCustomerAddress(int CustomerAddressId)
        {
            var Item = DetailsCustomerAddress(CustomerAddressId);
            try
            {
                _ILogService.AddLog(new LogObject()
                {
                    NextValue = null,
                    PerValue = HelperCommon.ShallowCopyEntityToString<CustomerAddress>(Item),
                    ObjectTypeId = "CustomerAddress",
                    ObjectTypeName = " آدرس",
                    DateCreate = DateTime.Now,
                    UserName = Item.UserName,
                    Title = "حذف  آدرس"
                });
                _CustomerAddress.Remove(Item);

                _uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    TitleResult = "حذف گروه مطالب",
                    MessageList = "حذف گروه با موفقیت انجام شد"
                };
            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    TitleResult = "خطا",
                    MessageList = $"در حذف آدرس خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
                };
            }
        }
        public List<CustomerAddress> CustomerAddressesList(string username)
        {
            return _CustomerAddress.Where(x => x.ApplicationUser.UserName == username).OrderByDescending(p => p.Id).ToList();
        }

        #endregion








    }
}
