namespace SistemaBiblioteca.Exceptions
{
    public class EmprestimoNaoEncontradoException : BibliotecaException
    {
        public EmprestimoNaoEncontradoException(int id) 
            : base($"Empréstimo com ID {id} não encontrado.")
        {
        }
    }
}
