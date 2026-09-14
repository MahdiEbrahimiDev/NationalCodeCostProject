namespace NationalCodeCostProject.Models;

public class ImportResult
{
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public List<ImportError> Errors { get; set; } = new();

    public bool HasErrors => Errors.Count > 0;
}

public class ImportError
{
    public int RowNumber { get; set; }
    public string Message { get; set; }
}