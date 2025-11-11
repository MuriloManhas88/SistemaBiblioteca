# Sistema de Biblioteca

Sistema de gerenciamento de biblioteca desenvolvido em C# com ASP.NET Core Web API.

## Etapa 1 - Modelagem do Domínio

### Entidades Implementadas

#### Livro
- ISBN (identificador único)
- Título
- Autor
- Categoria (FICCAO, TECNICO, DIDATICO)
- Status (DISPONIVEL, EMPRESTADO, RESERVADO)
- Data de cadastro

#### Usuário
- ID (identificador único)
- Nome
- Email
- Tipo (ALUNO, PROFESSOR, FUNCIONARIO)
- Data de cadastro

#### Empréstimo
- ID do empréstimo
- ISBN do livro
- ID do usuário
- Data do empréstimo
- Data prevista de devolução
- Data real de devolução (opcional)
- Status (ATIVO, FINALIZADO, ATRASADO)

#### Multa
- ID do empréstimo
- Valor da multa
- Status (PENDENTE, PAGA)

### Regras de Negócio

- Usuários não podem ter mais de 3 empréstimos ativos simultaneamente
- Livros emprestados não podem ser reservados
- Multa é calculada com base no atraso (R$ 1,00 por dia)
- Professores têm prazo de empréstimo maior que alunos

### Configuração do Banco de Dados

O projeto utiliza **Entity Framework Core** com banco de dados **InMemory** para desenvolvimento e testes.

## Como Executar

```bash
dotnet restore
dotnet run
```

Acesse a documentação Swagger em: `https://localhost:5001/swagger`
