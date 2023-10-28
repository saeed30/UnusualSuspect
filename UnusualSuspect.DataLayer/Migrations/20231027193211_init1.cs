using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMAreaName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMAreaName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseEnumEntity",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEnumEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySoftwareRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySoftwareRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterCard",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPlayers = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ViewOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectType",
                columns: table => new
                {
                    ObjectKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectType", x => x.ObjectKey);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BussinessTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmallTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactUsPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactUsMobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMSNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactUsEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentContactUsPage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DateTimeAddedToQueue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SmsSendingStatusId = table.Column<short>(type: "smallint", nullable: true),
                    StatusMessage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Identifier = table.Column<long>(type: "bigint", nullable: true),
                    SendingError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSend = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsReadForSending = table.Column<bool>(type: "bit", nullable: false),
                    SendAttemptCount = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsLog_BaseEnumEntity_SmsSendingStatusId",
                        column: x => x.SmsSendingStatusId,
                        principalTable: "BaseEnumEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    MobileToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireToken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lastlogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FireBaseToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatchImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberValidationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeForResetPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendCodeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GameTypeId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_GameType_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreGameGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadyToGameTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalculatedJoinedUsers = table.Column<short>(type: "smallint", nullable: false),
                    GameTypeId = table.Column<short>(type: "smallint", nullable: false),
                    PreGameGroupStatusId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreGameGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreGameGroup_BaseEnumEntity_PreGameGroupStatusId",
                        column: x => x.PreGameGroupStatusId,
                        principalTable: "BaseEnumEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreGameGroup_GameType_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AMController",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMAreaNameId = table.Column<int>(type: "int", nullable: false),
                    SoftSectionId = table.Column<int>(type: "int", nullable: true),
                    FarsiName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMController", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AMController_AMAreaName_AMAreaNameId",
                        column: x => x.AMAreaNameId,
                        principalTable: "AMAreaName",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AMController_SoftSection_SoftSectionId",
                        column: x => x.SoftSectionId,
                        principalTable: "SoftSection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoftSectionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_SoftSection_SoftSectionId",
                        column: x => x.SoftSectionId,
                        principalTable: "SoftSection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdminPanleUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPanleUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminPanleUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogObject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectTypeId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogObject_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogObject_ObjectType_ObjectTypeId",
                        column: x => x.ObjectTypeId,
                        principalTable: "ObjectType",
                        principalColumn: "ObjectKey");
                });

            migrationBuilder.CreateTable(
                name: "CharacterCardGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMurderer = table.Column<bool>(type: "bit", nullable: false),
                    RemovedTurn = table.Column<short>(type: "smallint", nullable: true),
                    CharacterCardId = table.Column<short>(type: "smallint", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCardGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterCardGame_CharacterCard_CharacterCardId",
                        column: x => x.CharacterCardId,
                        principalTable: "CharacterCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharacterCardGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderOfParticipation = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    RoleCardId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participate_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participate_BaseEnumEntity_RoleCardId",
                        column: x => x.RoleCardId,
                        principalTable: "BaseEnumEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participate_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<short>(type: "smallint", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Turn = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionGame_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JoinedPreGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOwnerOfPreGroup = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PreGameGroupId = table.Column<int>(type: "int", nullable: false),
                    ReadyToGameStatusId = table.Column<short>(type: "smallint", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinedPreGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_BaseEnumEntity_ReadyToGameStatusId",
                        column: x => x.ReadyToGameStatusId,
                        principalTable: "BaseEnumEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_PreGameGroup_PreGameGroupId",
                        column: x => x.PreGameGroupId,
                        principalTable: "PreGameGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AmAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMControllerId = table.Column<int>(type: "int", nullable: false),
                    SoftSectionId = table.Column<int>(type: "int", nullable: true),
                    FarsiName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmAction_AMController_AMControllerId",
                        column: x => x.AMControllerId,
                        principalTable: "AMController",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AmAction_SoftSection_SoftSectionId",
                        column: x => x.SoftSectionId,
                        principalTable: "SoftSection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwarerRoleForUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareRoleId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwarerRoleForUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftwarerRoleForUser_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SoftwarerRoleForUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionForRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmActionId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionForRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionForRole_AmAction_AmActionId",
                        column: x => x.AmActionId,
                        principalTable: "AmAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionForRole_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionForSoftwareRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmActionId = table.Column<int>(type: "int", nullable: false),
                    SoftwareRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionForSoftwareRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionForSoftwareRole_AmAction_AmActionId",
                        column: x => x.AmActionId,
                        principalTable: "AmAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionForSoftwareRole_AspNetRoles_SoftwareRoleId",
                        column: x => x.SoftwareRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionForUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmActionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionForUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionForUser_AmAction_AmActionId",
                        column: x => x.AmActionId,
                        principalTable: "AmAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionForUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftSectionId = table.Column<int>(type: "int", nullable: false),
                    AMActionId = table.Column<int>(type: "int", nullable: true),
                    AMNotifActionId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    PatchIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FontAweSomeIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    NotifName = table.Column<int>(type: "int", nullable: true),
                    ActionAndControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomMenu_AmAction_AMActionId",
                        column: x => x.AMActionId,
                        principalTable: "AmAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomMenu_AmAction_AMNotifActionId",
                        column: x => x.AMNotifActionId,
                        principalTable: "AmAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomMenu_CustomMenu_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CustomMenu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomMenu_SoftSection_SoftSectionId",
                        column: x => x.SoftSectionId,
                        principalTable: "SoftSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BaseEnumEntity",
                columns: new[] { "Id", "Discriminator", "Name", "Title" },
                values: new object[,]
                {
                    { (short)1, "PreGameGroupStatus", "NotReady", "قبل از آمادگی جهت بازی" },
                    { (short)2, "PreGameGroupStatus", "Ready", "آماده جهت بازی" },
                    { (short)3, "PreGameGroupStatus", "InGame", "در حال بازی" }
                });

            migrationBuilder.InsertData(
                table: "SoftSection",
                columns: new[] { "Id", "AreaName", "TypeName" },
                values: new object[] { 1, "AdminPanel", "پنل مدیریت" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionForRole_AmActionId",
                table: "ActionForRole",
                column: "AmActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionForRole_RoleId",
                table: "ActionForRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionForSoftwareRole_AmActionId",
                table: "ActionForSoftwareRole",
                column: "AmActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionForSoftwareRole_SoftwareRoleId",
                table: "ActionForSoftwareRole",
                column: "SoftwareRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionForUser_AmActionId",
                table: "ActionForUser",
                column: "AmActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionForUser_UserId",
                table: "ActionForUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminPanleUser_UserId",
                table: "AdminPanleUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AmAction_AMControllerId",
                table: "AmAction",
                column: "AMControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_AmAction_SoftSectionId",
                table: "AmAction",
                column: "SoftSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AMController_AMAreaNameId",
                table: "AMController",
                column: "AMAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AMController_SoftSectionId",
                table: "AMController",
                column: "SoftSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_SoftSectionId",
                table: "AspNetRoles",
                column: "SoftSectionId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_ApplicationUserId",
                table: "AspNetUserClaims",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_ApplicationUserId",
                table: "AspNetUserLogins",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DocumentId",
                table: "AspNetUsers",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCardGame_CharacterCardId",
                table: "CharacterCardGame",
                column: "CharacterCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCardGame_GameId",
                table: "CharacterCardGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomMenu_AMActionId",
                table: "CustomMenu",
                column: "AMActionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomMenu_AMNotifActionId",
                table: "CustomMenu",
                column: "AMNotifActionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomMenu_ParentId",
                table: "CustomMenu",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomMenu_SoftSectionId",
                table: "CustomMenu",
                column: "SoftSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_GameTypeId",
                table: "Game",
                column: "GameTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_GameId",
                table: "JoinedPreGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_PreGameGroupId",
                table: "JoinedPreGame",
                column: "PreGameGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_ReadyToGameStatusId",
                table: "JoinedPreGame",
                column: "ReadyToGameStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_UserId",
                table: "JoinedPreGame",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LogObject_ObjectTypeId",
                table: "LogObject",
                column: "ObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LogObject_UserId",
                table: "LogObject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participate_GameId",
                table: "Participate",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Participate_RoleCardId",
                table: "Participate",
                column: "RoleCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Participate_UserId",
                table: "Participate",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PreGameGroup_GameTypeId",
                table: "PreGameGroup",
                column: "GameTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreGameGroup_PreGameGroupStatusId",
                table: "PreGameGroup",
                column: "PreGameGroupStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGame_GameId",
                table: "QuestionGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGame_QuestionId",
                table: "QuestionGame",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsLog_SmsSendingStatusId",
                table: "SmsLog",
                column: "SmsSendingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwarerRoleForUser_RoleId",
                table: "SoftwarerRoleForUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwarerRoleForUser_UserId",
                table: "SoftwarerRoleForUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionForRole");

            migrationBuilder.DropTable(
                name: "ActionForSoftwareRole");

            migrationBuilder.DropTable(
                name: "ActionForUser");

            migrationBuilder.DropTable(
                name: "AdminNotification");

            migrationBuilder.DropTable(
                name: "AdminPanleUser");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CategorySoftwareRole");

            migrationBuilder.DropTable(
                name: "CharacterCardGame");

            migrationBuilder.DropTable(
                name: "CustomMenu");

            migrationBuilder.DropTable(
                name: "HomeMenu");

            migrationBuilder.DropTable(
                name: "JoinedPreGame");

            migrationBuilder.DropTable(
                name: "LogObject");

            migrationBuilder.DropTable(
                name: "Participate");

            migrationBuilder.DropTable(
                name: "QuestionGame");

            migrationBuilder.DropTable(
                name: "SmsLog");

            migrationBuilder.DropTable(
                name: "SoftSetting");

            migrationBuilder.DropTable(
                name: "SoftwarerRoleForUser");

            migrationBuilder.DropTable(
                name: "CharacterCard");

            migrationBuilder.DropTable(
                name: "AmAction");

            migrationBuilder.DropTable(
                name: "PreGameGroup");

            migrationBuilder.DropTable(
                name: "ObjectType");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AMController");

            migrationBuilder.DropTable(
                name: "BaseEnumEntity");

            migrationBuilder.DropTable(
                name: "GameType");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "AMAreaName");

            migrationBuilder.DropTable(
                name: "SoftSection");
        }
    }
}
