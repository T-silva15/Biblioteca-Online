using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models;

[Table("AUTOR")]
public partial class Autor
{
	[Key]
	[Column("Id_Autor")]
	[StringLength(36, ErrorMessage = "O ID do autor deve ter no máximo 36 caracteres.")]
	[Required(ErrorMessage = "O ID do autor é obrigatório.")]
	[Unicode(false)]
	public string IdAutor { get; set; } = Utilizador.GenerateId();

	[StringLength(30, ErrorMessage = "O nome do autor deve ter no máximo 30 caracteres.")]
	[Required(ErrorMessage = "O nome do autor é obrigatório.")]
	[Unicode(false)]
	public string Nome { get; set; } = null!;

	// Relacionamento com a tabela intermediária LivroAutor
	[InverseProperty("Autor")]
	public ICollection<LivroAutor> LivroAutores { get; set; } = new List<LivroAutor>();
}
