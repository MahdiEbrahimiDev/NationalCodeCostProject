public interface ICostService
{
   Task<CostReportViewModel> GetReportAsync();
    Task<PagedCostResult> GetCostsAsync(PaginationDto pagination, CostFilterDto filter);
}