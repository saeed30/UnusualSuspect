using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class noIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CharacterCard
	(
	Id smallint NOT NULL,
	Title nvarchar(MAX) NOT NULL,
	IsActive bit NOT NULL,
	ImageUrl nvarchar(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CharacterCard SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.CharacterCard)
	 EXEC('INSERT INTO dbo.Tmp_CharacterCard (Id, Title, IsActive, ImageUrl)
		SELECT Id, Title, IsActive, ImageUrl FROM dbo.CharacterCard WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.CharacterCardGame
	DROP CONSTRAINT FK_CharacterCardGame_CharacterCard_CharacterCardId
GO
ALTER TABLE dbo.QuestionCharacterCardDefaultAnswer
	DROP CONSTRAINT FK_QuestionCharacterCardDefaultAnswer_CharacterCard_CharacterCardId
GO
ALTER TABLE dbo.GameCandidate
	DROP CONSTRAINT FK_GameCandidate_CharacterCard_CharacterCardId
GO
DROP TABLE dbo.CharacterCard
GO
EXECUTE sp_rename N'dbo.Tmp_CharacterCard', N'CharacterCard', 'OBJECT' 
GO
ALTER TABLE dbo.CharacterCard ADD CONSTRAINT
	PK_CharacterCard PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GameCandidate ADD CONSTRAINT
	FK_GameCandidate_CharacterCard_CharacterCardId FOREIGN KEY
	(
	CharacterCardId
	) REFERENCES dbo.CharacterCard
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.GameCandidate SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.QuestionCharacterCardDefaultAnswer ADD CONSTRAINT
	FK_QuestionCharacterCardDefaultAnswer_CharacterCard_CharacterCardId FOREIGN KEY
	(
	CharacterCardId
	) REFERENCES dbo.CharacterCard
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.QuestionCharacterCardDefaultAnswer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CharacterCardGame ADD CONSTRAINT
	FK_CharacterCardGame_CharacterCard_CharacterCardId FOREIGN KEY
	(
	CharacterCardId
	) REFERENCES dbo.CharacterCard
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CharacterCardGame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Id",
                table: "CharacterCard",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
