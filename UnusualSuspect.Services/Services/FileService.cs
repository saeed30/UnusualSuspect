using UnusualSuspect.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using UnusualSuspect.Common.Utilities;
using Aspose.Cells;
using UnusualSuspect.Common.Attribute;

namespace UnusualSuspect.Services.Services;

public class FileService : IFileService
{
    //private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILogger<FileService> _logger;
    public FileService(/*IWebHostEnvironment hostingEnvironment,*/
                      ILogger<FileService> logger)
    {
        //_hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(_hostingEnvironment));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
    }
    public static void ActivateAspose()
    {
        //DownloadDevTools_aspose_cells.ActivateDateTimeNowSimulation();
        string licFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Aspose.Total.lic";
        if (System.IO.File.Exists(licFile))
            new Aspose.Cells.License().SetLicense(licFile);
    }

    public void DeleteFile(string filePath)
    {
      filePath = AppDomain.CurrentDomain.BaseDirectory + filePath;
      if (Path.Exists(filePath))
        File.Delete(filePath);
    }

    public string UploadFile(IFormFile file, string path)
    {
      string fileName = RandomHelper.GetUniqueFileName(file.FileName);
      var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Replace("/","\\"), fileName);
      file.CopyTo(new FileStream(filePath, FileMode.Create));
      return path + fileName;
    }
  public DataSet ImportExcel(Stream stream, int? maxRowCount = null, int? maxColCount = null)
    {
        Workbook workbook = new Workbook(stream);
        DataSet dataSet = new DataSet();
        for (int i = 0; i < workbook.Worksheets.Count; i++)
        {
            int maxDataRow = workbook.Worksheets[i].Cells.MaxDataRow + 1;
            int maxDataColumn = workbook.Worksheets[i].Cells.MaxDataColumn + 1;
            if (maxRowCount.HasValue && maxRowCount.Value < maxDataRow)
                maxDataRow = maxRowCount.Value;
            if (maxColCount.HasValue && maxColCount.Value < maxDataColumn)
                maxDataColumn = maxColCount.Value;
            DataTable dataTable = workbook.Worksheets[i].Cells.ExportDataTable(0, 0,
                maxDataRow, maxDataColumn, false);
            dataSet.Tables.Add(dataTable);
        }
        return dataSet;
    }

    public MemoryStream DatatableToExcel(DataTable dataTable, bool removeDangerousText = true)
    {
        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();
        Aspose.Cells.Worksheet sheet = book.Worksheets[0];
        Aspose.Cells.ImportTableOptions importOptions = new Aspose.Cells.ImportTableOptions
        {
            IsFieldNameShown = true
        };
        if (removeDangerousText)
            RemoveDangerTextFromDataTable(ref dataTable);
        sheet.Cells.ImportData(dataTable, 0, 0, importOptions);
        using (MemoryStream m = new MemoryStream())
        {
            book.Save(m, Aspose.Cells.SaveFormat.Xlsx);
            m.Position = 0;
            return m;
        }
    }
    public MemoryStream DatatableToExcel<T>(List<T> listOfItems)
    {
        //برای رفع مشکل عناوین فارسی ستون ها باید پراپرتی isPropertyNameShown  غیر فعال شده و یک سطر به صورت دستی اضافه شود.
        //https://stackoverflow.com/questions/30617649/assign-custom-property-name-in-aspose-importcustomobjects
        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook();
        Aspose.Cells.Worksheet sheet = book.Worksheets[0];
        //if (removeDangerousText)
        //    RemoveDangerTextFromDataTable(ref listOfItems);
        List<string> names = new List<string>();
        char c = 'A';
        foreach (var item in typeof(T).GetProperties())
        {
            names.Add(item.Name);
            string disName = item.GetDisplayName();
            if (disName.IsNull())
                disName = item.Name;
            sheet.Cells[c+"1"].PutValue(disName);
            c++;
        }
        sheet.Cells.ImportCustomObjects(listOfItems, names.ToArray(), false, 1, 0, listOfItems.Count, true, "yyyy/MM/dd", false);
        MemoryStream m = new MemoryStream();
        book.Save(m, Aspose.Cells.SaveFormat.Xlsx);
        book.Worksheets[0].AutoFitColumns();
        m.Position = 0;
        return m;
    }

    #region privateMethods
    private static void RemoveDangerTextFromDataTable(ref DataTable dataTable)
    {
        if (dataTable == null)
            return;
        for (int i = 0; i < dataTable.Rows.Count; i++)
            for (int j = 0; j < dataTable.Columns.Count; j++)
                if (dataTable.Rows[i][j] != null && dataTable.Rows[i][j] is string)
                    dataTable.Rows[i][j] = RemoveDangerText(dataTable.Rows[i][j].ToString());
    }
    private static string RemoveDangerText(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
            return text.Replace("@", "'@'").Replace("-", "'-'").Replace("+", "'+'").Replace("=", "'='");
        return text;
    }

    #endregion privateMethods

}
