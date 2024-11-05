using Microsoft.ML;
using MongoDB.Driver;
using System;
using System.Linq;
using Sprint4PlusSoft.ML;
using Sprint4PlusSoft.Models;

namespace Sprint4PlusSoft.Services
{
    public class CompanyPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly Lazy<ITransformer> _lazyModel;
        private readonly IMongoDatabase _database;

        public CompanyPredictionService(IMongoDatabase database)
        {
            _mlContext = new MLContext();
            _database = database;
            _lazyModel = new Lazy<ITransformer>(() => TrainModel());
        }

        private ITransformer TrainModel()
        {
            // Carregar dados da coleção CompanyReports
            var companyCollection = _database.GetCollection<CompanyReport>("companiesReports");
            var companyData = companyCollection.Find(_ => true).ToList();

            // Verifica se há dados suficientes para treinamento
            if (companyData.Count < 10)
            {
                throw new InvalidOperationException("Dados insuficientes para treinamento do modelo.");
            }

            // Converte os dados para o tipo de entrada necessário, usando o campo `Label` calculado
            var trainingData = companyData.Select(data => new CompanyData
            {
                ROI = data.ROI,
                MonthlyRevenue = data.MonthlyRevenue,
                ProfitMargin = data.ProfitMargin,
                EmployeeCount = data.EmployeeCount,
                CampaignConversionRate = data.CampaignConversionRate,
                Label = data.Label // Usa o campo Label calculado e armazenado no banco de dados
            }).ToList();

            // Carregar os dados em um IDataView do ML.NET
            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Definir e treinar o pipeline de classificação binária
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(CompanyData.ROI), nameof(CompanyData.MonthlyRevenue),
                    nameof(CompanyData.ProfitMargin), nameof(CompanyData.EmployeeCount), nameof(CompanyData.CampaignConversionRate))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

            return pipeline.Fit(dataView);
        }

        public bool Predict(CompanyData companyData)
        {
            var model = _lazyModel.Value;
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CompanyData, CompanyPrediction>(model);
            var prediction = predictionEngine.Predict(companyData);
            return prediction.PredictedLabel;
        }
    }
}
