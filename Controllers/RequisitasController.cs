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
using System.Net.WebSockets;
using SQLitePCL;
using System.Runtime.CompilerServices;

namespace Biblioteca_LAB.Controllers
{
    [Authorize]
    public class RequisitasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequisitasController(ApplicationDbContext context)
        {
            _context = context;
        }

		// GET: Requisitas
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Index()
        {
            var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation);

			foreach (var req in requisitas)
			{
				if (req.DataEntregaR == null && (DateTime.Now - req.DataRequisicao).Days > 14)
				{
					req.ValorMulta = (DateTime.Now - req.DataRequisicao).Days * 1.00m;
				}
			}

            _context.UpdateRange(requisitas);
			await _context.SaveChangesAsync();

			return View(await requisitas.ToListAsync());
    }

        public async Task<IActionResult> EmpPerfil(string id)
        {
            var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation).Where(r => r.IdLeitorR == id);

			foreach (var req in requisitas)
			{
				if (req.DataEntregaR == null && (DateTime.Now - req.DataRequisicao).Days > 14)
				{
					req.ValorMulta = (DateTime.Now - req.DataRequisicao).Days * 1.00m;
				}
			}

			_context.UpdateRange(requisitas);
			return View(await requisitas.ToListAsync());
		}


		public IActionResult NaoEntreguePartial(string id)
		{
			if (id == null)
			{
				var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation).Where(r => r.Estado == true);
				return PartialView("_ListaEmpPartial", requisitas);
			}
			else
            {
				var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation).Where(r => r.IdLeitorR == id && r.Estado == true);
				return PartialView("_ListaEmpPerfilPartial", requisitas);
			}
		}

		public IActionResult EntreguePartial(string id)
		{
			if (id == null)
			{
				var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation).Where(r => r.Estado == false);
				return PartialView("_ListaEmpPartial", requisitas);
			}
			else
			{
				var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation).Where(r => r.IdLeitorR == id && r.Estado == false);
				return PartialView("_ListaEmpPerfilPartial", requisitas);
			}
		}

		public async Task<IActionResult> Eliminar(string idLeitor, string isbn)
		{
			var requisicao = _context.Requisita.FirstOrDefault(r => r.IdLeitorR == idLeitor && r.IsbnR == isbn);

            _context.Armazenas.Find(requisicao.IsbnR, "BibliotecaUTAD").NumExemplares += 1;

			_context.Requisita.Remove(requisicao);
			await _context.SaveChangesAsync();


			
            return RedirectToAction("EmpPerfil");
		}

		// GET: Requisitas/Details/5
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisita = await _context.Requisita
                .Include(r => r.IdLeitorRNavigation)
                .Include(r => r.IsbnRNavigation)
                .FirstOrDefaultAsync(m => m.IsbnR == id);
            if (requisita == null)
            {
                return NotFound();
            }

            return View(requisita);
        }

		// GET: Requisitas/Create
		[Authorize(Roles = "Bibliotecario, Admin")]
		public IActionResult Create(string isbn)
        {
			RequisitaViewModel req = new RequisitaViewModel();
			req.DataRequisicao = DateTime.Now;
			req.IsbnR = isbn;
			ViewData["IdLeitorR"] = new SelectList(_context.Utilizadores, "Email", "Id");
			return View(req);
        }

        // POST: Requisitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Create([Bind("IsbnR,EmailUtilizador,DataEntregaR,DataRequisicao,ValorMulta")] RequisitaViewModel requisitaVM)
        {
            Requisita requisita = new Requisita();

			if (ModelState.IsValid)
            {
				requisita.IsbnR = requisitaVM.IsbnR;
				requisita.IdLeitorR = _context.Utilizadores.FirstOrDefault(u => u.Email == requisitaVM.EmailUtilizador).Id;
				requisita.DataRequisicao = requisitaVM.DataRequisicao;
				requisita.DataEntregaR = null;
                requisita.ValorMulta = 0;
                _context.Add(requisita);

				_context.Armazenas.Find(requisita.IsbnR, "BibliotecaUTAD").NumExemplares -= 1;


				await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


			return View(requisitaVM);
        }


		// GET: Requisitas/Edit/5
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisita = await _context.Requisita.FindAsync(id);
            if (requisita == null)
            {
                return NotFound();
            }
            ViewData["IdLeitorR"] = new SelectList(_context.Utilizadores, "Id", "Id", requisita.IdLeitorR);
            ViewData["IsbnR"] = new SelectList(_context.Livros, "Isbn", "Isbn", requisita.IsbnR);
            return View(requisita);
        }

        // POST: Requisitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Edit(string id, [Bind("IsbnR,IdLeitorR,DataEntregaR,DataRequisicao,ValorMulta")] Requisita requisita)
        {
            if (id != requisita.IsbnR)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requisita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisitaExists(requisita.IsbnR))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLeitorR"] = new SelectList(_context.Utilizadores, "Id", "Id", requisita.IdLeitorR);
            ViewData["IsbnR"] = new SelectList(_context.Livros, "Isbn", "Isbn", requisita.IsbnR);
            return View(requisita);
        }

		// GET: Requisitas/Delete/5
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisita = await _context.Requisita
                .Include(r => r.IdLeitorRNavigation)
                .Include(r => r.IsbnRNavigation)
                .FirstOrDefaultAsync(m => m.IsbnR == id);
            if (requisita == null)
            {
                return NotFound();
            }

            return View(requisita);
        }

        // POST: Requisitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Bibliotecario, Admin")]
		public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var requisita = await _context.Requisita.FindAsync(id);
            if (requisita != null)
            {
                _context.Requisita.Remove(requisita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequisitaExists(string id)
        {
            return _context.Requisita.Any(e => e.IsbnR == id);
        }


		[Authorize(Roles = "Bibliotecario, Admin")]
		public IActionResult Entregar(string idLeitor, string isbn)
        {
            var requisicao = _context.Requisita
                .FirstOrDefault(r => r.IdLeitorR == idLeitor && r.IsbnR == isbn);

			if (requisicao != null)
            {
                requisicao.Estado = true;
                requisicao.DataEntregaR = DateTime.Now;
                requisicao.ValorMulta = 0;

				_context.Armazenas.Find(requisicao.IsbnR, "BibliotecaUTAD").NumExemplares += 1;

				_context.SaveChanges(); 
            }

			var requisitas = _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation);
			return View(nameof(Index), requisitas.ToList());
		}

		public async Task<IActionResult> CancelarEntrega(string idLeitor, string isbn)
        {
            var requisicao = _context.Requisita
                .FirstOrDefault(r => r.IdLeitorR == idLeitor && r.IsbnR == isbn);
            if (requisicao != null)
            {
                requisicao.ValorMulta = null;
                requisicao.Estado = false; 
                requisicao.DataEntregaR = null; 

				_context.Armazenas.Find(requisicao.IsbnR, "BibliotecaUTAD").NumExemplares -= 1;

				_context.SaveChanges();
			}

            var requisitas = await _context.Requisita.Include(r => r.IdLeitorRNavigation).Include(r => r.IsbnRNavigation).ToListAsync();
			return RedirectToAction(nameof(Index), requisitas);
		}

		[HttpGet]
		[Authorize(Roles = "Bibliotecario, Admin")]
		public IActionResult Notificacoes()
        {
            var requisicoesAtrasadas = _context.Requisita
                .Where(r => r.Estado == false) // Filtrar apenas os que não foram entregues
                .Include(r => r.IdLeitorRNavigation) // Incluir o leitor
                .Include(r => r.IsbnRNavigation) // Incluir o livro
                .AsEnumerable() // Executar o restante no cliente
                .Where(r => (DateTime.Now - r.DataRequisicao).Days > 14) // Calcular dias no cliente
                .ToList();

            return View(requisicoesAtrasadas);
        }


    }
}
