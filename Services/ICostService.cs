public interface ICostService
{
    Task<PagedCostResult> GetCostsAsync(PaginationDto pagination, CostFilterDto filter);
}