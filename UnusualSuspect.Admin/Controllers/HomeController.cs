using UnusualSuspect.Admin.Infrastructure;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Identity;
using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnusualSuspect.Admin.Controllers;

public class HomeController : BaseController<HomeController>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IApplicationRoleService _applicationRoleManager;
    private readonly Services.IServices.IDocumentService _iDocumentService;


    public HomeController(ILogger<HomeController> logger, IApplicationRoleService applicationRoleManager,
         Services.IServices.IDocumentService iDocumentService, IWebHostEnvironment webHostEnvironment) : base(logger)
    {
        _applicationRoleManager = applicationRoleManager ?? throw new ArgumentNullException(nameof(applicationRoleManager));
        _iDocumentService = iDocumentService;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult ImageFetch(string s, string i)
    {
        Stream stream;
        string size = s;
        string webRootPath = _webHostEnvironment.WebRootPath;
        string path = "";
        path = Path.Combine(webRootPath, @"Images\Noimage.png");
        string defaultImageUrl = path;
        string imageId = i;
        var doc = _iDocumentService.GetDocument(imageId.ToInt());
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
                var rolelist = _applicationRoleManager.GetRoles().ToList();
                RetList = rolelist.Select(x => new Select2DTO(x.Id, x.Title)).ToList();
                break;
       }
        return Json(RetList);
    }


}