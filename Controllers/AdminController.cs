using System.ComponentModel.Design;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IExcelImportService _importService;

    public AdminController(IExcelImportService importService)
    {
        _importService = importService;
    }
    public async Task<IActionResult> Index(IndexViewModel viewmodel)
{
    var filter = new FilterDto(viewmodel.NationalCode, viewmodel.MinAmount, viewmodel.MaxAmount);
    var pagination = new PaginationDto(viewmodel.Page, PageSize: 10);
    var result = await _costService.GetCostsAsync(pagination, filter);
    return View(result);
}
    [HttpPost]
    
    public IActionResult Upload() => View();

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _importService.ImportCostsAsync(file);
        ViewBag.Result = result;
        return View();
    }
}