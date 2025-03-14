# 1. Projeto

Este projeto se trata de um teste para vaga de Desenvolvimento Back-end .NET na empresa Nexer. Este teste trata-se de criar uma API REST para gerenciar faturamento de clientes, com as características descritas abaixo.

**Funcionalidades 🛠️**

* Customer: CRUD; Criar um cadastro do cliente com os seguintes campos:
    * Id;
    * Name;
    * Email;
    * Address;
    * **Todos os campos são de preenchimento obrigatório.**
* Produtos: CRUD; Criar um cadastro de produtos com os seguintes campos:
    * Id;
    * Nome do produto;
    * **Todos os campos são de preenchimento obrigatório.**
* Controle de conferência e importação de billing.
    * Utilizar postman para consulta dos dados da API’s para criação das tabelas de billing e billingLines.
	  * Após consulta, e criação do passo anterior, inserir no banco de dados o primeiro registro do retorno da API de billing para criação de cliente e produto através do swagger ou dataseed.

    * Utilizar as API’s para consumo dos dados a partir da aplicação que está criada e fazer as seguintes verificações:
      * Se o cliente e o produto existirem, inserir o registro do billing e billingLines no DB local.
      * Caso se o cliente existir ou só o produto existir, deve retornar um erro na aplicação informando sobre a criação do registro faltante.
      * Criar exceptions tratando mal funcionamento ou interrupção de serviço quando API estiver fora.
* Lista de API’s :
	* Get https://65c3b12439055e7482c16bca.mockapi.io/api/v1/billing
	* Get https://65c3b12439055e7482c16bca.mockapi.io/api/v1/billing/:id
	* Post https://65c3b12439055e7482c16bca.mockapi.io/api/v1/billing
	* Delete https://65c3b12439055e7482c16bca.mockapi.io/api/v1/billing/:id
	* PUT https://65c3b12439055e7482c16bca.mockapi.io/api/v1/billing/:id

**Requisitos 💻**

* A aplicação deverá ser desenvolvida usando .NET a partir da versão 5+;
* Modelagem de dados pode ser no banco de dados de sua preferência, podendo ser um banco relacional ou não relacional (mongodb, SQL Server, PostgreSQL, MySQL, etc);
* Persistência de dados no banco deverá ser feita utilizando o Entity Framework Core;
* O retorno da API deverá ser em formato JSON;
* Utilizar as requisições GET, POST, PUT ou DELETE, conforme a melhor prática;
* Criar o README do projeto descrevendo as tecnologias utilizadas, chamadas dos serviços e configurações necessário para executar a aplicação.

**Pontos Extras ⭐**

* Desenvolvimento baseado em TDD;
* Práticas de modelagem de projeto;
* Criar e configurar o Swagger da API de acordo com as melhores práticas;
* Criar uma API para extração dos dados de faturamento.
* Sugestões serão bem vindas.

**Submissão do teste 📝**

Crie um fork do teste para acompanharmos o seu desenvolvimento através dos seus commits.

# 2. Solução

## 2.1. Arquitetura, Tecnologias e Funcionalidades

### 2.1.1. CQRS

O projeto foi construído seguindo os princípios do CQRS, onde as operações de leitura (*queries*) são separadas das operações de escrita (*commands*), permitindo maior escalabilidade, otimização de desempenho e flexibilidade na modelagem dos dados.

Além disso, utilizamos **Cache Distribuído** (com Redis) para armazenar os resultados das consultas, reduzindo a carga no banco de dados e melhorando a eficiência das operações de leitura. As operações de atualizações do *cache* estão em **Notifications** do **MediatR**, garantindo que os dados armazenados reflitam as alterações realizadas no sistema.

Os *commands* são: 

- Customers: 
  - `CreateCustomerCommand` que é responsável por criar um novo `Customer`;
  - `UpdateCustomerCommand` que é responsável por atualizar um `Customer` existente.
- Products:
  - `CreateProductCommand` que é responsável por criar um novo `Product`;
  - `UpdateProductCommand` que é responsável por atualizar um `Product` existente.
- Billings: 
  - `ImportBillingCommand` que é responsável por importar (da API externa) os `Billings` e `BillingLines` existentes.

As *queries*:

- Customers: 
  - `GetAllCustomerQuery` que é responsável por obter `Customers` de forma paginada;
  - `GetCustomerByIdQuery` que é responsável por obter um `Customer` pelo seu identificador.
- Products: 
  - `GetAllProductQuery` que é responsável por obter `Products` de forma paginada;
  - `GetProductByIdQuery` que é responsável por obter um `Product` pelo seu identificador.

E as *notifications*:
- `DeleteAllPaginatedEntityInCacheNotification` que deleta todos os dados paginados em *cache* quando há alterações no banco; 
- `UpdateEntityInCacheNotification` que atualiza uma entidade *cache* quando ela é consultada no banco, criada ou alterada;
- `UpdatePaginateEntityInCacheNotification` que atualiza entidades paginadas em *cache* quando é feita uma consulta paginada no banco de dados.

Quando ocorre uma alteração no banco de dados, seja um create ou update de Customer ou Product, a informação é atualizada no cache e todos os dados paginados são removidos. Isso garante que a paginação não exiba informações desatualizadas, já que os dados armazenados anteriormente não refletem as novas inserções ou modificações. Além disso, as consultas ao banco de dados relacional são seguidas pelo armazenamento dos resultados no cache, permitindo que consultas subsequentes recuperem os dados diretamente do Redis, reduzindo a carga no banco de dados e melhorando a performance do sistema.

### 2.1.2. Middleware de Exceptions 

No projeto foi desenvolvido um *middleware* para tratar *exceptions* de forma global, o `ExceptionMiddleware` (presente no projeto de API). Para cada tipo de *exception* mapeada contém uma classe *handle* que trata a resposta de forma diferente para cada um dos tipos de *exception*. Algumas das *exceptions* mapeadas são: 

- `ApiExceptionHandler`, um *handle* para um *exception* customizado (`ApiException`) para tratar o erro de API externa de *billing*;
- `BusinessExceptionHandler`, também customizado para tratar erros de negócio (`BusinessException`);
- `CustomUnsupportedApiVersionExceptionHandler` que trata o erro para o tipo de *exception* `CustomUnsupportedApiVersionException`, esta que também é customizada e é lançada quando o usuário tenta acessar uma versão não existente da rota (a API contém versionamento de *endpoints*);
- `ValidationExceptionHandler` que trata de erros `ValidationException`, um tipo de *exception* gerada quando a validação do *Fluent Validation* não é satisfeita.

Além de tratar resposta, os *handlers* também são utilizados para registrar *logs* no SEQ/arquivo/terminal com o erro, motivo, *trace*, entre outros dados.

### 2.1.3. Versionamento de API

O projeto conta com versionamento de *endpoints*, onde a versão é especificada na URL da requisição (*path*) e também incluída nos *response headers*. Essa abordagem garante maior controle sobre a evolução da API, permitindo a manutenção de diferentes versões simultaneamente sem impactar consumidores existentes.

### 2.1.4. Validação de dados da requisição

Utilizamos **FluentValidation** para validar todas as requisições do **CRUD**, garantindo que mensagens de erro claras sejam retornadas ao cliente.

A validação é centralizada no *behavior* `ValidationBehavior.cs`, uma camada intermediária entre o *controller* e o os *handlers* do **MediatR**. Essa abordagem elimina a necessidade de incluir validações diretamente nos *handlers*, reforçando o **Princípio da Responsabilidade Única** (SRP). Dessa forma, o *handler* permanece focado exclusivamente em executar sua lógica principal, enquanto o *behavior* gerencia a validação das requisições de forma desacoplada e reutilizável.

### 2.1.5. Validação de performance do Handle

Implementamos no projeto um mecanismo de verificação de performance para cada *command* e *query* *handler*. No `appsettings.json`, registramos o tempo médio de execução esperado para cada *handler* em milissegundos. Com base nisso, o *behavior* `PerformanceBehavior` monitora a execução e compara com o tempo médio definido. Caso o tempo exceda o limite estabelecido, um *warning* *log* é gerado, alertando sobre a possível degradação de desempenho na respectiva rota.

Essa verificação é fundamental para identificar gargalos de performance, seja no acesso ao banco de dados, na comunicação com serviços externos ou até mesmo em operações internas dentro do *cluster*, permitindo otimizações proativas no sistema.

### 2.1.6. Conversão de objetos com AutoMapper

Utilizamos AutoMapper para automatizar a conversão entre DTOs e entidades, reduzindo a necessidade de mapeamentos manuais e tornando o código mais limpo e manutenível.

### 2.1.7. Serilog e SEQ

Utilizamos SEQ em conjunto com Serilog para centralizar e visualizar os *logs*, facilitando o monitoramento e a depuração do sistema de forma eficiente. Os *logs* são registrados no SEQ, arquivo e terminal.

### 2.1.8. Retry com Polly

Para a integração com a API de *billing*, utilizamos o `HttpClient` em conjunto com a estratégia de *retry* da biblioteca **Polly**. Esse mecanismo permite que, em caso de falha na chamada externa, a requisição seja reexecutada automaticamente até três vezes, com um intervalo exponencial entre as tentativas. O tempo de espera segue a fórmula $2^N$  segundos, onde $N$ representa o número da tentativa atual. Dessa forma, a primeira repetição ocorre após 2 segundos, a segunda após 4 segundos e a terceira após 8 segundos, aumentando gradualmente o tempo de espera para evitar sobrecarga no serviço externo.

### 2.1.9. PostgreSQL com EF e Repositorio Genérico

O projeto utiliza **PostgreSQL** como banco de dados, em conjunto com o E**ntity Framework Core** para o mapeamento objeto-relacional (ORM). As configurações e alterações no banco são gerenciadas por meio de *migrations*.

Além disso, adotamos um repositório genérico para todas as entidades do sistema, promovendo reutilização de código e padronização no acesso aos dados. Esse repositório encapsula as operações comuns de CRUD (*Create, Read, Update, Delet*e), proporcionando uma camada de abstração a dados.

### 2.1.10. Centralização de mensagens de erro

Centralizamos todas as mensagens de erro em arquivos de recursos (`.resx`), permitindo uma manutenção mais organizada e facilitando a reutilização das mensagens em diferentes partes do sistema. Essa abordagem melhora a padronização, evita duplicação de código e simplifica futuras alterações, além de possibilitar a localização (*localization*), caso seja necessário oferecer suporte a múltiplos idiomas futuramente.

### 2.1.11. Swagger

Também utilizamos o Swagger para a documentação da API, facilitando a visualização e o consumo dos *endpoints*. Além disso, enriquecemos essa documentação incluindo descrições detalhadas, respostas esperadas e exemplos, utilizando o XML gerado a partir dos comentários no código-fonte (C# XML Docs). Essa abordagem melhora a clareza da documentação, tornando mais fácil para desenvolvedores entenderem e interagirem com a API de forma eficiente.

### 2.1.12. Docker e Kubernetes

...

## 3. Executando a Aplicação

...