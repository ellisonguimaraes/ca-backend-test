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

No projeto, foi elaborado um **Dockerfile** dentro da API para a construção da imagem Docker correspondente, que foi posteriormente publicada no repositório [`ellisonguimaraes/billing-api:v2`](https://hub.docker.com/r/ellisonguimaraes/billing-api). Além da criação da imagem Docker, foram desenvolvidos diversos recursos para possibilitar a execução do projeto em um *cluster* local utilizando o **Kind**.

> ⚠️ **Observação:** \
> Todos os arquivos referente ao Kubernetes estão na pasta `\k8s`.

#### 📌 Manifesto `seq.yaml`: 
Responsável pela implantação do servidor de *logs* **SEQ** no *cluster*, contendo:
- Uma **Secret** denominada `seq-secret`, que armazena as credenciais de acesso (usuário e senha) ao SEQ;
- Um **StatefulSet** chamado `seq`, contendo a configuração do servidor de *logs* **SEQ**;
- Um **Service** `seq-service` do tipo **NodePort**, que expõe o SEQ externamente ao *cluster* na porta `30004`.

#### 📌 Manifesto `redis.yaml`:
Define a implantação do **Redis** para *cache* distribuído no *cluster*, contendo:
- Uma **Secret** `redis-secret` e um **ConfigMap** `redis-config`, que armazenam as informações de credenciais e configuração do Redis;
- Um **StatefulSet** denominado `redis`, responsável pela execução do Redis;
- Um **Service** `redis-service` do tipo **NodePort**, permitindo acesso externo ao Redis através da porta `30003`.

#### 📌 Manifesto `postgres.yaml`:
Define a configuração do banco de dados **PostgreSQL** dentro do *cluster*, incluindo:
- Uma **Secret** `postgres-secret` e um **ConfigMap** `postgres-config`, que armazenam as credenciais de acesso (usuário, senha e banco de dados);
- Um **StatefulSet** `postgres`, contendo a imagem do PostgreSQL;
- Um **Service** `postgres-service` do tipo **NodePort**, que permite o acesso externo ao banco de dados pela porta `30002`.

#### 📌 Manifesto `ingress-nginx.yaml`:
Responsável pela instalação do **Nginx Ingress Controller**, configurado para expor o *ingress* na porta `30000`.  
> ⚠️ **Observação:** Este é o primeiro manifesto que deve ser aplicado ao *cluster*.

#### 📌 Manifesto `deployment.yaml`:
Define a implantação da aplicação, contendo:
- Um **Deployment** que instancia a aplicação utilizando a imagem `ellisonguimaraes/billing-api:v2`.
- Um **Service** `billing-api-service` do tipo **ClusterIP**, tornando a aplicação acessível via *ingress*.
- Um **ConfigMap** `billing-api-config`, contendo as *strings* de conexão com o PostgreSQL, Redis e SEQ.

O *deployment* deste projeto inclui três tipos de ***probes*** para monitoramento e garantir a disponibilidade da aplicação:

- **Liveness Probe**: Responsável por verificar se a aplicação está em execução. Para isso, ela realiza requisições à rota de *health check* `/health`. Caso a verificação falhe, o Kubernetes entende que o contêiner está travado ou não responde corretamente e realiza sua reinicialização automática.
- **Readiness Probe**: Utilizada para determinar se a aplicação está pronta para receber requisições, também através do *health check*. Enquanto essa verificação não for bem-sucedida, o Kubernetes não encaminha tráfego para o pod, evitando que requisições sejam enviadas para instâncias ainda em processo de inicialização ou que não estejam operacionais.
- **Startup Probe**: Garantia de que a aplicação tenha tempo suficiente para iniciar antes que as outras probes comecem a atuar. Essa probe é especialmente útil para aplicações que possuem um tempo de inicialização mais longo, evitando que o Kubernetes reinicie o contêiner prematuramente.

#### 📌 Manifesto `ingress.yaml`:
Define o **Ingress**, responsável por expor a aplicação e direcionar o tráfego para o *Service* `billing-api-service`. Como o **Nginx Ingress Controller** está mapeado na porta `30000`, a aplicação aqui desenvolvida será acessível por esta porta.


## 3. Executando a Aplicação

### 3.1. Através dos arquivos manifestos Kubernetes

> ⚠️ **Observação:** \
> Todos os arquivos referente ao Kubernetes estão na pasta `\k8s`.

Para que o projeto funcione corretamente, é necessário ter os seguintes componentes instalados:  

1. **Docker** instalado na máquina;
2. **kubectl**, a ferramenta de linha de comando do Kubernetes. Para instalar, consulte a documentação oficial: [Kubernetes CLI](https://kubernetes.io/docs/tasks/tools/);
3. **Kind** (Kubernetes in Docker), utilizado para criar um *cluster* local. O guia de instalação pode ser encontrado em: [Kind Quick Start](https://kind.sigs.k8s.io/docs/user/quick-start/#installation).  

O primeiro passo é criar um *cluster* local utilizando o **Kind**. Para isso, use o arquivo de configuração `kind-config.yaml` e execute o seguinte comando:  

```shell
kind create cluster --name cluster-nexer --config kind-config.yaml
```

Após a criação do *cluster*, é necessário aplicar os arquivos de manifesto na ordem correta. Isso pode ser feito utilizando o comando abaixo:

```sh
kubectl apply -f <file_name>.yaml
```

> Para que o arquivo `ingress.yaml` consiga ser aplicado, a instalação do ***Nginx Ingress Controller*** (do arquivo `ingress-nginx.yaml`) precisa estar rodando e funcional, ou seja, pode demorar um pouco até finalizar a instalação.

Para cada um dos arquivos, seguindo esta ordem:

1. `ingress-nginx.yaml`;
2. `seq.yaml`;
3. `redis.yaml`;
4. `postgres.yaml`;
5. `deployment.yaml`.
6. `ingress.yaml`;

O arquivo `kind-config.yaml` na criação do *cluster* local expoe externamente para acesso as portas:
- `30000` referente ao *ingress* que aponta para nossa aplicação desenvolvida, logo você pode acessar através desta porta a aplicação; 
- `30002` do PostgreSQL. Caso queira acessar localmente basta acessar com o `localhost` com a porta, o usuário `user01` e a senha `Abcd@1234`; 
- `30003` do Redis, onde basta acessar localmente pela porta utilizando a senha `Abcd@1234`; 
- `30004` do SEQ, que pode acessar através do navegador com a porta, onde o primeiro *login* é necessário ser feito utilizando as credenciais `admin` com senha `Abcd@1234`.

### 3.2. Através dos arquivos do projeto

Para executar o projeto localmente, é necessário ter os seguintes componentes instalados:

- **.NET 8.0** (dotnet), garantindo compatibilidade com a aplicação;
- **Redis**, utilizado para cache distribuído;
- **PostgreSQL**, que servirá como banco de dados principal;
- O servidor de *logs* **SEQ**, para monitoramento da aplicação.

Após isso, basta configurar no arquivo `appsettings.json` no projeto de API as conexões para cada um dos componentes acima, utilizando:

- O `ConnectionStrings.DefaultConnection` para a *string* de conexão do **PostgreSQL**;
- O `Serilog.WriteTo[0].Args.serverUrl` para definir conexão do **SEQ**;
- O `RedisSettings.Host` para definir o *host* do **Redis**.

E com isso, aplicar o `dotnet build` e `dotnet run` no projeto, podendo acessar o projeto no `localhost:5000` (**Swagger**).