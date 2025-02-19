using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca_LAB.Models
{
    [Table("LivroAutor")] // Nome da tabela intermediária
    public class LivroAutor
    {
        [ForeignKey("Livro")]
        public string LivroIsbn { get; set; } = null!; // Chave estrangeira para 'Livro'
        public Livro Livro { get; set; } = null!;      // Navegação para 'Livro'

        [ForeignKey("Autor")]
        public string AutorId { get; set; } = null!;         // Chave estrangeira para 'Autor'
        public Autor Autor { get; set; } = null!;     // Navegação para 'Autor'
    }
}
