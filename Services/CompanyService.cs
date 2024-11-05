using System.Collections.Generic;
using System.Threading.Tasks;
using Sprint4PlusSoft.Data;
using MongoDB.Driver;
using Sprint4PlusSoft.DTOs;
using Sprint4PlusSoft.Models;

namespace Sprint4PlusSoft.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Company> _companies;
        private readonly ValidationService _validationService;

        public CompanyService(MongoDbService mongoDbService, ValidationService validationService)
        {
            _companies = mongoDbService.Database?.GetCollection<Company>("companies");
            _validationService = validationService;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _companies.Find(FilterDefinition<Company>.Empty).ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(string id)
        {
            var filter = Builders<Company>.Filter.Eq(x => x.Id, id);
            return await _companies.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Company?> GetCompanyByCNPJAsync(string cnpj)
        {
            var filter = Builders<Company>.Filter.Eq(x => x.CNPJ, cnpj);
            return await _companies.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Company?> UpdateCompanyAsync(string id, UpdateCompanyDTO companyDto)
        {
            var company = await GetCompanyByIdAsync(id);
            if (company == null) return null;
            
            if (!string.IsNullOrEmpty(companyDto.Email))
            {
                if (!_validationService.IsValidEmail(companyDto.Email))
                    throw new ArgumentException("O e-mail informado não é válido.");
        
                var emailExists = await _companies.Find(Builders<Company>.Filter.Eq(c => c.Email, companyDto.Email)).FirstOrDefaultAsync();
                if (emailExists != null) throw new InvalidOperationException("O e-mail informado já está cadastrado. Por favor, use outro e-mail.");
                
                // Chama a validação assíncrona da API
                if (!await _validationService.IsEmailValidAsync(companyDto.Email))
                    throw new ArgumentException("O e-mail informado é considerado inválido pela API.");
            }
            
            if (!string.IsNullOrEmpty(companyDto.Name)) company.Name = companyDto.Name;
            if (!string.IsNullOrEmpty(companyDto.Description)) company.Description = companyDto.Description;
            if (!string.IsNullOrEmpty(companyDto.CNPJ)) company.CNPJ = companyDto.CNPJ;
            if (!string.IsNullOrEmpty(companyDto.Email)) company.Email = companyDto.Email;
            if (!string.IsNullOrEmpty(companyDto.Status)) company.Status = companyDto.Status;

            await _companies.ReplaceOneAsync(c => c.Id == id, company);
            return company;
        }

        public async Task<Company> CreateCompanyAsync(CompanyDTO companyDto)
        {
            if (!_validationService.IsValidCNPJ(companyDto.CNPJ))
            {
                throw new ArgumentException("O CNPJ informado não é válido.");
            }
            
            var cnpjExists = await _companies
                .Find(Builders<Company>.Filter.Eq(c => c.CNPJ, companyDto.CNPJ))
                .FirstOrDefaultAsync();
            if (cnpjExists != null) throw new InvalidOperationException("O CNPJ informado já está cadastrado. Por favor, use outro CNPJ.");
        
            if (!_validationService.IsValidEmail(companyDto.Email))
            {
                throw new ArgumentException("O e-mail informado não é válido.");
            }
        
            var emailExists = await _companies
                .Find(Builders<Company>.Filter.Eq(c => c.Email, companyDto.Email))
                .FirstOrDefaultAsync();
            if (emailExists != null) throw new InvalidOperationException("O e-mail informado já está cadastrado. Por favor, use outro e-mail.");
        
            if (!await _validationService.IsEmailValidAsync(companyDto.Email))
            {
                throw new ArgumentException("O e-mail informado é considerado inválido pela API.");
            }
                
            var company = new Company()
            {
                Name = companyDto.Name,
                Description = companyDto.Description,
                CNPJ = companyDto.CNPJ,
                Email = companyDto.Email,
                Status = companyDto.Status
            };
        
            await _companies.InsertOneAsync(company);
            return company;
        }


        public async Task<bool> DeleteCompanyAsync(string id)
        {
            var filter = Builders<Company>.Filter.Eq(x => x.Id, id);
            var result = await _companies.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
