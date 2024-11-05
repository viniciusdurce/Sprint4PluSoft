namespace Sprint4PlusSoft.DTOs
{
    public class CompanyReportDTO
    {
        /// <summary>
        /// ID da empresa associada. Este campo é obrigatório.
        /// </summary>
        public string? CompanyId { get; set; }

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
    }
}