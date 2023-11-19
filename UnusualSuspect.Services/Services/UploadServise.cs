using UnusualSuspect.Common.Enums;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;

namespace UnusualSuspect.Services.Services;

public interface IUploadServise
{
	Task<byte[]> GetFileDataAsync(IFormFile formFile);
	ResultAction IsUpload(IFormFile file, bool isRequired = true, ExtensionFileEnum extensionFile = ExtensionFileEnum.Image, int contentLength = 550000);
	Task<string> SaveFile(IFormFile file, string folder);
	Task<ResultAction> SaveFileAsync(IFormFile formFile, string filePath, bool isOverwrite = true);
	string SaveImageSharp(IFormFile file, string folder, int width, int height);
	string SaveImageSharpWithCrop(IFormFile file, string folder, int width, int height);
	string SaveImageWithRatio(IFormFile file, string folder, double ratio);
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
		return new ResultAction { Success = false };
	}

	public async Task<byte[]> GetFileDataAsync(IFormFile formFile)
  {
    using var memoryStream = new MemoryStream();
    await formFile.CopyToAsync(memoryStream);
    return memoryStream.ToArray();
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
	public string SaveImageSharp(IFormFile file, string folder, int width, int height)
	{
		if (file == null || file.Length == 0)
			return null;
		Random rand = new Random();
		string fileName = folder + rand.Next(10000000, 100000000).ToString() + file.FileName;
		string filePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + folder, fileName);
		var patch = "/Files/" + folder + "/" + fileName;
		ResizeOptions options = new ResizeOptions
		{
			Mode = ResizeMode.BoxPad,
			Position = AnchorPositionMode.Center,
			Size = new SixLabors.ImageSharp.Size(width, height)
		};
		using (Image image = Image.Load(file.OpenReadStream()))
		{
			image.Mutate(x => x
					 .Resize(options).BackgroundColor(Color.White));
			image.Save(filePatch);
		}
		return patch;
	}

	public async Task<string> SaveFile(IFormFile file, string folder)
	{
		if (file == null || file.Length == 0)
			return null;
		Random rand = new Random();
		string fileName = folder + rand.Next(10000000, 100000000).ToString() + file.FileName;
		string filePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + folder, fileName);
		var patch = "/Files/" + folder + "/" + fileName;
		using (var fileStream = new FileStream(filePatch, FileMode.Create))
		{
			await file.CopyToAsync(fileStream);
		}
		return patch;
	}

	public string SaveImageSharpWithCrop(IFormFile file, string folder, int width, int height)
	{
		if (file == null || file.Length == 0)
			return null;
		Random rand = new Random();
		string fileName = folder + rand.Next(10000000, 100000000).ToString() + file.FileName;
		string filePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + folder, fileName);
		var patch = "/Files/" + folder + "/" + fileName;
		using (Image image = Image.Load(file.OpenReadStream()))
		{
			ResizeOptions options = new ResizeOptions
			{
				Mode = ResizeMode.Crop,
				Position = AnchorPositionMode.Center,
				Size = new SixLabors.ImageSharp.Size(width, height)
			};
			image.Mutate(x => x.Resize(options).BackgroundColor(Color.White));
			image.Save(filePatch);
		}
		return patch;
	}

	public string SaveImageWithRatio(IFormFile file, string folder, double ratio)
	{
		if (file == null || file.Length == 0)
			return null;
		Random rand = new Random();
		string fileName = folder + rand.Next(10000000, 100000000).ToString() + file.FileName;
		string filePatch = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\" + folder, fileName);
		var patch = "/Files/" + folder + "/" + fileName;
		using (Image image = Image.Load(file.OpenReadStream()))
		{
			int width = 0; int height = 0;
			width = image.Width;
			height = (int)(width / ratio);
			ResizeOptions options = new ResizeOptions
			{
				Mode = ResizeMode.BoxPad,
				Position = AnchorPositionMode.Center,
				Size = new SixLabors.ImageSharp.Size(width, height)
			};
			image.Mutate(x => x.Resize(options).BackgroundColor(Color.White));
			image.Save(filePatch);
		}
		return patch;
	}



	// حذف تصویر کاربر به هنگام ویرایش
	public ResultAction DeletePictureUser(ApplicationUser model)
	{
		try
		{
			if (model.PatchImage != null && model.ImageFile != null)
			{
				if (model.PatchImage != "blank.png")
				{
					string imagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\media\\UserImage", model.PatchImage);
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
