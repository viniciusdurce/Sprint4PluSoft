using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.Models;
using Sprint4PlusSoft.Services;

namespace Sprint4PlusSoft.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de relatórios da empresa.
    /// Oferece endpoints para criar, atualizar, buscar e deletar relatórios da empresa.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyReportController : ControllerBase
    {
        private readonly ICompanyReportService _companyReportService;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="CompanyReportController"/>.
        /// </summary>
        /// <param name="companyReportService">Serviço de gerenciamento de relatórios das empresas.</param>
        public CompanyReportController(ICompanyReportService companyReportService)
        {
            _companyReportService = companyReportService;
        }

        /// <summary>
        /// Cria novos relatórios de empresas em massa.
        /// </summary>
        /// <param name="companyReportDtos">Lista de dados dos relatórios de empresas a serem criados.</param>
        /// <returns>Confirmação da criação dos relatórios de empresas.</returns>
        /// <response code="201">Relatórios de empresa criados com sucesso.</response>
        /// <response code="400">Dados inválidos para criação dos relatórios.</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CompanyReport>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Create([FromBody] List<CompanyReportDTO> campaignReportDtos)
        {
            var createdReports = new List<CompanyReport>();
            foreach (var campaignReportDto in campaignReportDtos)
            {
                var createdReport = await _companyReportService.CreateAsync(campaignReportDto);
                createdReports.Add(createdReport);
            }
            return CreatedAtAction(nameof(GetAll), createdReports);
        }


        /// <summary>
        /// Busca todos os relatórios de empresas.
        /// </summary>
        /// <returns>Lista de relatórios de empresas existentes.</returns>
        /// <response code="200">Retorna a lista de relatórios de empresas.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CompanyReport>), 200)]
        public async Task<ActionResult<IEnumerable<CompanyReport>>> GetAll()
        {
            var reports = await _companyReportService.GetAllAsync();
            return Ok(reports);
        }

        /// <summary>
        /// Busca um relatório de empresas específico pelo ID.
        /// </summary>
        /// <param name="id">ID do relatório de empresa.</param>
        /// <returns>O relatório de empresas correspondente ao ID.</returns>
        /// <response code="200">Retorna o relatório de empresas com o ID especificado.</response>
        /// <response code="404">Relatório de empresas não encontrado para o ID especificado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyReport), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CompanyReport>> GetById(string id)
        {
            var report = await _companyReportService.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }

        /// <summary>
        /// Atualiza um relatório de empresa existente.
        /// </summary>
        /// <param name="id">ID do relatório de empresa a ser atualizado.</param>
        /// <param name="companyReportDto">Dados atualizados do relatório de empresa.</param>
        /// <returns>Status da operação de atualização.</returns>
        /// <response code="204">Relatório de empresa atualizado com sucesso.</response>
        /// <response code="404">Relatório de empresa não encontrado para o ID especificado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(string id, [FromBody] CompanyReportDTO companyReportDto)
        {
            var updated = await _companyReportService.UpdateAsync(id, companyReportDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Exclui um relatório de empresa pelo ID.
        /// </summary>
        /// <param name="id">ID do relatório de empresa a ser excluído.</param>
        /// <returns>Status da operação de exclusão.</returns>
        /// <response code="204">Relatório de empresa excluído com sucesso.</response>
        /// <response code="404">Relatório de empresa não encontrado para o ID especificado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await _companyReportService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
