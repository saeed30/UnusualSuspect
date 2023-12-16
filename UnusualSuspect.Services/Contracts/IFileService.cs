using Microsoft.AspNetCore.Http;
using System.Data;

namespace UnusualSuspect.Services.Contracts;

public interface IFileService
{
    void UploadFile(IFormFile file);
    DataSet ImportExcel(Stream stream, int? maxRowCount = null, int? maxColCount = null);
    MemoryStream DatatableToExcel<T>(List<T> listOfItems);
    MemoryStream DatatableToExcel(DataTable table, bool removeDangerousText = true);
}
