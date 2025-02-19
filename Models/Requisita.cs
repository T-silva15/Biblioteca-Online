using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Models;

[PrimaryKey("IsbnR", "IdLeitorR", "DataRequisicao")]
public partial class Requisita
{
    [Key]
    [Column("ISBN_R")]
    [StringLength(13)]
    [Unicode(false)]
	[Display(Name = "ISBN do Livro")]
	public string IsbnR { get; set; } = null!;

    [Key]
    [Column("Id_Leitor_R")]
    [StringLength(36)]
    [Unicode(false)]
	[Display(Name = "ID do Leitor")]
	public string IdLeitorR { get; set; }

    [Column("Data_Entrega_R", TypeName = "smalldatetime")]
	[Display(Name = "Data de Entrega")]
	public DateTime? DataEntregaR { get; set; }

	[Column("Estado")]
	[Display(Name = "Estado da Requisição")]
	public bool Estado { get; set; }

    [Key]
    [Column("Data_Requisicao", TypeName = "smalldatetime")]
	[Display(Name = "Data de Requisição")]
	public DateTime DataRequisicao { get; set; } = DateTime.Now;

	[Column("Valor_Multa", TypeName = "money")]
	[Display(Name = "Multa")]
	public decimal? ValorMulta { get; set; }

    [ForeignKey("IdLeitorR")]
    [InverseProperty("Requisita")]
    public virtual Utilizador IdLeitorRNavigation { get; set; } = null!;

    [ForeignKey("IsbnR")]
    [InverseProperty("Requisita")]
    public virtual Livro IsbnRNavigation { get; set; } = null!;
}
