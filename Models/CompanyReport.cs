using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Sprint4PlusSoft.Models
{
    public class CompanyReport
    {
        /// <summary>
        /// Identificador único do relatório.
        /// </summary>
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public string Id { get; set; }

        /// <summary>
        /// ID da empresa específica à qual o relatório está associado.
        /// </summary>
        [BsonRequired]
        public string CompanyId { get; set; }

        /// <summary>
        /// Retorno sobre Investimento (ROI) da empresa.
        /// </summary>
        public float ROI { get; set; }

        /// <summary>
        /// Faturamento mensal da empresa.
        /// </summary>
        public float MonthlyRevenue { get; set; }

        /// <summary>
        /// Margem de lucro da empresa (em porcentagem).
        /// </summary>
        public float ProfitMargin { get; set; }

        /// <summary>
        /// Quantidade total de funcionários na empresa.
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// Taxa de conversão em campanhas (em porcentagem).
        /// </summary>
        public float CampaignConversionRate { get; set; }
        
        
        /// <summary>
        /// Verificação se a empresa é rentável.
        /// </summary>
        public bool Label { get; set; }

        // Construtor padrão
        public CompanyReport() { }

        /// <summary>
        /// Construtor para facilitar a criação de instâncias de CompanyReport.
        /// </summary>
        /// <param name="companyId">ID da empresa associada.</param>
        /// <param name="roi">Retorno sobre Investimento.</param>
        /// <param name="monthlyRevenue">Faturamento mensal.</param>
        /// <param name="profitMargin">Margem de lucro.</param>
        /// <param name="employeeCount">Quantidade de funcionários.</param>
        /// <param name="campainConversionRate">Taxa de conversão em campanhas.</param>
        public CompanyReport(string companyId, float roi, float monthlyRevenue, float profitMargin,
                             int employeeCount, float campaignConversionRate, bool label)
        {
            CompanyId = companyId;
            ROI = roi;
            MonthlyRevenue = monthlyRevenue;
            ProfitMargin = profitMargin;
            EmployeeCount = employeeCount;
            CampaignConversionRate = campaignConversionRate;
            Label = label;
        }
    }
}
