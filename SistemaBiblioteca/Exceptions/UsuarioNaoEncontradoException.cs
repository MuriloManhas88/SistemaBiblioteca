namespace SistemaBiblioteca.Exceptions
{
    public class UsuarioNaoEncontradoException : BibliotecaException
    {
        public UsuarioNaoEncontradoException(int id) 
            : base($"Usuário com ID {id} não encontrado.")
        {
        }
    }
}
