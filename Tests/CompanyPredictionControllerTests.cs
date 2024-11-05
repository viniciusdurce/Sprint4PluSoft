using Microsoft.AspNetCore.Mvc;
using Moq;
using Sprint4PlusSoft.Controllers;
using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.ML;
using Sprint4PlusSoft.Services;
using Xunit;

public class CompanyPredictionControllerTests
{
    private readonly Mock<CompanyPredictionService> _mockPredictionService;
    private readonly CompanyPredictionController _controller;

    public CompanyPredictionControllerTests()
    {
        _mockPredictionService = new Mock<CompanyPredictionService>();
        _controller = new CompanyPredictionController(_mockPredictionService.Object);
    }

    [Fact]
    public void PredictRentability_ReturnsOkResult_WithBooleanValue()
    {
        // Arrange
        var companyReportDto = new CompanyReportDTO
        {
            ROI = 120,
            MonthlyRevenue = 250000,
            ProfitMargin = 20,
            EmployeeCount = 30,
            CampaignConversionRate = 8
        };
        
        _mockPredictionService.Setup(service => service.Predict(It.IsAny<CompanyData>())).Returns(true);

        // Act
        var result = _controller.PredictRentability(companyReportDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var isRentable = Assert.IsType<bool>(okResult.Value);
        Assert.True(isRentable);
    }
}