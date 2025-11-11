namespace SistemaBiblioteca.Exceptions
{
    public class LivroNaoEncontradoException : BibliotecaException
    {
        public LivroNaoEncontradoException(string isbn) 
            : base($"Livro com ISBN {isbn} não encontrado.")
        {
        }
    }
}
