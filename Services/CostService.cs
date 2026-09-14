using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using NationalCodeCostProject.Data;
using NationalCodeCostProject.DTOs;
using NationalCodeCostProject.Models;

namespace NationalCodeCostProject.Services;

public class CostService : ICostService
{
    private readonly AppDbContext _context;

    public CostService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Cost>> GetCostsAsync(PaginationDto pagination, CostFilterDto filter)
    {
        var query = _context.Costs.AsQueryable(); // یه کوئری برای فیلتر کردنه چون یه سری پارامتر اختیاری داریم بخوایم
                                                  //برای هرکدوم لینک بنویسیم خیلی طولانی میشه بنابرای ن میگیم
                                                  //یه کوئری بساز اگه فیلتر کدملی داشت فلان کار اگر فیلتر فلان داشت فلان کار
                                                  //اخرش هم باید حتما تولیست  بزنیم  در انتها اگرنه  نمیره به دیتابیس

        if (!string.IsNullOrWhiteSpace(filter.NationalCode))
            query = query.Where(c => c.NationalCode.Contains(filter.NationalCode));

        if (filter.MinAmount.HasValue)
            query = query.Where(c => c.Amount >= filter.MinAmount.Value);

        if (filter.MaxAmount.HasValue)
            query = query.Where(c => c.Amount <= filter.MaxAmount.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<Cost>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }
    
    
       public async Task<CostReportViewModel> GetReportAsync()
{
    var hasAnyRecord = await _context.Costs.AnyAsync();

    if (!hasAnyRecord)
    {
        return new CostReportViewModel();
    }

var result=_context.Cost.GroupBy(g=>1).Select(a=>new CostReportViewModel()
{
    TotalCount = g.Count(),
            TotalAmount = g.Sum(c => c.Amount),
            AverageAmount = g.Average(c => c.Amount),
            MaxAmount = g.Max(c => c.Amount),
            MinAmount = g.Min(c => c.Amount)
});
result.FirstAsync();
return result;
    
}
    
}