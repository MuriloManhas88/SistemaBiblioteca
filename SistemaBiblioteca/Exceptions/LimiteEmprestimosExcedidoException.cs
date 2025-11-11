namespace SistemaBiblioteca.Exceptions
{
    public class LimiteEmprestimosExcedidoException : BibliotecaException
    {
        public LimiteEmprestimosExcedidoException(int usuarioId) 
            : base($"Usuário {usuarioId} já possui 3 empréstimos ativos. Limite excedido.")
        {
        }
    }
}
