using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Models;

[PrimaryKey("IdUtilizador", "IdAdmin", "DataGa")]
[Table("Gere_Adm")]
public partial class GereAdm
{
    [Key]
    [Column("Id_Utilizador")]
    [StringLength(36)]
    [Unicode(false)]
    public string IdUtilizador { get; set; }

    [Key]
    [Column("Id_Admin")]
    [StringLength(36)]
    [Unicode(false)]
    public string IdAdmin { get; set; }

    [Key]
    [Column("Data_GA", TypeName = "smalldatetime")]
    public DateTime DataGa { get; set; }

    [Column("Tipo_Alteracao")]
    [StringLength(30)]
    [Unicode(false)]
    public string TipoAlteracao { get; set; } = null!;

	[Column("Descricao")]
	[StringLength(255)]
	[Unicode(false)]
	public string? Descricao { get; set; } = "Bloqueado por Admnistrador";

}
