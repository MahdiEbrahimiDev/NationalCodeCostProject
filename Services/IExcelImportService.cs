using NationalCodeCostProject.Models;

namespace NationalCodeCostProject.Services;

public interface IExcelImportService
{
    Task<ImportResult> ImportCostsAsync(IFormFile file);
        

}