using UnusualSuspect.Common.Enums;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnusualSuspect.ApiViewModels.Endpoints.Document;

namespace UnusualSuspect.Services.IServices;

public interface IDocumentService
{
	Task<Document?> SaveFormFile(IFormFile file, string tableName, string keyName, int? documentId = null);
	ResultAction CreateDocumentFile(IFormFile file, string tableName, ExtensionFileEnum extensionFile = ExtensionFileEnum.Image);
	bool ActiveDeactiveDocument(int documentId, bool check);
	bool ActiveDeactiveSelectedDocument(int documentId, bool check);
	//ResultAction AddDocumentComment(DocumentComment model);
	void AddViewCount(Document item);
	//List<DocumentComment> CombinedAllDocumentCommentsearch(DocumentCommentSearchViewModel model);
	IQueryable<Document> CombinedAllDocumentSearch(DocumentSearchViewModel model);
	bool ConfirmDocumentComment(int documentCommentId, bool check);
	ResultAction CreateDocument(Document model);
	//ResultAction CreateDocumentComment(DocumentComment model);
	Task<List<Document>> CurrentNews(int takeCount, string language, CancellationToken cancellationToken);
	string? DeleteDocument(int documentId);
	ResultAction DeleteDocument(int documentId, string userName);
	ResultAction DeleteDocumentComment(int documentCommentId, string userName);
	Document DetailsDocument(long documentId);
	//DocumentComment DetailsDocumentComment(int Id);
	//DocumentComment DetailsDocumentComment(int DocumentId, long DocumentCommentId);
	//List<DocumentComment> DocumentCommentList(int DocumentId, int Step, ApplicationDbContext Db);
	Document? DocumentDetails(int documentId);
	List<Document> DocumentSearch(DocumentSearchViewModel model, bool? isActive = null);
	List<SelectListItem> DocumentTypeList(string preName, string language);
	ResultAction EditDocument(Document model);
	//ResultAction EditDocumentComment(DocumentComment model);
	int LikeDisLikeComment(int commentId, bool like);
	int LikeDisLikeDocumentComment(IHttpContextAccessor httpContextAccessor, int documentId, int commentId, bool like);
	int NotConfirmComments();
	Task<List<Document>> SelectedDocuments(int takeCount, string language, CancellationToken cancellationToken);
	List<Document> DocumentListOfGroup(int documentGroupId, int categoryId, string language);
	List<int> DocumentCategoryList(int documentId);
	Task<Document?> GetDocumentAsync(int id);
	Task<UnusualSuspectServiceResult<DocumentGetResponse>> GetDocumentByGuidKeyAsync(Guid guidKey);
	Document? GetDocument(int id);
}