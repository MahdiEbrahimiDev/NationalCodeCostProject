using ClosedXML.Excel;
using NationalCodeCostProject.Data;
using NationalCodeCostProject.Models;

namespace NationalCodeCostProject.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly AppDbContext _context;
    private static readonly string[] AllowedExtensions = { ".xlsx", ".xls" };
    private static readonly string[] ExpectedHeaders = { "کد ملی", "مبلغ" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public ExcelImportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ImportResult> ImportCostsAsync(IFormFile file)
    {
        var result = new ImportResult();

        if (!IsFileValid(file, out var fileError))
        {
            result.Errors.Add(new ImportError { RowNumber = 0, Message = fileError });
            return result;
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        using var workbook = new XLWorkbook(stream);

        if (workbook.Worksheets.Count == 0)
        {
            result.Errors.Add(new ImportError { RowNumber = 0, Message = "فایل اکسل هیچ شیتی ندارد" });
            return result;
        }

        var sheet = workbook.Worksheet(1);

        if (!sheet.RowsUsed().Any())
        {
            result.Errors.Add(new ImportError { RowNumber = 0, Message = "شیت خالی است" });
            return result;
        }

        if (!AreHeadersValid(sheet, out var headerError))
        {
            result.Errors.Add(new ImportError { RowNumber = 1, Message = headerError });
            return result;
        }

       var rows = sheet.RowsUsed().Skip(1).ToList(); // ردیف ۱ هدره
    result.TotalRows = rows.Count;

    var newCosts = new List<Cost>();
    var seenInFile = new HashSet<string>(); // برای تشخیص تکراری داخل خودِ فایل

    foreach (var row in rows)
    {
        var rowNumber = row.RowNumber();
        var nationalCode = row.Cell(1).GetString().Trim();
        var amountText = row.Cell(2).GetString().Trim();

        // اعتبارسنجی کد ملی
        if (!System.Text.RegularExpressions.Regex.IsMatch(nationalCode, @"^\d{10}$"))
        {
            result.Errors.Add(new ImportError
            {
                RowNumber = rowNumber,
                Message = $"کد ملی نامعتبر: '{nationalCode}' (باید دقیقاً ۱۰ رقم باشد)"
            });
            continue;
        }

        // اعتبارسنجی مبلغ
        if (!decimal.TryParse(amountText, out var amount) || amount <= 0)
        {
            result.Errors.Add(new ImportError
            {
                RowNumber = rowNumber,
                Message = $"مبلغ نامعتبر: '{amountText}'"
            });
            continue;
        }

        // تکراری داخل همین فایل
        if (!seenInFile.Add(nationalCode))
        {
            result.Errors.Add(new ImportError
            {
                RowNumber = rowNumber,
                Message = $"کد ملی '{nationalCode}' در همین فایل تکراری است"
            });
            continue;
        }

        // تکراری تو دیتابیس
        if (await _context.Costs.AnyAsync(c => c.NationalCode == nationalCode))
        {
            result.Errors.Add(new ImportError
            {
                RowNumber = rowNumber,
                Message = $"کد ملی '{nationalCode}' از قبل در دیتابیس موجود است"
            });
            continue;
        }

        newCosts.Add(new Cost { NationalCode = nationalCode, Amount = amount });
    }

    if (newCosts.Count > 0)
    {
        _context.Costs.AddRange(newCosts);
        await _context.SaveChangesAsync();
    }

    result.SuccessCount = newCosts.Count;
    return result;

       
    }

    private bool IsFileValid(IFormFile file, out string error)
    {
        if (file == null || file.Length == 0)
        {
            error="فایلی انتخاب نشده";
            return false;
        }
         if (file.Length > MaxFileSize)
        {
            error = "حجم فایل نباید بیشتر از ۵ مگابایت باشد";
            return false;
        }
        var extension =Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            error="فایل باید  xls یا xlsx باشد ";
            return false;
        }
         error = null;
        return true;
    }

    private bool AreHeadersValid(IXLWorksheet sheet, out string error)
    {
        var headerRow = sheet.Row(1);
        var actualHeaders = new[]
        {
            headerRow.Cell(1).GetString().Trim(),
            headerRow.Cell(2).GetString().Trim()
        };

        if (!actualHeaders.SequenceEqual(ExpectedHeaders))
        {
            error = $"هدرهای فایل نامعتبر است. انتظار می‌رفت: {string.Join(", ", ExpectedHeaders)}";
            return false;
        }

        error = null;
        return true;
    }
  
}