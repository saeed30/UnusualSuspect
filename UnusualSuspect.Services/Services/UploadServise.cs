using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace UnusualSuspect.Services.Services;

public interface IUploadServise
{
	Task<byte[]> GetFileDataAsync(IFormFile formFile);
	ResultAction IsUpload(IFormFile file, bool isRequired = true, ExtensionFileEnum extensionFile = ExtensionFileEnum.Image, int contentLength = 550000);
	Task<string> SaveFile(IFormFile file, string Folder);
	Task<ResultAction> SaveFileAsync(IFormFile formFile, string filePath, bool isOverwrite = true);
	string SaveImageSharp(IFormFile file, string Folder, int Width, int Height);
	string SaveImageSharpWithCrop(IFormFile file, string Folder, int Width, int Height);
	string SaveImageWithRatio(IFormFile file, string Folder, double Ratio);
	ResultAction DeletePictureUser(ApplicationUser model);
	string GetUniqueFileName(IFormFile formFile);
}

public class UploadServise : IUploadServise
{
	private const int MaxBufferSize = 0x10000;
	public async Task<ResultAction> SaveFileAsync(IFormFile formFile, string filePath, bool isOverwrite = true)
	{
		if (formFile != null)
		{
			string fileName = formFile.FileName;
			if (isOverwrite)
			{
				fileName = GetUniqueFileName(formFile);

			}
			string path = GetFilePath(fileName, filePath);
			using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write,
																	FileShare.None,
																	MaxBufferSize,
																	useAsync: true))
			{
				await formFile.CopyToAsync(fileStream);
			}
			return new ResultAction { Success = true, MessageList = fileName };
		}
		else
			return new ResultAction { Success = false };
	}

	public async Task<byte[]> GetFileDataAsync(IFormFile formFile)
	{
		using (var memoryStream = new MemoryStream())
		{
			await formFile.CopyToAsync(memoryStream);
			return memoryStream.ToArray();
		}
	}

	public ResultAction IsUpload(IFormFile file, bool isRequired = true, ExtensionFileEnum extensionFile = ExtensionFileEnum.Image, int contentLength = 950000)
	{
		if (isRequired)
		{
			if (file == null || file.Length == 0)
			{
				return new ResultAction { Success = false, MessageList = "Please enter the file" };
			}
		}
		else if (file != null)
		{

			var extension = Path.GetExtension(file.FileName);
			if (extension != null)
			{
				var ext = extension.ToLower();
				string[] arrext;
				if (extensionFile == ExtensionFileEnum.Image)
					arrext = new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" };
				else if (extensionFile == ExtensionFileEnum.Audio)
					arrext = new string[] { ".ogg", ".mp3" };
				else if (extensionFile == ExtensionFileEnum.Video)
					arrext = new string[] { ".ogg", ".mp4" };
				else if (extensionFile == ExtensionFileEnum.File)
					arrext = new string[] { ".zip", ".rar", ".pdf", ".txt", ".docx", ".doc" };
				else if (extensionFile == ExtensionFileEnum.Excel)
					arrext = new string[] { ".xls", ".xlsx" };
				else if (extensionFile == ExtensionFileEnum.Apk)
					arrext = new string[] { ".apk" };
				else
					arrext = new string[] { ".zip", ".rar", ".pdf", ".xls", ".xlsx", ".txt", ".gif", ".jpg", ".png", ".jpeg", ".svg", ".ogg", ".mp3", ".mp4", ".docx", ".doc" };

				var pos = Array.IndexOf(arrext, ext);
				if (pos <= -1)
				{
					return new ResultAction { Success = false, MessageList = "The file type is incorrect" };
				}
			}

			if (!(file.Length <= contentLength))
			{
				return new ResultAction { Success = false, MessageList = "The size of the submitted file is more than the allowed value" };
			}
		}


		return new ResultAction { Success = true };
	}

	public string GetUniqueFileName(IFormFile formFile)
	{
		var extension = Path.GetExtension(formFile.FileName);
		return $"{Guid.NewGuid().ToString().Replace('-', '_')}{extension}";
	}

	private string GetFilePath(string fileName, string uploadsRootFolder)
	{
		return Path.Combine(uploadsRootFolder, fileName);
	}
	public string SaveImageSharp(IFormFile file, string Folder, int Width, int Height)
	{
		if (file == null || file.Length == 0)
			return null;
		Random Rand = new Random();
		string FileName = Folder + Rand.Next(10000000, 100000000).ToString() + file.FileName;
		string FilePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + Folder, FileName);
		var Patch = "/Files/" + Folder + "/" + FileName;
		ResizeOptions options = new ResizeOptions
		{
			Mode = ResizeMode.BoxPad,
			Position = AnchorPositionMode.Center,
			Size = new SixLabors.ImageSharp.Size(Width, Height)
		};
		using (Image image = Image.Load(file.OpenReadStream()))
		{
			image.Mutate(x => x
					 .Resize(options).BackgroundColor(Color.White));
			image.Save(FilePatch);
		}
		return Patch;
	}

	public async Task<string> SaveFile(IFormFile file, string Folder)
	{
		if (file == null || file.Length == 0)
			return null;
		Random Rand = new Random();
		string FileName = Folder + Rand.Next(10000000, 100000000).ToString() + file.FileName;
		string FilePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + Folder, FileName);
		var Patch = "/Files/" + Folder + "/" + FileName;
		using (var fileStream = new FileStream(FilePatch, FileMode.Create))
		{
			await file.CopyToAsync(fileStream);
		}
		return Patch;
	}

	public string SaveImageSharpWithCrop(IFormFile file, string Folder, int Width, int Height)
	{
		if (file == null || file.Length == 0)
			return null;
		Random Rand = new Random();
		string FileName = Folder + Rand.Next(10000000, 100000000).ToString() + file.FileName;
		string FilePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + Folder, FileName);
		var Patch = "/Files/" + Folder + "/" + FileName;
		using (Image image = Image.Load(file.OpenReadStream()))
		{
			ResizeOptions options = new ResizeOptions
			{
				Mode = ResizeMode.Crop,
				Position = AnchorPositionMode.Center,
				Size = new SixLabors.ImageSharp.Size(Width, Height)
			};
			image.Mutate(x => x.Resize(options).BackgroundColor(Color.White));
			image.Save(FilePatch);
		}
		return Patch;
	}

	public string SaveImageWithRatio(IFormFile file, string Folder, double Ratio)
	{
		if (file == null || file.Length == 0)
			return null;
		Random Rand = new Random();
		string FileName = Folder + Rand.Next(10000000, 100000000).ToString() + file.FileName;
		string FilePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + Folder, FileName);
		var Patch = "/Files/" + Folder + "/" + FileName;
		using (Image image = Image.Load(file.OpenReadStream()))
		{
			int Width = 0; int Height = 0;
			Width = image.Width;
			Height = (int)(Width / Ratio);
			ResizeOptions options = new ResizeOptions
			{
				Mode = ResizeMode.BoxPad,
				Position = AnchorPositionMode.Center,
				Size = new SixLabors.ImageSharp.Size(Width, Height)
			};
			image.Mutate(x => x.Resize(options).BackgroundColor(Color.White));
			image.Save(FilePatch);
		}
		return Patch;
	}



	// حذف تصویر کاربر به هنگام ویرایش
	public ResultAction DeletePictureUser(ApplicationUser model)
	{
		try
		{
			if (model.PatchImage != null && model.ImageFile != null)
			{
				string imagePath = "";
				if (model.PatchImage != "blank.png")
				{
					imagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\media\\UserImage", model.PatchImage);
					if (File.Exists(imagePath))
					{
						File.Delete(imagePath);
					}
				}
			}
		}
		catch (Exception)
		{
			return new ResultAction() { Success = false };
		}
		return new ResultAction() { Success = true };
	}



}
