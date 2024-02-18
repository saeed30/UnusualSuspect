using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UnusualSuspect.Services.Services;

public interface IApplicationUserService
{
	bool ActiveDeactiveUser(int userId, bool check);
	IQueryable<UsersInRoleViewModel> CombinedAllApplicationUserSearch(CustomerSearchViewModel model);
	Task<ResultAction> CreateApplicationUser(ApplicationUser model, bool isLoginRegister);
	Task<ResultAction> DeleteApplicationUser(int applicationUserId, string userName);
	ApplicationUser? DetailsApplicationUser(long applicationUserId);
	ApplicationUser? DetailsApplicationUserWhitUserName(string userName);
	ApplicationUser DetailsUser(string userName);
	Task<ResultAction> EditApplicationUser(ApplicationUser model, bool isProfileEdit);
	List<SelectListItem> UserList(string preName);
	List<SelectListItem> TechnicalExpertList(string preName);
}

public class ApplicationUserService : IApplicationUserService
{
	private readonly ILogger<DocumentService> logger;
	private readonly ApplicationDbContext context;
	private readonly IUnitOfWork uow;
	private readonly DbSet<ApplicationUser> applicationUser;
	private readonly IDocumentService iDocumentService;
	private readonly ILogService iLogService;
	private readonly DbSet<Role> role;
	private readonly IApplicationUserManager iApplicationUserManager;
	protected readonly IUploadServise UploadServise;

	public ApplicationUserService(ILogger<DocumentService> logger, ApplicationDbContext context,
			IUnitOfWork uow, ILogService iLogService, IApplicationUserManager iApplicationUserManager,
			IUploadServise uploadServise, IDocumentService iDocumentService)
  {
    this.logger = logger;
		this.iLogService = iLogService;
		this.iDocumentService = iDocumentService;
    this.context = context;
    this.uow = uow;
		applicationUser = uow.Set<ApplicationUser>();
		role = uow.Set<Role>();
		this.iLogService = iLogService;
		this.iApplicationUserManager = iApplicationUserManager;
		UploadServise = uploadServise;
	}

	public ApplicationUser DetailsUser(string userName)
	{
		return applicationUser.Include(p => p.Document).FirstOrDefault(x => x.UserName == userName);
	}




	public List<SelectListItem> UserList(string preName)
	{
		List<SelectListItem> userList = new List<SelectListItem>
				{
						new SelectListItem() { Text = preName, Value = "" }
				};
		userList.AddRange(applicationUser.Select(u => new SelectListItem
		{
			Text = u.FullName,
			Value = u.Id.ToString()
		}).ToList());
		return userList;
	}


	public List<SelectListItem> TechnicalExpertList(string preName)
	{
		List<SelectListItem> userList = new List<SelectListItem>
				{
						new SelectListItem() { Text = preName, Value = "" }
				};

		var usersWithRoles = (from user in applicationUser
													select new
													{
														UserId = user.Id,
														Username = user.UserName,
														Email = user.Email,
														FirstName = user.FirstName,
														LastName = user.LastName,
														PhoneNumber = user.PhoneNumber,

														RoleNamesFa = (from userRole in context.UserRoles
																					 where userRole.UserId == user.Id
																					 join role in role on userRole.RoleId
																					 equals role.Id
																					 select role.Title
																						).ToList(),
														RoleNames = (from userRole in context.UserRoles
																				 where userRole.UserId == user.Id
																				 join role in role on userRole.RoleId
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

		usersWithRoles = usersWithRoles.Where(a => a.Role.Contains("TechnicalExpert"));//کارشناس فنی

		userList.AddRange(usersWithRoles.Select(u => new SelectListItem
		{
			Text = u.FirstName + u.LastName,
			Value = u.UserId.ToString()
		}).ToList());
		return userList;
	}

	public IQueryable<UsersInRoleViewModel> CombinedAllApplicationUserSearch(CustomerSearchViewModel model)
	{

		var usersWithRoles = (from user in applicationUser
													select new
													{
														UserId = user.Id,
														Username = user.UserName,
														Email = user.Email,
														FirstName = user.FirstName,
														LastName = user.LastName,
														PhoneNumber = user.PhoneNumber,

														RoleNamesFa = (from userRole in context.UserRoles
																					 where userRole.UserId == user.Id
																					 join role in role on userRole.RoleId
																					 equals role.Id
																					 select role.Title
																						).ToList(),
														RoleNames = (from userRole in context.UserRoles
																				 where userRole.UserId == user.Id
																				 join role in role on userRole.RoleId
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
			var applicationUserList2 = usersWithRoles.Where(x =>
								(x.FirstName ?? "").Contains(term)
								|| (x.LastName ?? "").Contains(term)
								|| (x.Username ?? "").Contains(term)
								|| (x.PhoneNumber ?? "").Contains(term));
			foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
			{
				applicationUserList2 = applicationUserList2.Union(usersWithRoles.Where(x =>
									(x.FirstName ?? "").Contains(tempTerm)
								 || (x.LastName ?? "").Contains(tempTerm)
									|| (x.Username ?? "").Contains(tempTerm)
									|| (x.PhoneNumber ?? "").Contains(tempTerm)));
			}
			usersWithRoles = applicationUserList2;
		}
		return usersWithRoles.AsQueryable();
	}


	public ApplicationUser? DetailsApplicationUser(long applicationUserId)
	{
		return applicationUser.FirstOrDefault(x => x.Id == applicationUserId);
	}



	/// <summary>
	/// پیدا کردن مشتری با نام کاربری
	/// </summary>
	/// <param name="userName"></param>
	/// <returns></returns>

	public ApplicationUser? DetailsApplicationUserWhitUserName(string userName)
	{
		return applicationUser.Include(p => p.Document).FirstOrDefault(x => x.UserName == userName);
	}



	public async Task<ResultAction> CreateApplicationUser(ApplicationUser model, bool isLoginRegister)
	{
		try
		{
			if (applicationUser.Any(x => x.PhoneNumber == model.PhoneNumber))
				return new ResultAction()
				{
					Success = false,
					MessageList = "این شماره همراه قبلا ثبت شده است . لطفا شماره همراه دیگری را وارد کنید"
				};


			model.UserName = model.PhoneNumber;
			model.IsActive = true;
			model.DateCreate = DateTime.Now;
			model.PhoneNumberConfirmed = true;
			model.EmailConfirmed = true;
			model.SecurityStamp = Guid.NewGuid().ToString();



			applicationUser.Add(model);
			//_uow.SaveChanges();

			iLogService.AddLog(new LogObject()
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

			//_uow.SaveChanges();

			if (!isLoginRegister && model.SoftwarerRoleList != null && model.SoftwarerRoleList.Length != 0)
			{
				var applicationUserRoleIds = model.SoftwarerRoleList.Split(',');
				foreach (var applicationUserRoleId in applicationUserRoleIds.Where(x => !string.IsNullOrEmpty(x)))
				{

					var roleId = int.Parse(applicationUserRoleId);
					var amApplicationUserRoleOb = role.FirstOrDefault(x => x.Id == roleId);
					context.UserRoles.Add(new IdentityUserRole<int>() { UserId = model.Id, RoleId = roleId });


				}
				//_context.SaveChanges();
			}
			else
			{
				var currentUser = await applicationUser.FirstOrDefaultAsync(x => x.UserName == model.UserName);
				await iApplicationUserManager.AddUserToRoleAsync(currentUser, "PublicUser");

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



	public async Task<ResultAction> EditApplicationUser(ApplicationUser model, bool isProfileEdit)
	{
		var item = DetailsApplicationUser(model.Id);
    if (item == null)
      throw new Exception("invalid userId: " + model.Id);
		if (model.ImageFile != null)
		{
			var doc = await iDocumentService.SaveFormFile(model.ImageFile, "ApplicationUser", "DocumentId", item.DocumentId);
			if (doc == null)
				return new ResultAction()
				{
					Success = false,
					MessageList = $"اشکالی در زمان ذخیره سازی فایل رخ داد",
					Id = model.Id.ToString()
				};
			item.Document = doc;
		}

		try
		{
			item.FirstName = model.FirstName;
			item.LastName = model.LastName;
			item.Email = model.Email;
			item.AvatarId = model.AvatarId;
			if (!string.IsNullOrEmpty(model.PatchImage))
				item.PatchImage = model.PatchImage;
			var itemTemp = DetailsApplicationUser(model.Id);

			iLogService.AddLog(new LogObject()
			{
				NextValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(item),
				PerValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(itemTemp),
				ObjectTypeId = "ApplicationUser",
				ObjectTypeName = "کاربر",
				DateCreate = DateTime.Now,
				UserName = model.UserName,
				Title = "ویرایش مشخصات کاربر"
			});
			//await _uow.SaveChangesAsync();

			if (!isProfileEdit)
			{
				List<int> authorsRange = new List<int>{ 1, 2, 3, 5, 12 };//necessary Roles

				context.UserRoles.RemoveRange(context.UserRoles.Where(x => x.UserId == model.Id && !authorsRange.Contains(x.RoleId)));
				//applicationRoleService.(_ApplicationUserRoles.Where(x => x.UserId == UsrOB.Id).ToList());
				//_context.SaveChanges();
				if (model.SoftwarerRoleList.Length != 0)
				{



					var applicationUserRoleIds = model.SoftwarerRoleList.Split(',');
					foreach (var applicationUserRoleId in applicationUserRoleIds.Where(x => !string.IsNullOrEmpty(x)))
					{

						var roleId = int.Parse(applicationUserRoleId);
						//var amApplicationUserRoleOB = _Role.FirstOrDefault(x => x.Id == RoleId);
						context.UserRoles.Add(new IdentityUserRole<int>() { UserId = model.Id, RoleId = roleId });


					}
					//_context.SaveChanges();
				}
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



	public async Task<ResultAction> DeleteApplicationUser(int applicationUserId, string userName)
	{
		var item = DetailsApplicationUser(applicationUserId);
		try
		{
			var currentUser = iApplicationUserManager.FindByName(item.UserName);
			iLogService.AddLog(new LogObject()
			{
				NextValue = null,
				PerValue = HelperCommon.ShallowCopyEntityToString<ApplicationUser>(item),
				ObjectTypeId = "ApplicationUser",
				ObjectTypeName = "کاربر عادی",
				DateCreate = DateTime.Now,
				UserName = userName,
				Title = "حذف کاربر عادی"
			});
			var result = await iApplicationUserManager.DeleteAsync(currentUser);
			if (result.Succeeded)
			{
				return new ResultAction()
				{
					Success = true,
					TitleResult = "موفقیت آمیز",
					MessageList = $"کاربر انتخاب شده با موفقیت حذف گردید",
				};
			}
			else
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
			var item = applicationUser.FirstOrDefault(x => x.Id == userId);
			item.IsActive = check;
			uow.SaveChanges();
			return true;
		}
		catch
		{
			return false;
		}
	}
}
