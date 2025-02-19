using System.Diagnostics;
using System.Security.Claims;
using Biblioteca_LAB.Data;
using Biblioteca_LAB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_LAB.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			var numUsers = _context.Utilizadores.Count();
			var numLivros = _context.Livros.Count();

            var randomLivros = _context.Livros
				.OrderBy(x => Guid.NewGuid())
				.Take(6)
				.AsQueryable();

            ViewBag.NumUsers = numUsers;
			ViewBag.NumLivros = numLivros;
			return View(randomLivros.ToList());
		}

		[AllowAnonymous]
		public IActionResult Block(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var description = _context.AlteracoesAdm
				.Where(g => g.IdUtilizador == id)
				.OrderByDescending(g => g.DataGa)
				.Select(g => g.Descricao)         
				.FirstOrDefault();                

			var user = _context.Utilizadores.Find(id);

			if (user == null)
			{
				return NotFound();
			}

			ViewData["msg"] = description;
			return View(user);
		}

		[AllowAnonymous]
		public IActionResult Informacoes()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
