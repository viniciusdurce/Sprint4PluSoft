using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprint4PlusSoft.Models;
using Sprint4PlusSoft.DTOs;

namespace Sprint4PlusSoft.Services
{
    public class CompanyReportService : ICompanyReportService
    {
        private readonly IMongoCollection<CompanyReport> _collection;

        public CompanyReportService(IMongoDatabase database)
        {
            _collection = database.GetCollection<CompanyReport>("companiesReports");
        }

        public async Task<CompanyReport> CreateAsync(CompanyReportDTO companyReportDto)
        {
            var companyReport = MapToEntity(companyReportDto);
            companyReport.Label = CalculateLabel(companyReport); // Calcula o Label
            await _collection.InsertOneAsync(companyReport);
            return companyReport;
        }

        public async Task<CompanyReport> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CompanyReport>> GetAllAsync()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<bool> UpdateAsync(string id, CompanyReportDTO companyReportDto)
        {
            var companyReport = MapToEntity(companyReportDto);
            companyReport.Id = id; // Define o ID para atualização
            companyReport.Label = CalculateLabel(companyReport); // Recalcula o Label ao atualizar

            var result = await _collection.ReplaceOneAsync(x => x.Id == id, companyReport);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        private CompanyReport MapToEntity(CompanyReportDTO dto)
        {
            return new CompanyReport
            {
                CompanyId = dto.CompanyId,
                ROI = dto.ROI,
                MonthlyRevenue = dto.MonthlyRevenue,
                ProfitMargin = dto.ProfitMargin,
                EmployeeCount = dto.EmployeeCount,
                CampaignConversionRate = dto.CampaignConversionRate
            };
        }

        // Método para calcular o Label com base nos critérios fornecidos
        private bool CalculateLabel(CompanyReport data)
        {
            bool isROIPositive = data.ROI > 100;
            bool isConversionRateGood = data.CampaignConversionRate > 6;
            bool isProfitMarginGood = data.ProfitMargin >= 15 && data.ProfitMargin <= 30;
            bool hasSufficientRevenue = data.MonthlyRevenue >= 100000;

            return isROIPositive && isConversionRateGood && isProfitMarginGood && hasSufficientRevenue;
        }
    }
}
