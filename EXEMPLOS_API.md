# Exemplos de Uso da API - Sistema de Biblioteca

## Cadastrar Livro

```bash
POST /api/livros
Content-Type: application/json

{
  "isbn": "978-3-16-148410-0",
  "titulo": "Clean Code",
  "autor": "Robert C. Martin",
  "categoria": 1,
  "status": 0
}
```

## Cadastrar Usuário

```bash
POST /api/usuarios
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@email.com",
  "tipo": 0
}
```

## Registrar Empréstimo

```bash
POST /api/emprestimos
Content-Type: application/json

{
  "isbn": "978-3-16-148410-0",
  "usuarioId": 1
}
```

**Validações aplicadas:**
- Verifica se o livro existe e está disponível
- Verifica se o usuário existe
- Verifica se o usuário não excedeu o limite de 3 empréstimos ativos
- Verifica se o usuário não possui multas pendentes
- Calcula prazo de devolução: 30 dias para professores, 14 dias para alunos

## Registrar Devolução

```bash
POST /api/emprestimos/1/devolucao
```

**Validações aplicadas:**
- Verifica se o empréstimo existe e está ativo
- Calcula multa automaticamente se houver atraso (R$ 1,00 por dia)
- Atualiza status do livro para disponível

## Listar Empréstimos em Atraso

```bash
GET /api/relatorios/emprestimos-atrasados
```

## Verificar Multas Pendentes

```bash
GET /api/multas/usuario/1/pendentes
```

## Pagar Multa

```bash
POST /api/multas/5/pagar
```

## Cenários de Validação Testados

### 1. Tentativa de emprestar livro já emprestado
**Erro:** `LivroIndisponivelException`
**Mensagem:** "Livro com ISBN XXX não está disponível para empréstimo."

### 2. Usuário com mais de 3 empréstimos ativos
**Erro:** `LimiteEmprestimosExcedidoException`
**Mensagem:** "Usuário X já possui 3 empréstimos ativos. Limite excedido."

### 3. Tentativa de devolução sem empréstimo ativo
**Erro:** `EmprestimoInvalidoException`
**Mensagem:** "Empréstimo não está ativo."

### 4. Bloqueio de empréstimo para usuários com multas pendentes
**Erro:** `MultaPendenteException`
**Mensagem:** "Usuário X possui multas pendentes e não pode realizar novos empréstimos."

### 5. Cálculo automático de multa por atraso
- Multa calculada automaticamente na devolução
- Valor: R$ 1,00 por dia de atraso
- Status do empréstimo atualizado para ATRASADO

### 6. Atualização correta do status do livro
- DISPONIVEL → EMPRESTADO (ao registrar empréstimo)
- EMPRESTADO → DISPONIVEL (ao registrar devolução)
