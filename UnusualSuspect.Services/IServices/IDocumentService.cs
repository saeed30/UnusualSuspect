using UnusualSuspect.Common.Enums;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.IServices;

public interface IDocumentService
{
    Document SaveFormFile(IFormFile file, string tableName,string keyName);
    ResultAction  CreateDocumentFile(IFormFile file, string TableName, ExtensionFileEnum extensionFile = ExtensionFileEnum.Image);
    bool ActiveDeactiveDocument(int DocumentId, bool check);
    bool ActiveDeactiveSelectedDocument(int DocumentId, bool check);
    //ResultAction AddDocumentComment(DocumentComment model);
    void AddViewCount(Document Item);
    //List<DocumentComment> CombinedAllDocumentCommentsearch(DocumentCommentSearchViewModel model);
    IQueryable<Document> CombinedAllDocumentSearch(DocumentSearchViewModel model);
    bool ConfirmDocumentComment(int DocumentCommentId, bool check);
    ResultAction CreateDocument(Document model);
    //ResultAction CreateDocumentComment(DocumentComment model);
    Task<List<Document>> CurrentNews(ApplicationDbContext Db, int takeCount, string Language, CancellationToken CancellationToken);
    string DeleteDocument(int DocumentId);
    ResultAction DeleteDocument(int DocumentId, string UserName);
    ResultAction DeleteDocumentComment(int DocumentCommentId, string UserName);
    Document DetailsDocument(long DocumentId);
    //DocumentComment DetailsDocumentComment(int Id);
    //DocumentComment DetailsDocumentComment(int DocumentId, long DocumentCommentId);
    //List<DocumentComment> DocumentCommentList(int DocumentId, int Step, ApplicationDbContext Db);
    Document DocumentDetails(int DocumentId, ApplicationDbContext Db);
    List<Document> DocumentSearch(DocumentSearchViewModel model, bool? IsActive = null);
    List<SelectListItem> DocumentTypeList(string PreName, string Language);
    ResultAction EditDocument(Document model);
    //ResultAction EditDocumentComment(DocumentComment model);
    int LikeDisLikeComment(int CommentId, bool Like);
    int LikeDisLikeDocumentComment(IHttpContextAccessor httpContextAccessor, int DocumentId, int CommentId, bool Like);
    int NotConfirmComments();
    Task<List<Document>> SelectedDocuments(ApplicationDbContext Db, int takeCount, string Language, CancellationToken CancellationToken);
    List<Document> DocumentListOfGroup(int DocumentGroupId, int CategoryId, string Language);
    List<int> DocumentCategoryList(int DocumentId);
    Document GetDocument(int Id);
}