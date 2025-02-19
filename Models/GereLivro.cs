using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Models;

[PrimaryKey("IsbnGl", "IdBibGl", "DataGl")]
[Table("Gere_Livro")]
public partial class GereLivro
{
    [Key]
    [Column("ISBN_GL")]
    [StringLength(13)]
    [Unicode(false)]
    public string IsbnGl { get; set; } = null!;

    [Key]
    [Column("Id_Bib_GL")]
    [StringLength(36)]
    [Unicode(false)]
    public string IdBibGl { get; set; }

    [Key]
    [Column("Data_GL", TypeName = "smalldatetime")]
    public DateTime DataGl { get; set; }

    [Column("Tipo_Alteracao")]
    [StringLength(30)]
    [Unicode(false)]
    public string TipoAlteracao { get; set; } = null!;

    [ForeignKey("IdBibGl")]
    [InverseProperty("GereLivros")]
    public virtual Utilizador IdBibGlNavigation { get; set; } = null!;

    [ForeignKey("IsbnGl")]
    [InverseProperty("GereLivros")]
    public virtual Livro IsbnGlNavigation { get; set; } = null!;
}
