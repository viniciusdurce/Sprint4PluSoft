using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sprint4PlusSoft.Services
{
    public class ValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "EdMryPzR5MSOS1wv5IMIjno1Y3s2LNfivyJZ4X4a");
        }

        /// <summary>
        /// Valida se o e-mail contém o símbolo "@" e tem um formato mínimo válido.
        /// </summary>
        public bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@");
        }

        /// <summary>
        /// Verifica se o CNPJ possui exatamente 14 dígitos numéricos.
        /// </summary>
        public bool IsValidCNPJ(string cnpj)
        {
            return !string.IsNullOrEmpty(cnpj) && cnpj.Length == 14 && long.TryParse(cnpj, out _);
        }

        /// <summary>
        /// Valida o e-mail utilizando a API externa do Bouncer.
        /// </summary>
        public async Task<bool> IsEmailValidAsync(string email)
        {
            if (!IsValidEmail(email)) return false;

            // Codifica o e-mail para uso em URL
            var encodedEmail = Uri.EscapeDataString(email);
            var requestUrl = $"https://api.usebouncer.com/v1.1/email/verify?email={encodedEmail}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializa apenas o campo 'status'
                    var result = JsonSerializer.Deserialize<EmailValidationResponse>(jsonResponse);

                    if (result != null)
                    {
                        var status = result.Status;

                        // Avalia o status retornado pela API
                        if (status == "deliverable")
                        {
                            return true;
                        }
                        else if (status == "risky")
                        {
                            // Decida se deseja considerar "risky" como válido
                            return false; // ou true, dependendo da sua necessidade
                        }
                        else // "undeliverable" ou "unknown"
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // Opcionalmente, você pode registrar o código de status ou mensagem de erro
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Trate exceções de acordo com a necessidade
                Console.WriteLine($"Erro ao validar o e-mail: {ex.Message}");
            }

            return false;
        }
    }

    // Classe para deserialização contendo apenas o campo 'status'
    public class EmailValidationResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
