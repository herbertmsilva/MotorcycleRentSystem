# Gestão de Aluguel de Motos e Entregadores

Este projeto é uma aplicação de gerenciamento de aluguel de motos e entregadores. A aplicação permite o cadastro de motos, consulta e modificação de motos, cadastro de entregadores, upload de fotos de CNH, aluguel de motos e cálculo de multas por atraso.

## Tecnologias Utilizadas

- **Linguagem**: C# com .NET
- **Banco de Dados**: PostgreSQL para armazenamento relacional e MongoDB para gravação de eventos, logs, auditorias e configurações específicas.
- **Mensageria**: RabbitMQ para comunicação assíncrona.
- **Padrões de Projeto**: CQRS (Command Query Responsibility Segregation), SOLID.

## Estrutura do Projeto

- **src**: Diretório principal que contém o código-fonte.
  - **api**: Camada de apresentação da aplicação.
  - **Aplicação**: Contém casos de uso e lógica de negócios.
  - **Domínio**: Entidades e regras de negócio.
  - **Infraestrutura**: Implementações de repositórios, serviços de mensageria (RabbitMQ), e configurações de banco de dados.
  - **Persistência**: Separa as entidades e repositórios do PostgreSQL e MongoDB.

## Funcionalidades

- Cadastro e gerenciamento de motos.
- Cadastro e gerenciamento de entregadores.
- Upload de fotos de CNH de entregadores.
- Aluguel de motos e controle de disponibilidade.
- Cálculo de multas por atraso no retorno de motos alugadas.
- Gravação de eventos, logs, auditorias e configurações em MongoDB.

## Como Executar

1. Clone o repositório:
    ```bash
    git clone [https://github.com/seu-usuario/nome-do-repositorio.git](https://github.com/herbertmsilva/MotorcycleRentSystem)
    ```

2. Navegue até o diretório do projeto:
    ```bash
    cd MotorcycleRentSystem
    ```

3. Execute o Docker Compose para construir e iniciar os serviços necessários:
    ```bash
    docker-compose up --build
    ```

4. Após os serviços estarem ativos, rode as migrações para configurar o banco de dados:
    ```bash
    dotnet ef database update -p .\MotorcycleRentalSystem.Persistence\ -s .\MotorcycleRentalSystem.Api\
    ```

5. Compile e execute o projeto:
    ```bash
    dotnet run
    ```

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

## Licença

Este projeto está licenciado sob a Licença MIT.
