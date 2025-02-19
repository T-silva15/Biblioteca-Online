using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Models;

[Table("Biblioteca")]
public partial class Biblioteca
{
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("Loc_End")]
    [StringLength(30)]
    [Unicode(false)]
    public string LocEnd { get; set; } = null!;

    [Column("Loc_Cidade")]
    [StringLength(30)]
    [Unicode(false)]
    public string LocCidade { get; set; } = null!;

    [Column("Loc_CP")]
    [StringLength(8)]
    [Unicode(false)]
    public string LocCp { get; set; } = null!;

    [StringLength(40)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(9)]
    [Unicode(false)]
    public string Telefone { get; set; } = null!;

    [Column("Horario_Abertura")]
    public TimeOnly HorarioAbertura { get; set; }

    [Column("Horario_Encerramento")]
    public TimeOnly HorarioEncerramento { get; set; }

    [InverseProperty("NomeANavigation")]
    public virtual ICollection<Armazena> Armazenas { get; set; } = new List<Armazena>();
}
