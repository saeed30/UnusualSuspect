using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Services;

public class BaseInfoService : IBaseInfoService
{

    private readonly ILogger<BaseInfoService> _logger;
    private readonly IUnitOfWork _uow;


    private readonly IDocumentService _IDocumentService;
    private readonly IApplicationUserManager _IApplicationUserManager;
    private readonly ILogService _ILogService;
    protected readonly IUploadServise _uploadServise;
    protected readonly IApplicationRoleService _roleManager;

    public BaseInfoService(ILogger<BaseInfoService> logger, IUnitOfWork uow, ILogService iLogService, IUploadServise uploadServise,
        IDocumentService documentService, IApplicationUserManager IApplicationUserManager, IApplicationRoleService roleManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _ILogService = iLogService;
        _IDocumentService = documentService;
        _uploadServise = uploadServise;
        _IApplicationUserManager = IApplicationUserManager;
        _roleManager = roleManager;
    }
}