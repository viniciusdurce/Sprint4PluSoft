using Sprint4PlusSoft.Models;

namespace Sprint4PlusSoft.DTOs
{
    /// <summary>
    /// Data Transfer Object para atualização de empresas.
    /// </summary>
    public class UpdateCompanyDTO
    {
        /// <summary>
        /// Nome da empresa.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descrição detalhada da empresa.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// CNPJ da empresa.
        /// </summary>
        public string? CNPJ { get; set; }

        /// <summary>
        /// Email da empresa. Deve conter o endereço de email "ex: @xxx.xxx".
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// Status atual da empresa.
        /// </summary>
        public string? Status { get; set; } = "Ativa";
    }
}