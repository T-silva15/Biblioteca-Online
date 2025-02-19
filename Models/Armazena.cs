using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Models;

[PrimaryKey("IsbnA", "NomeA")]
[Table("Armazena")]
public partial class Armazena
{
    [Key]
    [Column("ISBN_A")]
    [StringLength(13)]
    [Unicode(false)]
    public string IsbnA { get; set; } = null!;

    [Key]
    [Column("Nome_A")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeA { get; set; } = null!;

    [Column("Num_Exemplares")]
    public int NumExemplares { get; set; }

    [ForeignKey("IsbnA")]
    [InverseProperty("Armazenas")]
    public virtual Livro IsbnANavigation { get; set; } = null!;

    [ForeignKey("NomeA")]
    [InverseProperty("Armazenas")]
    public virtual Biblioteca NomeANavigation { get; set; } = null!;
}
