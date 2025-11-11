namespace SistemaBiblioteca.Exceptions
{
    public class LivroIndisponivelException : BibliotecaException
    {
        public LivroIndisponivelException(string isbn) 
            : base($"Livro com ISBN {isbn} não está disponível para empréstimo.")
        {
        }
    }
}
