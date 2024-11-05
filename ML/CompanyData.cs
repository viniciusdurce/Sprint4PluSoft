using Microsoft.ML.Data;

namespace Sprint4PlusSoft.ML
{
    public class CompanyData
    {
        [LoadColumn(0)]
        public float ROI { get; set; }

        [LoadColumn(1)]
        public float MonthlyRevenue { get; set; }

        [LoadColumn(2)]
        public float ProfitMargin { get; set; }

        [LoadColumn(3)]
        public float EmployeeCount { get; set; }

        [LoadColumn(4)]
        public float CampaignConversionRate { get; set; }
    
        [LoadColumn(5), ColumnName("Label")]
        public bool Label { get; set; }
    }
}
