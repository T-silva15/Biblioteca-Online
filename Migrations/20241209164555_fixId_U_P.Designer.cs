﻿// <auto-generated />
using System;
using Biblioteca_LAB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Biblioteca_LAB.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241209164555_fixId_U_P")]
    partial class fixId_U_P
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Biblioteca_LAB.Models.Armazena", b =>
                {
                    b.Property<string>("IsbnA")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("ISBN_A");

                    b.Property<string>("NomeA")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Nome_A");

                    b.Property<int>("NumExemplares")
                        .HasColumnType("int")
                        .HasColumnName("Num_Exemplares");

                    b.HasKey("IsbnA", "NomeA");

                    b.HasIndex("NomeA");

                    b.ToTable("Armazena");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Autor", b =>
                {
                    b.Property<string>("IdAutor")
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("Id_Autor");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("IdAutor");

                    b.ToTable("AUTOR");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Biblioteca", b =>
                {
                    b.Property<string>("Nome")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<TimeOnly>("HorarioAbertura")
                        .HasColumnType("time")
                        .HasColumnName("Horario_Abertura");

                    b.Property<TimeOnly>("HorarioEncerramento")
                        .HasColumnType("time")
                        .HasColumnName("Horario_Encerramento");

                    b.Property<string>("LocCidade")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("Loc_Cidade");

                    b.Property<string>("LocCp")
                        .IsRequired()
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("Loc_CP");

                    b.Property<string>("LocEnd")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("Loc_End");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(9)
                        .IsUnicode(false)
                        .HasColumnType("varchar(9)");

                    b.HasKey("Nome");

                    b.ToTable("Biblioteca");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.GereLivro", b =>
                {
                    b.Property<string>("IsbnGl")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("ISBN_GL");

                    b.Property<string>("IdBibGl")
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("Id_Bib_GL");

                    b.Property<DateTime>("DataGl")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("Data_GL");

                    b.Property<string>("TipoAlteracao")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("Tipo_Alteracao");

                    b.HasKey("IsbnGl", "IdBibGl", "DataGl");

                    b.HasIndex("IdBibGl");

                    b.ToTable("Gere_Livro");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Livro", b =>
                {
                    b.Property<string>("Isbn")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("ISBN");

                    b.Property<string>("Capa")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Edicao")
                        .HasColumnType("int");

                    b.Property<string>("Genero")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("money");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.HasKey("Isbn");

                    b.ToTable("Livro");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.LivroAutor", b =>
                {
                    b.Property<string>("LivroIsbn")
                        .HasColumnType("varchar(13)");

                    b.Property<string>("AutorId")
                        .HasColumnType("varchar(32)");

                    b.HasKey("LivroIsbn", "AutorId");

                    b.HasIndex("AutorId");

                    b.ToTable("LivroAutor");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Requisitum", b =>
                {
                    b.Property<string>("IsbnR")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("ISBN_R");

                    b.Property<string>("IdLeitorR")
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("Id_Leitor_R");

                    b.Property<DateTime>("DataRequisicao")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("Data_Requisicao");

                    b.Property<DateTime?>("DataEntregaR")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("Data_Entrega_R");

                    b.Property<decimal?>("ValorMulta")
                        .HasColumnType("money")
                        .HasColumnName("Valor_Multa");

                    b.HasKey("IsbnR", "IdLeitorR", "DataRequisicao");

                    b.HasIndex("IdLeitorR");

                    b.ToTable("Requisitum");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Utilizador", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Cidade")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("CodigoPostal")
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("Codigo_Postal");

                    b.Property<string>("Contacto")
                        .HasMaxLength(9)
                        .IsUnicode(false)
                        .HasColumnType("varchar(9)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Endereco")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Utilizador");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.UtilizadorPendente", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("AdminAprov")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasColumnName("AdminAprov");

                    b.Property<string>("Cidade")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("CodigoPostal")
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("Codigo_Postal");

                    b.Property<string>("Contacto")
                        .HasMaxLength(9)
                        .IsUnicode(false)
                        .HasColumnType("varchar(9)");

                    b.Property<DateTime?>("DataRes")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataResultado");

                    b.Property<DateTime>("DataSub")
                        .HasColumnType("datetime2")
                        .HasColumnName("DataSubmissao");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Endereco")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("Estado")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("Resultado")
                        .HasColumnType("int")
                        .HasColumnName("Resultado");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("UtilizadorPendente");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Armazena", b =>
                {
                    b.HasOne("Biblioteca_LAB.Models.Livro", "IsbnANavigation")
                        .WithMany("Armazenas")
                        .HasForeignKey("IsbnA")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca_LAB.Models.Biblioteca", "NomeANavigation")
                        .WithMany("Armazenas")
                        .HasForeignKey("NomeA")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IsbnANavigation");

                    b.Navigation("NomeANavigation");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.GereLivro", b =>
                {
                    b.HasOne("Biblioteca_LAB.Models.Utilizador", "IdBibGlNavigation")
                        .WithMany("GereLivros")
                        .HasForeignKey("IdBibGl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca_LAB.Models.Livro", "IsbnGlNavigation")
                        .WithMany("GereLivros")
                        .HasForeignKey("IsbnGl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdBibGlNavigation");

                    b.Navigation("IsbnGlNavigation");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.LivroAutor", b =>
                {
                    b.HasOne("Biblioteca_LAB.Models.Autor", "Autor")
                        .WithMany("LivroAutores")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca_LAB.Models.Livro", "Livro")
                        .WithMany("LivroAutores")
                        .HasForeignKey("LivroIsbn")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Requisitum", b =>
                {
                    b.HasOne("Biblioteca_LAB.Models.Utilizador", "IdLeitorRNavigation")
                        .WithMany("Requisita")
                        .HasForeignKey("IdLeitorR")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca_LAB.Models.Livro", "IsbnRNavigation")
                        .WithMany("Requisita")
                        .HasForeignKey("IsbnR")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdLeitorRNavigation");

                    b.Navigation("IsbnRNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Autor", b =>
                {
                    b.Navigation("LivroAutores");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Biblioteca", b =>
                {
                    b.Navigation("Armazenas");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Livro", b =>
                {
                    b.Navigation("Armazenas");

                    b.Navigation("GereLivros");

                    b.Navigation("LivroAutores");

                    b.Navigation("Requisita");
                });

            modelBuilder.Entity("Biblioteca_LAB.Models.Utilizador", b =>
                {
                    b.Navigation("GereLivros");

                    b.Navigation("Requisita");
                });
#pragma warning restore 612, 618
        }
    }
}
