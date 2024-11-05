using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sprint4PlusSoft.Controllers;
using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.Models;
using Sprint4PlusSoft.Services;
using Xunit;

public class CompaniesControllerTests
{
    private readonly Mock<CompanyService> _mockCompanyService;
    private readonly CompaniesController _controller;

    public CompaniesControllerTests()
    {
        _mockCompanyService = new Mock<CompanyService>();
        _controller = new CompaniesController(_mockCompanyService.Object);
    }

    [Fact]
    public async Task GetCampaigns_ReturnsOkResult_WithListOfCompanies()
    {
        // Arrange
        var companies = new List<Company> { new Company { Id = "1", Name = "Test Company" } };
        _mockCompanyService.Setup(service => service.GetCompaniesAsync()).ReturnsAsync(companies);

        // Act
        var result = await _controller.GetCampaigns();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCompanies = Assert.IsType<List<Company>>(okResult.Value);
        Assert.Equal(companies.Count, returnedCompanies.Count);
    }

    [Fact]
    public async Task GetCampaignById_ReturnsNotFound_WhenCompanyDoesNotExist()
    {
        // Arrange
        _mockCompanyService.Setup(service => service.GetCompanyByIdAsync(It.IsAny<string>())).ReturnsAsync((Company)null);

        // Act
        var result = await _controller.GetCampaignById("invalid_id");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostCampaign_ReturnsCreatedResult_WhenCompanyIsCreated()
    {
        // Arrange
        var companyDto = new CompanyDTO { Name = "New Company" };
        var createdCompany = new Company { Id = "1", Name = companyDto.Name };
        _mockCompanyService.Setup(service => service.CreateCompanyAsync(companyDto)).ReturnsAsync(createdCompany);

        // Act
        var result = await _controller.PostCampaign(companyDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedCompany = Assert.IsType<Company>(createdResult.Value);
        Assert.Equal(createdCompany.Name, returnedCompany.Name);
    }

    [Fact]
    public async Task DeleteCompany_ReturnsNotFound_WhenCompanyDoesNotExist()
    {
        // Arrange
        _mockCompanyService.Setup(service => service.DeleteCompanyAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteCompany("invalid_id");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
