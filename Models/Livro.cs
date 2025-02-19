using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Biblioteca_LAB.Models;

[Table("Livro")]
public partial class Livro
{
    [Key]
    [Column("ISBN")]
    [StringLength(13)]
    [Unicode(false)]
	[Required(ErrorMessage = "O ISBN é obrigatório.")]
	[RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$", ErrorMessage = "Introduza um ISBN válido.")]
	public string Isbn { get; set; } = null!;

    [StringLength(40)]
    [Unicode(false)]
	[Required(ErrorMessage = "O título é obrigatório.")]
	public string Titulo { get; set; } = null!;

    [Range(1, 9999, ErrorMessage = "Introduza um número válido para a edição")]
	[Required(ErrorMessage = "A edição é obrigatória.")]
	public int Edicao { get; set; }

	[Column(TypeName = "money")]
	[Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
	[Required(ErrorMessage = "O preço é obrigatório.")]
	[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
	[DataType(DataType.Currency)]
	public decimal Preco { get; set; }

	[StringLength(20, ErrorMessage = "O género deve ter no máximo 20 caracteres.")]
	[Required(ErrorMessage = "O género é obrigatório.")]
	[Unicode(false)]
    public string Genero { get; set; } = null!;

	[StringLength(100, ErrorMessage = "O nome da capa deve ter no máximo 100 caracteres.")]
	[Unicode(false)]
    public string? Capa { get; set; }

    [Unicode(false)]
    [Column("DataSubmissao")]
	public DateTime? DataSubmissao { get; set; } = DateTime.Now;

	[InverseProperty("IsbnANavigation")]
    public virtual ICollection<Armazena> Armazenas { get; set; } = new List<Armazena>();

    [InverseProperty("IsbnGlNavigation")]
    public virtual ICollection<GereLivro> GereLivros { get; set; } = new List<GereLivro>();

    [InverseProperty("IsbnRNavigation")]
    public virtual ICollection<Requisita> Requisita { get; set; } = new List<Requisita>();

    // Corrigido: Usando 'Isbn' como chave estrangeira em vez de 'IsbnLa'
    [ForeignKey("LivroIsbn")]  // A chave estrangeira deve se referir à propriedade 'LivroIsbn' na tabela intermediária
    public ICollection<LivroAutor> LivroAutores { get; set; } = new List<LivroAutor>();
}