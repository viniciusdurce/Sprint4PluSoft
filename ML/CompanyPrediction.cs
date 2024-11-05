using Microsoft.ML.Data;

namespace Sprint4PlusSoft.ML;

public class CompanyPrediction
{
    [ColumnName("PredictedLabel")]
    public bool PredictedLabel { get; set; }
}