# Sprint4PlusSoft Company Analysis API

**Sprint4PlusSoft** é uma API .NET desenvolvida para analisar a rentabilidade de empresas usando machine learning. A API permite criar relatórios de empresas, prever a rentabilidade com base em dados históricos e calcular automaticamente um indicador de rentabilidade (`Label`) usando critérios específicos.

## 📑 Índice

- [Descrição do Projeto](#descrição-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Arquitetura e Principais Componentes](#arquitetura-e-principais-componentes)
- [Critérios de Rentabilidade](#critérios-de-rentabilidade)
- [Endpoints da API](#endpoints-da-api)
- [Exemplo de Dados e Previsões](#exemplo-de-dados-e-previsões)
- [Treinamento do Modelo de Machine Learning](#treinamento-do-modelo-de-machine-learning)
- [Uso da API Validadora de E-mail](#uso-da-api-validadora-de-email)
- [Testes Implementados](#testes-implementados)
- [Práticas de Clean Code Aplicadas](#práticas-de-clean-code-aplicadas)
- [Funcionalidades de IA Generativa Adicionadas](#funcionalidades-de-ia-generativa-adicionadas)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

---

## Descrição do Projeto

Sprint4PlusSoft é uma API RESTful que utiliza machine learning para prever se uma empresa é rentável com base em características como ROI, receita mensal, margem de lucro, número de funcionários e taxa de conversão de campanhas. A API realiza o cálculo do rótulo de rentabilidade (`Label`) automaticamente, simplificando as requisições e garantindo consistência nas análises.

## Pré-requisitos

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MongoDB](https://www.mongodb.com/try/download/community) - Banco de dados para armazenar relatórios de empresas
- [ML.NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet) - Biblioteca de machine learning para .NET

## Instalação e Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/username/Sprint4PlusSoft.git
cd Sprint4PlusSoft
```

## 2. Configure o Banco de Dados MongoDB
   Crie um banco de dados MongoDB chamado CompanyReports e adicione uma coleção companiesReports. Atualize a string de conexão do MongoDB no arquivo appsettings.json da API:

```
{
"ConnectionStrings": {
"MongoDB": "mongodb://localhost:27017/CompanyReports"
}
}
```

## 3. Instale Dependências
No diretório do projeto, instale as dependências.

## Arquitetura e Principais Componentes

- **CompanyReportService**: Gerencia a criação, atualização, leitura e exclusão de relatórios de empresas, calculando automaticamente o `Label` de rentabilidade.
- **CompanyPredictionService**: Utiliza o ML.NET para treinar um modelo com os dados históricos, permitindo previsões sobre a rentabilidade de novas empresas.
- **CompanyReportDTO e CompanyData**: Estruturas de dados utilizadas para transferir dados da API e estruturar as informações para o modelo de machine learning.

## Critérios de Rentabilidade

O `Label` de rentabilidade é calculado automaticamente com base nos seguintes critérios:

- **ROI**: Maior que 100.
- **Margem de Lucro**: Entre 15% e 30%.
- **Taxa de Conversão**: Acima de 6%.
- **Receita Mensal**: Acima de 100.000.

Esses critérios foram definidos para representar uma empresa rentável no setor de serviços, comércio e indústria.

## Endpoints da API

### `POST /api/CompanyReport`

Cria um relatório de empresa com os dados fornecidos e calcula automaticamente o `Label`.

**Requisição**
```json
{
  "companyId": "1",
  "roi": 120,
  "monthlyRevenue": 250000,
  "profitMargin": 20,
  "employeeCount": 30,
  "campaignConversionRate": 8
}
```

**Resposta**

```json
{
  "id": "generated_id",
  "companyId": "1",
  "roi": 120,
  "monthlyRevenue": 250000,
  "profitMargin": 20,
  "employeeCount": 30,
  "campaignConversionRate": 8,
  "label": true
}
```
### `POST /api/CompanyPrediction/predict`
Usa os dados da empresa para prever se ela é rentável ou não, com base no modelo treinado.

**Requisição**
```json
{
  "roi": 120,
  "monthlyRevenue": 250000,
  "profitMargin": 20,
  "employeeCount": 30,
  "campaignConversionRate": 8
}

```

**Resposta**

```json
{
  "predictedLabel": true
}
```

### `GET /api/CompanyReport/{id}`
Retorna os detalhes de um relatório de empresa específico pelo ID.

### `GET /api/CompanyReport`
Retorna todos os relatórios de empresas.

### `PUT /api/CompanyReport/{id}`
Atualiza um relatório existente e recalcula o `Label`.

### `DELETE /api/CompanyReport/{id}`
Exclui um relatório de empresa pelo ID.

## Exemplo de Dados e Previsões

### Exemplo de uma Empresa Rentável (Boa)
```json
{
  "roi": 120,
  "monthlyRevenue": 250000,
  "profitMargin": 20,
  "employeeCount": 30,
  "campaignConversionRate": 8
}
```
Resultado previsto: true (Rentável)

### Exemplo de uma Empresa Não Rentável (Ruim)
```json
{
  "roi": 50,
  "monthlyRevenue": 30000,
  "profitMargin": 5,
  "employeeCount": 50,
  "campaignConversionRate": 3
}
```
Resultado previsto: false (Não Rentável)

## Treinamento do Modelo de Machine Learning

O modelo de machine learning utiliza ML.NET com o algoritmo `SdcaLogisticRegression` para classificação binária. Durante o treinamento, a API carrega os dados de `CompanyReport`, que incluem o `Label` calculado, para ensinar o modelo a identificar padrões de rentabilidade.

### Pipeline de Treinamento

1. **Transformação dos Dados**: Concatenar as características (`ROI`, `MonthlyRevenue`, etc.) em uma coluna `Features`.
2. **Treinamento**: O modelo aprende a partir dos dados históricos de `Label`.
3. **Previsão**: Com o modelo treinado, é possível fazer previsões de rentabilidade em novos dados de empresas.

## Uso da API Validadora de E-mail

A API validadora de e-mail é uma parte essencial do sistema, garantindo que os endereços de e-mail sejam válidos e funcionais antes de serem aceitos no banco de dados. Utilizamos a API Bouncer para validação de e-mails, o que nos permite garantir a integridade e a validade dos dados de contato das empresas.

### Endpoint de Validação de E-mail - Bouncer

O serviço `ValidationService` faz uma requisição GET à API Bouncer para validar cada endereço de e-mail antes de aceitar a entrada.

**Exemplo de implementação:**

```csharp
public async Task<bool> IsEmailValidAsync(string email)
{
    if (!IsValidEmail(email)) return false;

    var requestUrl = $"https://api.usebouncer.com/v1.1/email/verify?email={email}";
    var response = await _httpClient.GetAsync(requestUrl);

    if (response.IsSuccessStatusCode)
    {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EmailValidationResponse>(jsonResponse);
        return result.Status == "deliverable";
    }
    return false;
}
```

### Retornos da API de Validação

A API de validação classifica os e-mails em:

- **Deliverable**: O e-mail é seguro e aceito.
- **Risky**: E-mails com caixa de entrada cheia ou temporários.
- **Undeliverable**: E-mail inválido ou inexistente.
- **Unknown**: O servidor não respondeu.

Essa validação garante que a API só armazene e-mails que têm uma chance confiável de entrega.

## Testes Implementados

Foram implementados diversos testes unitários e de integração para garantir a confiabilidade dos serviços e a correta execução da lógica de negócio.

### Tipos de Testes

- **Testes de Unidade**: Valida a lógica interna de cada componente isoladamente.
- **Testes de Integração**: Testam a interação entre múltiplos componentes.
- **Testes de API**: Testes dos endpoints para verificar se a API retorna os dados corretos e responde com o status HTTP apropriado.

### Ferramentas de Testes

Usamos o **xUnit** como o framework de testes principal, juntamente com **Moq** para mockar dependências e simular respostas da API Bouncer em ambientes de teste.

## Práticas de Clean Code Aplicadas

Para garantir a clareza, a legibilidade e a manutenção do código, foram aplicadas as seguintes práticas de clean code:

1. **Divisão de Responsabilidades (SRP)**.
2. **Naming Conventions Consistentes**.
3. **Injeção de Dependência (DI)**.
4. **Tratamento de Exceções**.
5. **Remoção de Código Duplicado**.
6. **Documentação de Código**.

## Funcionalidades de IA Generativa Adicionadas

1. **Modelo de Machine Learning com ML.NET**.
2. **Treinamento com Dados Rotulados**.
3. **Previsão Automática**.
4. **Pipeline de Treinamento e Previsão Automatizado**.

## Tecnologias Utilizadas

- **.NET Core 6.0**
- **MongoDB**
- **ML.NET**
- **Swagger**



