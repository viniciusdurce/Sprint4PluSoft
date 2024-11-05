using Microsoft.AspNetCore.Mvc;
using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.ML;
using Sprint4PlusSoft.Services;

namespace Sprint4PlusSoft.Controllers
{
    /// <summary>
    /// Controlador para previsão de rentabilidade de empresas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyPredictionController : ControllerBase
    {
        private readonly CompanyPredictionService _predictionService;

        public CompanyPredictionController(CompanyPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        /// <summary>
        /// Prever a rentabilidade de uma empresa com base nas métricas fornecidas.
        /// </summary>
        /// <param name="companyReportDto">Dados do relatório da empresa.</param>
        /// <returns>Boolean indicando se a empresa é rentável ou não.</returns>
        [HttpPost("predict")]
        public ActionResult<bool> PredictRentability([FromBody] CompanyReportDTO companyReportDto)
        {
            var companyData = new CompanyData
            {
                ROI = companyReportDto.ROI,
                MonthlyRevenue = companyReportDto.MonthlyRevenue,
                ProfitMargin = companyReportDto.ProfitMargin,
                EmployeeCount = companyReportDto.EmployeeCount,
                CampaignConversionRate = companyReportDto.CampaignConversionRate
            };

            bool isRentable = _predictionService.Predict(companyData);
            return Ok(isRentable);
        }
    }
}