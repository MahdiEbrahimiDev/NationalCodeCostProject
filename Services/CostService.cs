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
        var query = _context.Costs.AsQueryable();

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
}