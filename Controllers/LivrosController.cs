using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca_LAB.Data;
using Biblioteca_LAB.Models;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;
using System.Globalization;
using NuGet.Versioning;

namespace Biblioteca_LAB.Controllers
{
    [Authorize]
    public class LivrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivrosController(ApplicationDbContext context)
        {
            _context = context;
        }

		[Authorize(Roles = "Bibliotecario, Admin")]
		public IActionResult Create()
		{
			var autores = _context.Autor.ToList();
			var generos = new List<string> { "Ficção", "Aventura", "Romance", "Fantasia", "Ação", "Jogos", "Terror", "Mistério",
									  "Desporto", "Culinária", "Programação" };

			ViewBag.Generos = generos;
			ViewBag.Autores = autores;

			return View();
		}
		// POST: Livros/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Create(Livro livro, string Preco, string NomeAutor, IFormFile? capa)
		{
			// Buscar autores para a ViewBag caso a validação falhe
			var autores = await _context.Autor.ToListAsync();
            ViewBag.Autores = autores ?? new List<Autor>();

            if (_context.Livros.Any(l => l.Isbn == livro.Isbn))
            {
				return RedirectToAction(nameof(Catalogo));
			}
			decimal price;
			try
            {
                price = decimal.Parse(Preco, new CultureInfo("pt-PT"));
			}
            catch (Exception e)
            {
				ModelState.AddModelError("Preco", "O preço introduzido não é válido.");
				return View(livro);
			}

			ModelState.Remove("Preco");
			if (ModelState.IsValid)
			{
				// Processar a capa (se enviada)
				if (capa != null && capa.Length > 0)
				{
					var fileName = Guid.NewGuid().ToString() + Path.GetExtension(capa.FileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "capasLivros", fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await capa.CopyToAsync(stream);
					}

					livro.Capa = fileName;
				}

				Autor autor;
				if (_context.Autor.Any(a => a.Nome == NomeAutor))
				{
					autor = await _context.Autor.Where(a => a.Nome == NomeAutor).FirstOrDefaultAsync();
					if (autor == null)
					{
						ModelState.AddModelError("IdAutor", "O autor selecionado não existe.");
						return View(livro);
					}
				}
				else
				{
					// Criar novo autor
					autor = new Autor
                    { 
						Nome = NomeAutor
					};

					_context.Autor.Add(autor);
					await _context.SaveChangesAsync();
				}

				livro.LivroAutores ??= new List<LivroAutor>();
				livro.LivroAutores.Add(new LivroAutor
				{
					Livro = livro,
					Autor = autor,
					AutorId = autor.IdAutor,
					LivroIsbn = livro.Isbn
				});

                livro.Preco = price;
				_context.Livros.Add(livro);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Catalogo));
			}

			return View(livro);
		}


		public IActionResult Catalogo(string search, string genero, string preco, string autor, int page = 1)
        {
            int pageSize = 5; // Número de livros por página

            // Consulta básica para buscar os livros
            var livros = _context.Livros.AsQueryable();

            // Filtro por busca
            if (!string.IsNullOrEmpty(search))
            {
                livros = livros.Where(l => l.Titulo.Contains(search));
            }

            // Filtro por género
            if (!string.IsNullOrEmpty(genero))
            {
                livros = livros.Where(l => l.Genero == genero);
            }

            // Filtro por autor
            if (!string.IsNullOrEmpty(autor))
            {
                livros = livros.Where(l => l.LivroAutores.Any(la => la.Autor.Nome == autor));
            }

			// Filtro por preço
			if (!string.IsNullOrEmpty(preco))
			{
				if (preco == "0-10")
				{
					livros = livros.Where(l => l.Preco <= 10);
				}
				else if (preco == "10-20")
				{
					livros = livros.Where(l => l.Preco > 10 && l.Preco <= 20); 
				}
				else if (preco == "20+")
				{
					livros = livros.Where(l => l.Preco > 20); 
				}
			}

			livros = livros
                .GroupBy(l => l.Titulo)
                .Select(group => group.First());

            // Paginação
            var totalItems = livros.Count();
            var livrosPaginados = livros
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Lista de géneros
            var generos = new List<string> { "Ficção", "Aventura", "Romance", "Fantasia", "Ação", "Jogos", "Terror", "Mistério", 
                                              "Desporto", "Culinária", "Programação", "Crime", "História"};


            var topAutores = _context.Autor
                .Select(a => a.Nome)
                .OrderBy(nome => nome)
                .ToList();



            if (topAutores == null || !topAutores.Any())
            {
                topAutores = new List<string> { "Nenhum autor encontrado" };
            }

            ViewBag.AutoresPopulares = topAutores;
            ViewBag.Generos = generos;
            ViewBag.Autores = topAutores;
            ViewBag.Search = search;
            ViewBag.Genero = genero;
            ViewBag.Autor = autor;
            ViewBag.Page = page;
			ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(livrosPaginados);
        }

        public IActionResult Detalhes(string id)
        {
            var livro = _context.Livros
                .Include(l => l.LivroAutores) 
                .ThenInclude(la => la.Autor)
                .FirstOrDefault(l => l.Isbn == id);

            if (livro == null)
            {
                return NotFound();
            }

            ViewBag.IsEmprestado = _context.Requisita.Any(r => r.IsbnR == id && r.Estado == false);

			ViewBag.NumExemplares = _context.Armazenas
				.Where(a => a.IsbnA == id)
				.Sum(a => a.NumExemplares);

			return View(livro);
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

			// Lista de gêneros e autores
			var autores = _context.Autor.ToList();
			var generos = new List<string> { "Ficção", "Aventura", "Romance", "Fantasia", "Ação", "Jogos", "Terror", "Mistério",
									  "Desporto", "Culinária", "Programação" };

			ViewBag.Generos = generos;
			ViewBag.Autores = autores;

			return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string preco, string NomeAutor, Livro livro, IFormFile? novaCapa)
        {
			var autores = _context.Autor.ToList();
			var generos = new List<string> { "Ficção", "Aventura", "Romance", "Fantasia", "Ação", "Jogos", "Terror", "Mistério",
									  "Desporto", "Culinária", "Programação" };

			ViewBag.Generos = generos;
			ViewBag.Autores = autores;

			if (id != livro.Isbn)
            {
                return NotFound();
            }

			decimal price;
			try
			{
				price = decimal.Parse(preco, new CultureInfo("pt-PT"));
			}
			catch (Exception e)
			{
				ModelState.AddModelError("Preco", "O preço introduzido não é válido.");
				return View(livro);
			}
            livro.Preco = price;

			ModelState.Remove("NomeAutor");
			if (ModelState.IsValid)
            {
                try
                {
                    // Carrega o livro atual da base de dados
                    var livroAtual = await _context.Livros.AsNoTracking().FirstOrDefaultAsync(l => l.Isbn == id);
                    if (livroAtual == null)
                    {
                        return NotFound();
                    }

                    // Se uma nova capa foi enviada, processa-a
                    if (novaCapa != null && novaCapa.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(novaCapa.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "capasLivros", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await novaCapa.CopyToAsync(stream);
                        }

                        // Exclui a capa antiga, se existir
                        if (!string.IsNullOrEmpty(livroAtual.Capa))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", livroAtual.Capa.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Atualiza o campo 'Capa' com o novo caminho
                        livro.Capa = fileName;
                    }
                    else
                    {
                        // Mantém a capa existente
                        livro.Capa = livroAtual.Capa;
                    }

					if (NomeAutor != null)
                    {
						Autor au;
						if (_context.Autor.Any(a => a.Nome == NomeAutor))
						{
							au = await _context.Autor.Where(a => a.Nome == NomeAutor).FirstOrDefaultAsync();
							if (au == null)
							{
								ModelState.AddModelError("IdAutor", "O autor selecionado não existe.");
								return View(livro);
							}
						}
						else
						{
							au = new Autor
							{
								Nome = NomeAutor
							};

							_context.Autor.Add(au);
							await _context.SaveChangesAsync();
						}

						livro.LivroAutores ??= new List<LivroAutor>();
						livro.LivroAutores.Add(new LivroAutor
						{
							Livro = livro,
							Autor = au,
							AutorId = au.IdAutor,
							LivroIsbn = livro.Isbn
						});
					}
                    else
                    {
						livro.LivroAutores = livroAtual.LivroAutores;
					}

					livro.DataSubmissao = DateTime.Now;

					// Atualiza o banco de dados
					_context.Entry(livro).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Isbn))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Catalogo));
            }
            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                var la = _context.LivroAutores.Where(la => la.LivroIsbn == id);
                var req = _context.Requisita.Where(r => r.IsbnR == id);
                _context.Requisita.RemoveRange(req);
				_context.LivroAutores.RemoveRange(la);
				_context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Catalogo));
        }

        private bool LivroExists(string id)
        {
            return _context.Livros.Any(e => e.Isbn == id);
        }
    }
}
