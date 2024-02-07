using UnusualSuspect.Admin.Infrastructure;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UnusualSuspect.Admin.Controllers;

public class HomeController(ILogger<HomeController> logger, IApplicationRoleService applicationRoleManager,
    Services.IServices.IDocumentService iDocumentService, IWebHostEnvironment webHostEnvironment)
  : BaseController<HomeController>(logger)
{
  public IActionResult ImageFetch(string s, string i)
    {
        Stream stream;
        string size = s;
        string webRootPath = webHostEnvironment.WebRootPath;
        string path = Path.Combine(webRootPath, @"Images\Noimage.png");
        string defaultImageUrl = path;
        string imageId = i;
        var doc = iDocumentService.GetDocument(imageId.ToInt());
        if (doc == null)
            stream = ImageHelper.WriteThumbnailImage(null, defaultImageUrl, size);
        else
            stream = ImageHelper.WriteThumbnailImage(doc.File, defaultImageUrl, size);
        StreamReader sr = new StreamReader(stream);
        // later... after we read stuff
        stream.Position = 0;
        sr.DiscardBufferedData();
        if (doc == null)
            return File(stream, ImageHelper.GetContentType("png"));
        else
            return File(stream, ImageHelper.GetContentType(doc.DocumentType));

    }
    public IActionResult Index()
    {
        return Redirect("/Dashboard");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public JsonResult GetKendoDropDownData(string tblName, string text = "", string SelectedId = "", string filter = "")
    {
        tblName = tblName.ToLower();
        var RetList = new List<Select2DTO>();
        switch (tblName)
        {
            case "roles":
                var rolelist = applicationRoleManager.GetRoles().ToList();
                RetList = rolelist.Select(x => new Select2DTO(x.Id, x.Title)).ToList();
                break;
       }
        return Json(RetList);
    }


}