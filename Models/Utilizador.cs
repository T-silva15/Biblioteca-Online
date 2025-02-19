using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Models;

[Table("Utilizador")]
public partial class Utilizador
{
	[StringLength(30, ErrorMessage = "O nome de utilizador deve ter no máximo 30 caracteres.")]
	[Required(ErrorMessage = "O nome de utilizador é obrigatório.")]
	[Unicode(false)]
	public string Username { get; set; } = null!;

	[StringLength(40, ErrorMessage = "O email deve ter no máximo 40 caracteres.")]
	[Required(ErrorMessage = "O email é obrigatório.")]
	[EmailAddress(ErrorMessage = "Insira um email válido.")]
	[Unicode(false)]
	public string Email { get; set; } = null!;

	[StringLength(40, ErrorMessage = "O nome deve ter no máximo 40 caracteres.")]
	[Required(ErrorMessage = "O nome é obrigatório.")]
	[Unicode(false)]
	public string Nome { get; set; } = null!;

	[StringLength(100, ErrorMessage = "A palavra-passe deve ter no máximo 100 caracteres.")]
	[Required(ErrorMessage = "A palavra-passe é obrigatória.")]
	[DataType(DataType.Password)]
	[Unicode(false)]
	public string? Password { get; set; } = null!;

	[Key]
	[StringLength(36, ErrorMessage = "O ID deve ter no máximo 36 caracteres.")]
	[Required(ErrorMessage = "O ID é obrigatório.")]
	[Unicode(false)]
	public string Id { get; set; }

	[Phone(ErrorMessage = "Insira um número de telefone válido.")]
	[StringLength(9, ErrorMessage = "O contacto deve ter no máximo 9 caracteres.")]
	[Unicode(false)]
	public string? Contacto { get; set; }

	[StringLength(30, ErrorMessage = "A cidade deve ter no máximo 30 caracteres.")]
	[Unicode(false)]
	public string? Cidade { get; set; }

	[StringLength(30, ErrorMessage = "O endereço deve ter no máximo 30 caracteres.")]
	[Unicode(false)]
	public string? Endereco { get; set; }

	[Column("Codigo_Postal")]
	[DataType(DataType.PostalCode)]
	[StringLength(8, ErrorMessage = "O código postal deve ter no máximo 8 caracteres.")]
	[Unicode(false)]
	public string? CodigoPostal { get; set; }

	[StringLength(15, ErrorMessage = "O tipo de utilizador deve ter no máximo 15 caracteres.")]
	[Required(ErrorMessage = "O tipo de utilizador é obrigatório.")]
	[Unicode(false)]
	public string Tipo { get; set; } = null!;

	[Required]
	public bool Block { get; set; } = false;

	public DateTime? DataBlock { get; set; }

	public DateTime DataSub { get; set; } = DateTime.Now;

	[InverseProperty("IdBibGlNavigation")]
	public virtual ICollection<GereLivro> GereLivros { get; set; } = new List<GereLivro>();

	[InverseProperty("IdLeitorRNavigation")]
	public virtual ICollection<Requisita> Requisita { get; set; } = new List<Requisita>();

	public static string GenerateId()
	{
		return Guid.NewGuid().ToString();
	}
}
