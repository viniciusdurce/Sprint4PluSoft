using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.Models;

namespace Sprint4PlusSoft.Services;

public interface ICompanyReportService
{
    Task<CompanyReport> CreateAsync(CompanyReportDTO campaignReportDto);
    Task<CompanyReport> GetByIdAsync(string id);
    Task<IEnumerable<CompanyReport>> GetAllAsync();
    Task<bool> UpdateAsync(string id, CompanyReportDTO campaignReportDto);
    Task<bool> DeleteAsync(string id);
}