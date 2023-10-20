using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DNTPersianUtils.Core;

namespace UnusualSuspect.Services.Services;

public interface IApplicationUserService
{
	bool ActiveDeactiveUser(int userId, bool check);
	IQueryable<UsersInRoleViewModel> CombinedAllApplicationUserSearch(CustomerSearchViewModel model);
	Task<ResultAction> CreateApplicationUser(ApplicationUser model, bool IsLoginRegister);
	Task<ResultAction> DeleteApplicationUser(int ApplicationUserId, string UserName);
	ApplicationUser? DetailsApplicationUser(long ApplicationUserId);
	ApplicationUser? DetailsApplicationUserWhitUserName(string UserName);
	ApplicationUser DetailsUser(string userName);
	Task<ResultAction> EditApplicationUser(ApplicationUser model, bool IsProfileEdit);
	List<SelectListItem> UserList(string PreName);
	List<SelectListItem> TechnicalExpertList(string PreName);
}

public class ApplicationUserService : IApplicationUserService
{
	private readonly ILogger<DocumentService> _logger;
	private readonly ApplicationDbContext _context;
	private readonly IUnitOfWork _uow;
	private readonly DbSet<ApplicationUser> _ApplicationUser;
	private readonly IDocumentService _IDocumentService;
	private readonly ILogService _ILogService;
	private readonly DbSet<Role> _Role;
	private readonly IApplicationUserManager _IApplicationUserManager;
	protected readonly IUploadServise _uploadServise;

	public ApplicationUserService(ILogger<DocumentService> logger, ApplicationDbContext context,
			IUnitOfWork uow, ILogService iLogService, IApplicationUserManager iApplicationUserManager,
			IUploadServise uploadServise, IDocumentService iDocumentService)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(_logger));
		_ILogService = iLogService;
		_IDocumentService = iDocumentService;
		_context = context ?? throw new ArgumentNullException(nameof(_context));
		_uow = uow ?? throw new ArgumentNullException(nameof(_uow));
		_ApplicationUser = uow.Set<ApplicationUser>();
		_Role = uow.Set<Role>();
		_ILogService = iLogService;
		_IApplicationUserManager = iApplicationUserManager;
		_uploadServise = uploadServise;
	}

	public ApplicationUser DetailsUser(string userName)
	{
		return _ApplicationUser.Include(p => p.Document).FirstOrDefault(x => x.UserName == userName);
	}




	public List<SelectListItem> UserList(string PreName)
	{
		List<SelectListItem> UserList = new List<SelectListItem>
				{
						new SelectListItem() { Text = PreName, Value = "" }
				};
		UserList.AddRange(_ApplicationUser.Select(u => new SelectListItem
		{
			Text = u.FullName,
			Value = u.Id.ToString()
		}).ToList());
		return UserList;
	}


	public List<SelectListItem> TechnicalExpertList(string PreName)
	{
		List<SelectListItem> UserList = new List<SelectListItem>
				{
						new SelectListItem() { Text = PreName, Value = "" }
				};

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

		usersWithRoles = usersWithRoles.Where(a => a.Role.Contains("TechnicalExpert"));//کارشناس فنی

		UserList.AddRange(usersWithRoles.Select(u => new SelectListItem
		{
			Text = u.FirstName + u.LastName,
			Value = u.UserId.ToString()
		}).ToList());
		return UserList;
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
									|| (x.PhoneNumber ?? "").Contains(tempTerm)));
			}
			usersWithRoles = ApplicationUserList2;
		}
		return usersWithRoles.AsQueryable();
	}


	public ApplicationUser? DetailsApplicationUser(long ApplicationUserId)
	{
		return _ApplicationUser.FirstOrDefault(x => x.Id == ApplicationUserId);
	}



	/// <summary>
	/// پیدا کردن مشتری با نام کاربری
	/// </summary>
	/// <param name="UserName"></param>
	/// <returns></returns>

	public ApplicationUser? DetailsApplicationUserWhitUserName(string UserName)
	{
		return _ApplicationUser.Include(p => p.Document).FirstOrDefault(x => x.UserName == UserName);
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


			model.UserName = model.PhoneNumber;
			model.IsActive = true;
			model.DateCreate = DateTime.Now;
			model.PhoneNumberConfirmed = true;
			model.EmailConfirmed = true;
			model.SecurityStamp = Guid.NewGuid().ToString();



			await _ApplicationUser.AddAsync(model);
			//_uow.SaveChanges();

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

			//_uow.SaveChanges();

			if (!IsLoginRegister && model.SoftwarerRoleList != null && model.SoftwarerRoleList.Length != 0)
			{
				var ApplicationUserRoleIds = model.SoftwarerRoleList.Split(',');
				foreach (var ApplicationUserRoleId in ApplicationUserRoleIds.Where(x => !string.IsNullOrEmpty(x)))
				{

					var RoleId = int.Parse(ApplicationUserRoleId);
					var AmApplicationUserRoleOB = _Role.FirstOrDefault(x => x.Id == RoleId);
					_context.UserRoles.Add(new IdentityUserRole<int>() { UserId = model.Id, RoleId = RoleId });


				}
				//_context.SaveChanges();
			}
			else
			{
				var currentUser = await _ApplicationUser.FirstOrDefaultAsync(x => x.UserName == model.UserName);
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



	public async Task<ResultAction> EditApplicationUser(ApplicationUser model, bool IsProfileEdit)
	{
		var Item = DetailsApplicationUser(model.Id);

		if (model.ImageFile != null)
		{
			var doc = await _IDocumentService.SaveFormFile(model.ImageFile, "ApplicationUser", "DocumentId", Item.DocumentId);
			if (doc == null)
				return new ResultAction()
				{
					Success = false,
					MessageList = $"اشکالی در زمان ذخیره سازی فایل رخ داد",
					Id = model.Id.ToString()
				};
			Item.Document = doc;
		}

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
			//await _uow.SaveChangesAsync();

			if (!IsProfileEdit)
			{
				int[] RolesId = { 1, 2, 3, 5, 12 };//necessary Roles
				List<int> authorsRange = new List<int>(RolesId);

				_context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == model.Id && !authorsRange.Contains(x.RoleId)));
				//applicationRoleService.(_ApplicationUserRoles.Where(x => x.UserId == UsrOB.Id).ToList());
				//_context.SaveChanges();
				if (model.SoftwarerRoleList.Length != 0)
				{



					var ApplicationUserRoleIds = model.SoftwarerRoleList.Split(',');
					foreach (var ApplicationUserRoleId in ApplicationUserRoleIds.Where(x => !string.IsNullOrEmpty(x)))
					{

						var RoleId = int.Parse(ApplicationUserRoleId);
						var AmApplicationUserRoleOB = _Role.FirstOrDefault(x => x.Id == RoleId);
						_context.UserRoles.Add(new IdentityUserRole<int>() { UserId = model.Id, RoleId = RoleId });


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
			var Result = await _IApplicationUserManager.DeleteAsync(currentUser);
			if (Result.Succeeded)
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
}
