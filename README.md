# Sistema de Biblioteca

Sistema completo de gerenciamento de biblioteca desenvolvido em C# com ASP.NET Core Web API, implementando controle de empréstimos, devoluções, multas e relatórios.

## Descrição do Projeto

Este sistema foi desenvolvido para gerenciar operações de uma biblioteca, incluindo cadastro de livros e usuários, controle de empréstimos e devoluções, cálculo automático de multas por atraso e geração de relatórios gerenciais.

## Tecnologias Utilizadas

- **ASP.NET Core 8.0** - Framework Web API
- **Entity Framework Core** - ORM para acesso a dados
- **InMemory Database** - Banco de dados em memória para desenvolvimento e testes
- **Swagger/OpenAPI** - Documentação interativa da API

## Arquitetura do Projeto

O projeto segue uma arquitetura em camadas com separação de responsabilidades:

- **Models** - Entidades do domínio
- **Enums** - Enumerações para categorias e status
- **Services** - Lógica de negócio e regras
- **Controllers** - Endpoints da API REST
- **Data** - Contexto do banco de dados
- **Exceptions** - Exceções personalizadas

## Diagrama de Entidades

```
┌─────────────────┐         ┌──────────────────┐         ┌─────────────────┐
│     Livro       │         │   Empréstimo     │         │    Usuário      │
├─────────────────┤         ├──────────────────┤         ├─────────────────┤
│ ISBN (PK)       │◄────────│ ISBN (FK)        │────────►│ Id (PK)         │
│ Titulo          │         │ UsuarioId (FK)   │         │ Nome            │
│ Autor           │         │ DataEmprestimo   │         │ Email           │
│ Categoria       │         │ DataPrevista     │         │ Tipo            │
│ Status          │         │ DataReal         │         │ DataCadastro    │
│ DataCadastro    │         │ Status           │         └─────────────────┘
└─────────────────┘         └──────────────────┘
                                     │
                                     │
                                     ▼
                            ┌──────────────────┐
                            │      Multa       │
                            ├──────────────────┤
                            │ Id (PK)          │
                            │ EmprestimoId(FK) │
                            │ Valor            │
                            │ Status           │
                            └──────────────────┘
```

## Regras de Negócio Implementadas

### Empréstimos

O sistema implementa as seguintes regras para empréstimos de livros:

**Limite de Empréstimos**: Cada usuário pode ter no máximo 3 empréstimos ativos simultaneamente. Tentativas de exceder este limite resultam em erro `LimiteEmprestimosExcedidoException`.

**Disponibilidade de Livros**: Apenas livros com status DISPONIVEL podem ser emprestados. Livros já emprestados ou reservados não podem ser objeto de novo empréstimo, gerando erro `LivroIndisponivelException`.

**Prazo de Devolução**: O prazo varia conforme o tipo de usuário. Professores têm 30 dias para devolução, enquanto alunos e funcionários têm 14 dias.

**Bloqueio por Multas**: Usuários com multas pendentes não podem realizar novos empréstimos até quitarem seus débitos, conforme validação `MultaPendenteException`.

### Multas

O cálculo de multas é realizado automaticamente no momento da devolução:

**Cálculo Automático**: Quando a data real de devolução excede a data prevista, o sistema calcula automaticamente a multa à razão de R$ 1,00 por dia de atraso.

**Status do Empréstimo**: Empréstimos devolvidos com atraso têm seu status alterado para ATRASADO, facilitando a geração de relatórios.

**Controle de Pagamento**: Multas são criadas com status PENDENTE e podem ser pagas através do endpoint específico, alterando seu status para PAGA.

### Validações de Integridade

O sistema implementa validações rigorosas para garantir a integridade dos dados:

- Verificação de existência de livros antes de operações
- Verificação de existência de usuários antes de empréstimos
- Validação de status de empréstimo antes de devolução
- Prevenção de operações em entidades inexistentes

## Endpoints da API

### Livros

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/livros` | Cadastrar novo livro |
| GET | `/api/livros/{isbn}` | Obter livro por ISBN |
| GET | `/api/livros` | Listar todos os livros |
| PUT | `/api/livros/{isbn}/status` | Atualizar status do livro |

### Usuários

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/usuarios` | Cadastrar novo usuário |
| GET | `/api/usuarios/{id}` | Obter usuário por ID |
| GET | `/api/usuarios` | Listar todos os usuários |
| GET | `/api/usuarios/{id}/emprestimos-ativos` | Obter quantidade de empréstimos ativos |

### Empréstimos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/emprestimos` | Registrar novo empréstimo |
| POST | `/api/emprestimos/{id}/devolucao` | Registrar devolução |
| GET | `/api/emprestimos/{id}` | Obter empréstimo por ID |
| GET | `/api/emprestimos/usuario/{usuarioId}` | Listar empréstimos por usuário |

### Multas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/multas/usuario/{usuarioId}` | Listar multas do usuário |
| POST | `/api/multas/{multaId}/pagar` | Registrar pagamento de multa |
| GET | `/api/multas/usuario/{usuarioId}/pendentes` | Verificar se há multas pendentes |

### Relatórios

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/relatorios/livros-mais-emprestados` | Top 10 livros mais emprestados |
| GET | `/api/relatorios/usuarios-mais-emprestimos` | Top 10 usuários com mais empréstimos |
| GET | `/api/relatorios/emprestimos-atrasados` | Listar empréstimos em atraso |

## Exemplos de Requisições

### Cadastrar Livro

```json
POST /api/livros
{
  "isbn": "978-3-16-148410-0",
  "titulo": "Clean Code",
  "autor": "Robert C. Martin",
  "categoria": 1,
  "status": 0
}
```

### Registrar Empréstimo

```json
POST /api/emprestimos
{
  "isbn": "978-3-16-148410-0",
  "usuarioId": 1
}
```

### Registrar Devolução

```
POST /api/emprestimos/1/devolucao
```

## Como Executar o Projeto

### Pré-requisitos

- .NET SDK 8.0 ou superior

### Passos para Execução

1. Clone o repositório:
```bash
git clone https://github.com/MuriloManhas88/SistemaBiblioteca.git
cd SistemaBiblioteca
```

2. Restaure as dependências:
```bash
cd SistemaBiblioteca
dotnet restore
```

3. Execute o projeto:
```bash
dotnet run
```

4. Acesse a documentação Swagger:
```
https://localhost:5001/swagger
```

## Tratamento de Erros

O sistema implementa exceções personalizadas para diferentes cenários de erro:

- **LivroNaoEncontradoException** - Livro não existe no sistema
- **LivroIndisponivelException** - Livro não está disponível para empréstimo
- **UsuarioNaoEncontradoException** - Usuário não existe no sistema
- **LimiteEmprestimosExcedidoException** - Usuário excedeu limite de 3 empréstimos
- **MultaPendenteException** - Usuário possui multas não pagas
- **EmprestimoNaoEncontradoException** - Empréstimo não existe
- **EmprestimoInvalidoException** - Operação inválida no empréstimo

## Histórico de Desenvolvimento

### Etapa 1 - Modelagem do Domínio
- Criação das entidades: Livro, Usuário, Empréstimo e Multa
- Definição dos enums: Categoria, StatusLivro, TipoUsuario, StatusEmprestimo, StatusMulta
- Configuração do Entity Framework Core com InMemory Database

### Etapa 2 - Implementação das Regras de Negócio
- Implementação dos serviços de Livro e Usuário
- Implementação do serviço de Empréstimo com validações
- Implementação do serviço de Relatórios
- Criação dos controllers da API

### Etapa 3 - Validações e Tratamento de Erros
- Criação de exceções personalizadas
- Implementação do serviço de Multas
- Validação de multas pendentes antes de empréstimos
- Cálculo automático de multas por atraso

### Etapa 4 - Documentação e Entrega
- Atualização completa do README
- Documentação de endpoints e exemplos
- Criação de diagramas de entidades
- Tag de versão final

## Autor

Murilo Manhas - [GitHub](https://github.com/MuriloManhas88)

## Licença

Este projeto foi desenvolvido para fins educacionais.
