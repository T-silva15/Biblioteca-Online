using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca_LAB.Models;

public class RequisitaViewModel
{
	[StringLength(13)]
	[Unicode(false)]
	[Display(Name = "ISBN do Livro")]
	public string IsbnR { get; set; } = null!;

	public DateTime? DataEntregaR { get; set; }

	[Display(Name = "Data de Requisição")]
	public DateTime DataRequisicao { get; set; } = DateTime.Now;

	[Display(Name = "Multa")]
	public decimal? ValorMulta { get; set; }

	[StringLength(40)]
	[Unicode(false)]
	[Display(Name = "Email do Utilizador")]
	public string EmailUtilizador { get; set; } 
}

