using UnusualSuspect.Common;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.ApiViewModels.Endpoints.Document;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.Services.Services;

public class DocumentService : IDocumentService
{
	private readonly IUnitOfWork uow;
	private readonly DbSet<Document> document;

	private readonly ILogService iLogService;
	public readonly IUploadServise IuploadServise;

	public DocumentService(ILogger<DocumentService> logger, IUnitOfWork uow, ILogService iLogService, IUploadServise uploadServise)
	{
		this.uow = uow ?? throw new ArgumentNullException(nameof(this.uow));
		document = uow.Set<Document>();
		this.iLogService = iLogService;
		IuploadServise = uploadServise;
		//_sharedLocalizer = sharedLocalizer;
	}
	public List<Document> DocumentSearch(DocumentSearchViewModel model, bool? isActive = null)
	{
		model.StartDate ??= DateTime.MinValue;
		model.EndDate ??= DateTime.MaxValue;
		var documentList = document.Where(x => (isActive == null) && (model.DocumentGroupId == null)
																								&& (model.Active == null)
																								&& (model.Lang == null)
																								&& ((model.StartDate <= x.ModifyDate && model.EndDate >= x.ModifyDate)));
		if (!string.IsNullOrEmpty(model.KeyWord))
		{
			var searchTerms = model.KeyWord.Split(' ');
			var term = searchTerms[0];
			var documentList2 = documentList.Where(x =>
								//(x.Title ?? "").Contains(term)
								//|| (x.Tags ?? "").Contains(term)
								//|| 
								(x.Id.ToString() == term));
			foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
			{
				documentList2 = documentList2.Union(documentList.Where(x =>
							//(x.Title ?? "").Contains(tempTerm)
							//|| (x.Tags ?? "").Contains(tempTerm)
							//|| 
							(x.Id.ToString() == tempTerm)));
			}
			documentList = documentList2;
		}

		switch (model.SortTypeId)
		{
			case 1:
				{
					documentList = documentList.OrderByDescending(x => x.ModifyDate);
					break;
				}
			case 2:
				{
					documentList = documentList.OrderBy(x => x.ModifyDate);
					break;
				}
			default:
				{
					documentList = documentList.OrderByDescending(x => x.ModifyDate);
					break;
				}
		}
		model.ResultCount = documentList.Count();
		return documentList.Skip((model.Step - 1) * model.PageSize).Take(model.PageSize).ToList();

	}

	public async Task<Document?> GetDocumentAsync(int id)
	{
		return await document.FirstOrDefaultAsync(m => m.Id == id);
	}
	public async Task<UnusualSuspectServiceResult<DocumentGetResponse>> GetDocumentByGuidKeyAsync(Guid guidKey)
	{
		var doc = await document.FirstOrDefaultAsync(m => m.GuidKey == guidKey);
    if (doc == null)
      return new UnusualSuspectServiceResult<DocumentGetResponse>(new UnusualSuspectErrorResult(LogicErrorCode.FileNotFound));
    return new UnusualSuspectServiceResult<DocumentGetResponse>(new DocumentGetResponse()

    {
      File = doc.File,
      FileName = doc.DocumentName
    });
  }

	public Document? GetDocument(int id)
	{
		return document.FirstOrDefault(m => m.Id == id);
	}
	public ResultAction DeleteDocumentComment(int documentCommentId, string userName)
	{
		//var Item = DetailsDocumentComment(DocumentCommentId);
		try
		{
			//_DocumentComment.Remove(Item);
			//_ILogService.AddLog(new LogObject()
			//{
			//    NextValue = null,
			//    //PerValue = HelperCommon.ShallowCopyEntityToString<DocumentComment>(Item),
			//    ObjectTypeId = "DocumentComment",
			//    ObjectTypeName = "DocumentComment",
			//    Title = "Delete DocumentComment",
			//    DateCreate = DateTime.Now,
			//    UserName = UserName
			//});
			//_uow.SaveChanges();
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
				MessageList = HelperCommon.ReturnMessageException(e)
			};
		}
	}
	public bool ConfirmDocumentComment(int documentCommentId, bool check)
	{
		try
		{
			//var item = DetailsDocumentComment(DocumentCommentId);
			//item.Confirm = check;
			//_uow.SaveChanges();
			return true;
		}
		catch
		{
			return false;
		}
	}
	public int NotConfirmComments()
	{
		return 0; // _DocumentComment.Count(x => x.Confirm == false);
	}

	//public List<DocumentComment> CombinedAllDocumentCommentsearch(DocumentCommentSearchViewModel model)
	//{
	//    var DocumentCommentList = _DocumentComment.AsQueryable();
	//    if (!string.IsNullOrEmpty(model.KeyWord))
	//    {
	//        var searchTerms = model.KeyWord.Split(' ');
	//        var term = searchTerms[0];
	//        var DocumentCommentList2 = DocumentCommentList.Where(x =>
	//                  (x.CommentText ?? "").Contains(term)
	//                  || (x.Id.ToString() == term));
	//        foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
	//        {
	//            DocumentCommentList2 = DocumentCommentList2.Union(DocumentCommentList.Where(x =>
	//                  (x.CommentText ?? "").Contains(tempTerm)
	//                  || (x.Id.ToString() == tempTerm)));
	//        }
	//        DocumentCommentList = DocumentCommentList2;
	//    }

	//    switch (model.SortTypeId)
	//    {
	//        case 1:
	//            {
	//                DocumentCommentList = DocumentCommentList.OrderByDescending(x => x.Id);
	//                break;
	//            }
	//        case 2:
	//            {
	//                DocumentCommentList = DocumentCommentList.OrderBy(x => x.Id);
	//                break;
	//            }
	//        default:
	//            {
	//                DocumentCommentList = DocumentCommentList.OrderByDescending(x => x.Id);
	//                break;
	//            }
	//    }
	//    model.ResultCount = DocumentCommentList.Count();
	//    return DocumentCommentList.Skip((model.Step - 1) * model.PageSize).Take(model.PageSize).ToList();
	//}

	//public List<DocumentComment> DocumentCommentList(int DocumentId, int Step, ApplicationDbContext Db)
	//{
	//    return _DocumentComment.Where(x => x.DocumentId == DocumentId && x.Confirm).OrderByDescending(x => x.Id).Skip(Step * 25).Take(25).ToList();
	//}

	public void AddViewCount(Document item)
	{
		//Item.ViewCount = (Item.ViewCount + 1);
		uow.SaveChanges();
	}

	public Document DetailsDocument(long documentId)
	{
		return document.FirstOrDefault(x => x.Id == documentId);
	}
	public List<int> DocumentCategoryList(int documentId)
	{
		return null; // _DocumentCategory.Where(x => x.DocumentId == DocumentId).Select(p => p.CategoryId).ToList();
	}

	public IQueryable<Document> CombinedAllDocumentSearch(DocumentSearchViewModel model)
	{
		model.StartDate = model.StartDate ?? DateTime.MinValue;
		model.EndDate = model.EndDate ?? DateTime.MaxValue;
		var documentList = document.Where(x => (model.DocumentGroupId == null /*|| x.DocumentGroupId == model.DocumentGroupId*/));
		if (!string.IsNullOrEmpty(model.KeyWord))
		{
			var searchTerms = model.KeyWord.Split(' ');
			var term = searchTerms[0];
			var documentList2 = documentList.Where(x =>
								//(x.Title ?? "").Contains(term)
								//||
								(x.Id.ToString() == term));
			foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
			{
				documentList2 = documentList2.Union(documentList.Where(x =>
							//(x.Title ?? "").Contains(tempTerm)
							//|| 
							(x.Id.ToString() == tempTerm)));
			}
			documentList = documentList2;
		}

		return documentList;

	}
	public int LikeDisLikeDocumentComment(IHttpContextAccessor httpContextAccessor, int documentId, int commentId, bool like)
	{
		if (httpContextAccessor.HttpContext.Session.GetString($"DocumentComment{documentId}{commentId}") == null)
		{
			httpContextAccessor.HttpContext.Session.SetString($"DocumentComment{documentId}{commentId}", $"{documentId}{commentId}");
			return LikeDisLikeComment(commentId, like);
		}
		else
		{
			return 0;
		}
	}


	//public DocumentComment DetailsDocumentComment(int DocumentId, long DocumentCommentId)
	//{
	//    return _DocumentComment.FirstOrDefault(x => x.DocumentId == DocumentId && x.Id == DocumentCommentId);
	//}

	public int LikeDisLikeComment(int commentId, bool like)
	{
		//var Item = _DocumentComment.FirstOrDefault(x => x.Id == CommentId);
		//if (Item != null)
		//{
		//    if (Like)
		//    {
		//        Item.LikeCount += 1;
		//        _uow.SaveChanges();
		//        return Item.LikeCount;
		//    }
		//    else
		//    {
		//        Item.DisLikeCount += 1;
		//        _uow.SaveChanges();
		//        return Item.DisLikeCount;
		//    }
		//}
		return 0;
	}

	/// <summary>
	/// ثبت نظر برای مطلب
	/// </summary>
	/// <param name="UserName"></param>
	/// <param name="CommentText"></param>
	/// <param name="DocumentId"></param>
	/// <param name="RateValue"></param>
	/// <returns></returns>
	//public ResultAction AddDocumentComment(DocumentComment model)
	//{
	//    try
	//    {
	//        model.CommentDate = DateTime.Now;
	//        _DocumentComment.Add(model);
	//        _uow.SaveChanges();
	//        return new ResultAction()
	//        {
	//            Success = true,
	//            MessageList = "نظر شما با موفقیت ثبت گردید. بعد از تائید توسط مدیریت در بخش نظرات نمایش داده می شود"
	//        };
	//    }
	//    catch
	//    {
	//        return new ResultAction()
	//        {
	//            Success = false,
	//            MessageList = "در ثبت نظر خطایی رخ  داده است"
	//        };
	//    }
	//}
	public ResultAction CreateDocument(Document model)
	{
		try
		{
			//var CheckUploade = _IuploadServise.IsUpload(model.ImageFile, false);
			//var CheckUploadeVideo = _IuploadServise.IsUpload(model.VideoFile, false, Common.Enums.ExtensionFileEnum.Video, 2000000);

			//if (!CheckUploade.Success || !CheckUploadeVideo.Success)
			//{
			//    return new ResultAction()
			//    {
			//        Success = false,
			//        MessageList = CheckUploade.MessageList + " " + CheckUploadeVideo.MessageList
			//    };
			//}
			//var UploadeFile = _IuploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\Document", true);
			//model.PatchImage = UploadeFile.Result.MessageList;

			//var UploadeVideoFile = _IuploadServise.SaveFileAsync(model.VideoFile, $"wwwroot\\media\\Document", true);
			//model.PatchVideo = UploadeVideoFile.Result.MessageList;

			//model.Tags = model.ArrayTags != null ? string.Join(',', model.ArrayTags) : null;
			//model.ModifyDate = DateTime.Now;

			//_Document.Add(model);
			//_uow.SaveChanges();

			//if (model.CategoryIdListS.Count() > 0)
			//{
			//    //_DocumentCategory.RemoveRange(_DocumentCategory.Where(p => p.DocumentId == model.Id ));

			//    var DocumentCategory = new DocumentCategory();
			//    foreach (var item in model.CategoryIdListS)
			//    {
			//        _DocumentCategory.Add(new DocumentCategory { DocumentId = model.Id, CategoryId = item, AddDate = DateTime.Now });

			//    }
			//}

			//_ILogService.AddLog(new LogObject()
			//{
			//    NextValue = HelperCommon.ShallowCopyEntityToString<Document>(model),
			//    PerValue = null,
			//    ObjectTypeId = "Document",
			//    ObjectTypeName = "Document",
			//    Title = "Create Document",
			//    DateCreate = DateTime.Now,
			//    UserName = model.UserName
			//});
			//var result = _uow.SaveChanges();
			return new ResultAction()
			{
				Success = true,
				Id = model.Id.ToString(),
				MessageList = $"سند {model.Id} با موفقیت ثبت گردید",
			};
		}
		catch (Exception e)
		{
			return new ResultAction()
			{
				Success = false,
				MessageList = $"در ثبت سند {model.Id} خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}",
			};
		}

	}
	public async Task<Document?> SaveFormFile(IFormFile file, string tableName, string keyName, int? documentId = null)
	{
		if (file == null || file.Length < 1)
			return null;
		var extension = Path.GetExtension(file.FileName);
		if (extension.IsNull())
			extension = "txt";
		if (extension.Contains("."))
			extension = extension.Replace(".", "");
		if (tableName.IsNull())
			throw new Exception("tableName is null in SaveFormFile");
		tableName = tableName.ToLower();
		if (keyName.IsNull())
			throw new Exception("keyName is null in SaveFormFile");
		keyName = keyName.ToLower();
		var uploadeFile = IuploadServise.GetFileDataAsync(file);
		Document? document = null;
		if (documentId.HasValue)
			document = await this.document.FirstOrDefaultAsync(x => x.Id == documentId.Value);
		if (document == null)
		{
			document = new Document
			{
				DocumentName = IuploadServise.GetUniqueFileName(file),
				File = uploadeFile.Result,
				DocumentType = extension,
				ModifyDate = DateTime.Now,
				TableName = tableName,
				KeyName = keyName,
				GuidKey = Guid.NewGuid()
			};
			this.document.Add(document);
		}
		else
		{
			document.DocumentName = IuploadServise.GetUniqueFileName(file);
			document.File = uploadeFile.Result;
			document.DocumentType = extension;
			document.ModifyDate = DateTime.Now;
			document.TableName = tableName;
			document.KeyName = keyName;
		}
		return document;
	}
	public ResultAction CreateDocumentFile(IFormFile file, string tableName, ExtensionFileEnum extensionFile = ExtensionFileEnum.Image)
	{
		if (file != null)
		{
			var extension = Path.GetExtension(file.FileName);
			var checkUploade = IuploadServise.IsUpload(file, false, extensionFile);

			if (!checkUploade.Success)
			{
				return new ResultAction()
				{
					Success = false,
					MessageList = checkUploade.MessageList
				};
			}
			string documentName = IuploadServise.GetUniqueFileName(file);

			//if (Extension == ".ogg" || Extension == ".mp4")
			//{
			//	var UploadeVideoFile = _IuploadServise.SaveFileAsync(file, $"wwwroot\\media\\Document", true);
			//	documentName = UploadeVideoFile.Result.MessageList;

			//}

			var uploadeFile = IuploadServise.GetFileDataAsync(file);

			var document = new Document
			{
				ModifyDate = DateTime.Now,
				File = uploadeFile.Result,
				DocumentType = extension,
				TableName = tableName,
				DocumentName = documentName,
				GuidKey = Guid.NewGuid()
			};

			this.document.Add(document);
			uow.SaveChanges();



			return new ResultAction()
			{
				Success = true,
				Id = document.Id.ToString(),
				MessageList = $" با موفقیت ثبت گردید",
			};
		}
		return new ResultAction()
		{
			Success = false,
			MessageList = $"فایل را انتخاب نمایید",
		};
	}

	public ResultAction EditDocument(Document model)
	{
		var item = DetailsDocument(model.Id);
		try
		{
			//var CheckUploade = _IuploadServise.IsUpload(model.ImageFile, false);
			//if (!CheckUploade.Success)
			//{

			//    return new ResultAction()
			//    {
			//        Success = false,
			//        MessageList = CheckUploade.MessageList
			//    };
			//}
			//var UploadeFile = _IuploadServise.SaveFileAsync(model.ImageFile, $"wwwroot\\media\\Document", true);
			//model.PatchImage = UploadeFile.Result.MessageList;
			//model.Tags = model.ArrayTags != null ? string.Join(',', model.ArrayTags) : null;


			//Item.LanguageId = model.LanguageId;
			//Item.Title = model.Title;
			//Item.Summery = model.Summery;
			//Item.MainText = model.MainText;
			//Item.Tags = model.Tags;
			//Item.Selected = model.Selected;
			//Item.ModifyDate = model.ModifyDate;
			//if (!string.IsNullOrEmpty(model.PatchImage))
			//    Item.PatchImage = model.PatchImage;

			//_ILogService.AddLog(new LogObject()
			//{
			//    NextValue = HelperCommon.ShallowCopyEntityToString<Document>(Item),
			//    PerValue = HelperCommon.ShallowCopyEntityToString<Document>(DetailsDocument(model.Id)),
			//    ObjectTypeId = "Document",
			//    ObjectTypeName = "Document",
			//    Title = "Update Document",
			//    DateCreate = DateTime.Now,
			//    UserName = model.UserName
			//});

			//if (model.CategoryIdListS.Count() > 0)
			//{
			//    _DocumentCategory.RemoveRange(_DocumentCategory.Where(p => p.DocumentId == model.Id));

			//    var DocumentCategory = new DocumentCategory();
			//    foreach (var item in model.CategoryIdListS)
			//    {
			//        _DocumentCategory.Add(new DocumentCategory { DocumentId = model.Id, CategoryId = item, AddDate = DateTime.Now });

			//    }
			//}
			//_uow.SaveChanges();
			return new ResultAction()
			{
				Success = true,
				Id = model.Id.ToString(),
				MessageList = $"مشخصات سند {model.Id} با موفقیت ویرایش گردید",
			};
		}
		catch (Exception e)
		{
			return new ResultAction()
			{
				Success = false,
				MessageList = $"در ویرایش مشخصات مشخصات سند {model.Id} خطایی رخ داده است. " + HelperCommon.ReturnMessageException(e)
			};
		}
	}

	public string? DeleteDocument(int documentId)
	{
		var item = document.SingleOrDefault(x => x.Id == documentId);
		if (item == null)
			return "فایل مورد نظر جهت حذف یافت نشد.";
		document.Remove(item);
		return null;
	}
	public ResultAction DeleteDocument(int documentId, string userName)
	{
		var item = DetailsDocument(documentId);
		try
		{
			document.Remove(item);
			iLogService.AddLog(new LogObject()
			{
				NextValue = null,
				PerValue = HelperCommon.ShallowCopyEntityToString<Document>(item),
				ObjectTypeId = "Document",
				ObjectTypeName = "Document",
				Title = "Delete Document",
				DateCreate = DateTime.Now,
				UserName = userName
			});
			uow.SaveChanges();
			return new ResultAction()
			{
				Success = true,
				TitleResult = "موفقیت آمیز",
				MessageList = $"سند انتخاب شده با موفقیت حذف گردید",
			};
		}
		catch (Exception e)
		{
			return new ResultAction()
			{
				Success = false,
				TitleResult = "خطا",
				MessageList = $"در حذف سند خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
			};
		}
	}


	public bool ActiveDeactiveDocument(int documentId, bool check)
	{
		try
		{
			var item = DetailsDocument(documentId);
			// item.IsActive = check;
			uow.SaveChanges();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public bool ActiveDeactiveSelectedDocument(int documentId, bool check)
	{
		try
		{
			var item = DetailsDocument(documentId);
			// item.Selected = check;
			uow.SaveChanges();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public List<SelectListItem> DocumentTypeList(string preName, string language)
	{
		List<SelectListItem> documentTypes = new List<SelectListItem>
				{
						new SelectListItem() { Text = preName, Value = "" }
				};
		//DocumentTypes.AddRange(_DocumentGroup.Where(x => x.Documents.Any(c => c.Language.Abbreviation == Language)).Select(u => new SelectListItem
		//{
		//    Text = u.Name,
		//    Value = u.Id.ToString()
		//}).ToList());
		return documentTypes;
	}

	public Document? DocumentDetails(int documentId)
	{
		return document.FirstOrDefault(x => x.Id == documentId);
	}
	public async Task<List<Document>> CurrentNews(int takeCount, string language, CancellationToken cancellationToken)
	{
		return await document.Where(x => /*x.IsActive &&*/ (string.IsNullOrEmpty(language))).OrderByDescending(x => x.Id).Take(takeCount).ToListAsync(cancellationToken);
	}
	public async Task<List<Document>> SelectedDocuments(int takeCount, string language, CancellationToken cancellationToken)
	{
		return await document.Where(x =>/* x.IsActive && x.Selected &&*/ (string.IsNullOrEmpty(language))).OrderByDescending(x => x.Id).Take(takeCount).ToListAsync(cancellationToken);
	}
	public List<Document> DocumentListOfGroup(int documentGroupId, int categoryId, string language)
	{
		var list = document.Where(x => /*x.IsActive && x.DocumentGroupId == DocumentGroupId && x.Selected &&*/ (string.IsNullOrEmpty(language)));
		if (categoryId > 0)
		{
			//var categoryIdList = _DocumentCategory.Where(p => p.CategoryId == CategoryId).Select(p => p.DocumentId).ToList();
			//list = list.Where(p => categoryIdList.Contains(p.Id));
		}
		return list.OrderByDescending(x => x.Id).ToList();
	}




}
