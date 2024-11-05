using System;
using System.ComponentModel.DataAnnotations;

namespace Sprint4PlusSoft.DTOs
{
    /// <summary>
    /// Data Transfer Object para a criação e atualização de empresas.
    /// </summary>
    public class CompanyDTO
    {
        /// <summary>
        /// Nome da empresa.
        /// </summary>
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        public string Name { get; set; }

        /// <summary>
        /// Descrição detalhada da empresa.
        /// </summary>
        [Required(ErrorMessage = "A descrição é obrigatória.")] 
        public string Description { get; set; }
        
        /// <summary>
        /// CNPJ da empresa.
        /// </summary>
        [Required(ErrorMessage = "O CNPJ é obrigatório."), MinLength(14, ErrorMessage = "O CNPJ deve ter 14 caracteres.")]
        public String CNPJ { get; set; }
        
        /// <summary>
        /// Email da empresa. Deve conter o endereçio de email "ex: @xxx.xxx"
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        public String Email { get; set; }
        
        /// <summary>
        /// Status atual da empresa.
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = "Ativa";
    }
}