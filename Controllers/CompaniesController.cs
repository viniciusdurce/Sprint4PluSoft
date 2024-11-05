using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.Models;
using Sprint4PlusSoft.Services;

namespace Sprint4PlusSoft.Controllers
{
    /// <summary>
    /// Controlador para gerenciar cadastro de Empresas no sistema.
    /// Este controlador fornece endpoints para criar, ler, atualizar e deletar cadastros de empresas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _companyService;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CompaniesController"/>.
        /// </summary>
        /// <param name="companyService">O serviço responsável pelas operações da empresa.</param>
        public CompaniesController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Obtém uma lista de todas as Empresas.
        /// </summary>
        /// <returns>Uma lista de empresas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCampaigns()
        {
            var company = await _companyService.GetCompaniesAsync();
            return Ok(company);
        }

        /// <summary>
        /// Obtém uma empresa específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da empresa a ser obtida.</param>
        /// <returns>A empresa correspondente ao ID fornecido, ou um status 404 se não for encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Company?>> GetCampaignById(string id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            return company is not null ? Ok(company) : NotFound();
        }

        /// <summary>
        /// Obtém uma empresa específica pelo seu CNPJ.
        /// </summary>
        /// <param name="cnpj">O CNPJ da empresa a ser obtida.</param>
        /// <returns>A empresa correspondente ao CNPJ fornecido, ou um status 404 se não for encontrada.</returns>
        [HttpGet("cnpj/{cnpj}")]
        public async Task<ActionResult<Company?>> GetCampaignByCNPJ(string cnpj)
        {
            var company = await _companyService.GetCompanyByCNPJAsync(cnpj);
            return company is not null ? Ok(company) : NotFound();
        }

        /// <summary>
        /// Atualiza uma empresa existente.
        /// </summary>
        /// <param name="id">O ID da empresa a ser atualizada.</param>
        /// <param name="companyDto">O objeto contendo os dados atualizados da empresa.</param>
        /// <returns>Um status 200 se a atualização for bem-sucedida, um status 404 se a empresa não for encontrada, ou um status 400/409 em caso de erro.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(string id, UpdateCompanyDTO companyDto)
        {
            try
            {
                var updateCompany = await _companyService.UpdateCompanyAsync(id, companyDto);
                return updateCompany != null ? Ok(updateCompany) : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cria uma nova empresa.
        /// </summary>
        /// <param name="companyDto">Os dados da nova empresa a ser criada.</param>
        /// <returns>Um status 201 se a empresa for criada com sucesso, ou um status 400/409 em caso de erro.</returns>
        [HttpPost]
        public async Task<ActionResult<Company>> PostCampaign(CompanyDTO companyDto)
        {
            try
            {
                var company = await _companyService.CreateCompanyAsync(companyDto);
                return CreatedAtAction(nameof(GetCampaignById), new { id = company.Id }, company);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deleta uma empresa específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da empresa a ser deletada.</param>
        /// <returns>Um status 200 se a empresa for deletada com sucesso, ou um status 404 se a empresa não for encontrada.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var deleted = await _companyService.DeleteCompanyAsync(id);
            return deleted ? Ok() : NotFound();
        }
    }
}
