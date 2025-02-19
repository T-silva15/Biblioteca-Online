using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca_LAB.Data;
using Biblioteca_LAB.Models;
using Microsoft.AspNetCore.Identity;
using LABW_Biblioteca2024.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Biblioteca_LAB.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca_LAB.Controllers
{
	[Authorize(Roles = "Admin")]
	public class BackofficeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public BackofficeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, 
			SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> List()
		{
			return View(_context.Utilizadores.Where(x => x.Username != User.Identity.Name).ToList());
		}

		public IActionResult UserListPartial()
		{
			return PartialView("_UserListPartial", _context.Utilizadores.Where(x => x.Username != User.Identity.Name).ToList());
		}

		public IActionResult SearchUsers(string searchString)
		{

			if (string.IsNullOrEmpty(searchString))
			{
				return PartialView("_UserListPartial", _context.Utilizadores.Where(x => x.Username != User.Identity.Name).ToList());
			}
			var users = _context.Utilizadores
				.Where(u => (u.Username.ToLower().Contains(searchString.ToLower()) || u.Nome.ToLower().Contains(searchString.ToLower()))
							&& u.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();

			return PartialView("_UserListPartial", users);
		}

		public IActionResult AprovListPartial()
		{
			var users = _context.Utilizadores.Where(u => u.Block == true && u.DataBlock == null && u.Tipo == "Bibliotecario").ToList();
			return PartialView("_AprovListPartial", users);
		}

		public IActionResult SearchAprov(string searchString)
		{
			if (string.IsNullOrEmpty(searchString))
			{
				return PartialView("_AprovListPartial", _context.Utilizadores.Where(u => u.Block == true && u.DataBlock == null && u.Tipo == "Bibliotecario").ToList());
			}
				return PartialView("_AprovListPartial", _context.Utilizadores.Where(u => u.Username.ToLower().Contains(searchString) ||
			u.Nome.ToLower().Contains(searchString) && u.Block == true && u.DataBlock == null && u.Tipo == "Bibliotecario").ToList());
		}

		private bool UtilizadorExists(string id)
		{
			return _context.Utilizadores.Any(e => e.Id == id);
		}

		public async Task<string> Aprovar(string id)
		{
			var user = await _signInManager.UserManager.FindByIdAsync(id);
			user.LockoutEnabled = false;

			var utilizador = await _context.Utilizadores.FindAsync(id);
			utilizador.Block = false;
			_context.Utilizadores.Update(utilizador);

			var result = await _signInManager.UserManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				return "Erro ao aprovar bibliotecário";
			}


			var alteracao = new GereAdm();
			var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			alteracao.IdUtilizador = id;
			alteracao.IdAdmin = adminId;
			alteracao.DataGa = DateTime.Now;
			alteracao.TipoAlteracao = "Aprovação";
			alteracao.Descricao = "Utilizador aprovado por Administrador";

			_context.AlteracoesAdm.Add(alteracao);
			await _context.SaveChangesAsync();

			return "";
		}


		public async Task<string> EliminarAprov(string id)
		{
			var user = await _signInManager.UserManager.FindByIdAsync(id);

			_context.Remove(await _context.Utilizadores.FindAsync(id));

			var result = await _signInManager.UserManager.DeleteAsync(user);

			if (!result.Succeeded)
			{
				return "Erro ao eliminar bibliotecário";

			}
			var alteracao = new GereAdm();

			alteracao.IdUtilizador = id;
			alteracao.IdAdmin = User.FindFirstValue(ClaimTypes.NameIdentifier);
			alteracao.DataGa = DateTime.Now;
			alteracao.TipoAlteracao = "Eliminação";
			alteracao.Descricao = "Bibliotecário não aprovado por Administrador";

			_context.AlteracoesAdm.Add(alteracao);
			await _context.SaveChangesAsync();

			return "";
		}

		public async Task<string> EliminarUser(string id)
		{
			var user = await _signInManager.UserManager.FindByIdAsync(id);

			_context.Remove(await _context.Utilizadores.FindAsync(id));

			var result = await _signInManager.UserManager.DeleteAsync(user);

			if (!result.Succeeded)
			{
				return "Erro ao eliminar utilizador";

			}
			var alteracao = new GereAdm();

			alteracao.IdUtilizador = id;
			alteracao.IdAdmin = User.FindFirstValue(ClaimTypes.NameIdentifier);
			alteracao.DataGa = DateTime.Now;
			alteracao.TipoAlteracao = "Eliminação";
			alteracao.Descricao = "Utilizador eliminado por Administrador";

			_context.AlteracoesAdm.Add(alteracao);
			await _context.SaveChangesAsync();

			return "";
		}

		public async Task<IActionResult> EditUser(string id)
		{
			var utilizador = await _context.Utilizadores.FindAsync(id);

			ViewData["Roles"] = _roleManager.Roles.OrderByDescending(x => x.NormalizedName).ToList();

			return View(utilizador);
		}

		[HttpPost]
		public async Task<IActionResult> EditUser(string id, [Bind("Password,Username,Email,Nome,Id,Contacto,Cidade,Endereco,CodigoPostal,Tipo,Block,DataBlock")] Utilizador utilizador)
		{
			var utilizadorAntigo = await _context.Utilizadores.FindAsync(id);
			var user = await _userManager.FindByIdAsync(id);


			if (utilizador.Username != null)
			{
				await _signInManager.UserManager.SetUserNameAsync(user, utilizador.Username);
			}

			if (utilizador.Email != null)
			{
				await _signInManager.UserManager.SetEmailAsync(user, utilizador.Email);
				user.EmailConfirmed = true;
			}

			if (utilizador.Password != null)
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				var result = await _userManager.ResetPasswordAsync(user, token, utilizador.Password);
				if (!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, "Erro ao alterar a password do utilizador");
				}

				if (ModelState.IsValid)
				{
					utilizador.Password = user.PasswordHash;
				}
			}
			else
			{
				utilizador.Password = utilizadorAntigo.Password;
			}


			if (utilizadorAntigo.Tipo != utilizador.Tipo)
			{
				var roles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRoleAsync(user, utilizadorAntigo.Tipo);
				await _userManager.AddToRoleAsync(user, utilizador.Tipo);

				if (utilizador.Tipo == "Admin")
				{
					utilizador.Contacto = null;
					utilizador.Cidade = null;
					utilizador.Endereco = null;
					utilizador.CodigoPostal = null;
				}
				if (utilizador.Tipo == "Bibliotecario")
				{
					utilizador.Cidade = null;
					utilizador.Endereco = null;
					utilizador.CodigoPostal = null;
				}
			}

			_context.Entry(utilizadorAntigo).State = EntityState.Detached;


			if (utilizador.Block == true)
			{
				user.LockoutEnabled = true;
				user.LockoutEnd = utilizador.DataBlock;
			}
			else
			{
				user.LockoutEnabled = false;
				user.LockoutEnd = null;
			}

			await _userManager.UpdateAsync(user);

			_context.Utilizadores.Update(utilizador);
			await _context.SaveChangesAsync();

			return RedirectToAction("UserList");
		}

		public async Task<IActionResult> AdminActions()
		{
			var alteracoesAdm = await (from a in _context.AlteracoesAdm
									   join u in _context.Utilizadores on a.IdUtilizador equals u.Id
									   join admin in _context.Utilizadores on a.IdAdmin equals admin.Id
									   orderby a.DataGa descending
									   select new GereAdmViewModel
									   {
										   IdUtilizador = a.IdUtilizador,
										   UtilizadorNome = u.Username,
										   IdAdmin = a.IdAdmin,
										   AdminNome = admin.Username,
										   TipoAlteracao = a.TipoAlteracao,
										   Descricao = a.Descricao,
										   DataGa = a.DataGa
									   }).ToListAsync();

			ViewBag.AlteracoesAdm = alteracoesAdm;

			return View(alteracoesAdm);
		}


		public async Task<IActionResult> SearchAdmActions(string searchString)
		{
			if (string.IsNullOrEmpty(searchString))
			{
				var alteracoesAdmNoString = await (from a in _context.AlteracoesAdm
										   join u in _context.Utilizadores on a.IdUtilizador equals u.Id
										   join admin in _context.Utilizadores on a.IdAdmin equals admin.Id
										   orderby a.DataGa descending
										   select new GereAdmViewModel
										   {
											   IdUtilizador = a.IdUtilizador,
											   UtilizadorNome = u.Username,
											   IdAdmin = a.IdAdmin,
											   AdminNome = admin.Username,
											   TipoAlteracao = a.TipoAlteracao,
											   Descricao = a.Descricao,
											   DataGa = a.DataGa
										   }).ToListAsync();

				ViewBag.AlteracoesAdm = alteracoesAdmNoString;

				return View(alteracoesAdmNoString);
			}

			var alteracoesAdm = await(from a in _context.AlteracoesAdm
									  join u in _context.Utilizadores on a.IdUtilizador equals u.Id
									  join admin in _context.Utilizadores on a.IdAdmin equals admin.Id
									  where u.Username.ToLower().Contains(searchString.ToLower()) || admin.Username.ToLower().Contains(searchString.ToLower())
									  orderby a.DataGa descending
									  select new GereAdmViewModel
									  {
										  IdUtilizador = a.IdUtilizador,
										  UtilizadorNome = u.Username,
										  IdAdmin = a.IdAdmin,
										  AdminNome = admin.Username,
										  TipoAlteracao = a.TipoAlteracao,
										  Descricao = a.Descricao,
										  DataGa = a.DataGa
									  }).ToListAsync();

			ViewBag.AlteracoesAdm = alteracoesAdm;

			return PartialView("_AdminActions",alteracoesAdm);
		}

		public async Task<IActionResult> Block(string id)
		{
			return View(await _context.Utilizadores.FindAsync(id));
		}

		public async Task<IActionResult> Unblock(string id)
		{
			var utilizador = await _context.Utilizadores.FindAsync(id);
			utilizador.Block = false;
			utilizador.DataBlock = null;
			_context.Update(utilizador);

			var alteracao = new GereAdm();

			alteracao.IdUtilizador = id;
			alteracao.IdAdmin = User.FindFirstValue(ClaimTypes.NameIdentifier);
			alteracao.DataGa = DateTime.Now;
			alteracao.TipoAlteracao = "Desbloqueio";
			alteracao.Descricao = "Utilizador desbloqueado por Administrador";

			_context.AlteracoesAdm.Add(alteracao);

			await _context.SaveChangesAsync();

			// Update user lockout in Identity
			var user = await _userManager.FindByIdAsync(utilizador.Id);
			user.LockoutEnabled = false;
			user.LockoutEnd = null;
			await _signInManager.UserManager.UpdateAsync(user);


			return RedirectToAction("List");
		}

		[HttpPost]
		public async Task<IActionResult> Block(string id, DateTime blockDate, string blockDescription)
		{
			var utilizador = _context.Utilizadores.Find(id);
			utilizador.Block = true;
			utilizador.DataBlock = blockDate;

			_context.Update(utilizador);

			var alteracao = new GereAdm();

			alteracao.IdUtilizador = id;
			alteracao.IdAdmin = User.FindFirstValue(ClaimTypes.NameIdentifier);
			alteracao.DataGa = DateTime.Now;
			alteracao.TipoAlteracao = "Bloqueio";
			if (blockDescription == null)
				alteracao.Descricao = "Bloqueado por Administrador";
			else
				alteracao.Descricao = blockDescription;

			_context.AlteracoesAdm.Add(alteracao);

			await _context.SaveChangesAsync();

			var user = await _userManager.FindByIdAsync(utilizador.Id);

			user.LockoutEnabled = true;
			user.LockoutEnd = blockDate;
			await _userManager.UpdateAsync(user);

			return RedirectToAction("List");
		}

		public IActionResult BlockPartial(string id)
		{
			var utilizador = _context.Utilizadores.Find(id);
			return PartialView("_BlockModalPartial", utilizador);
		}
	}
}
