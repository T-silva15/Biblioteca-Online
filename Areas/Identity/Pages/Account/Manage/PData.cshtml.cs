// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Biblioteca_LAB.Data;
using Biblioteca_LAB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

namespace Biblioteca_LAB.Areas.Identity.Pages.Account.Manage
{
    public class PDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public PDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_context = context;
		}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }


		[TempData]
		public string StatusMessage { get; set; } = null;

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
        {
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			/// 


			[StringLength(40)]
			[Unicode(false)]
			[Display(Name = "Nome")]
			public string Name { get; set; }

			[DataType(DataType.PhoneNumber)]
            [MaxLength(9)]
			[Range(0, int.MaxValue, ErrorMessage = "Por favor introduza um contacto válido")]
			public string Contacto { get; set; }

			[StringLength(30)]
			[Unicode(false)]
			public string? Cidade { get; set; }

			[StringLength(30)]
			[Unicode(false)]
			[Display(Name = "Endereço")]
			public string? Endereco { get; set; }

			[Column("Codigo_Postal")]
			[DataType(DataType.PostalCode)]
			[StringLength(8)]
			[Unicode(false)]
			[Display(Name = "Código postal")]
			public string? CodigoPostal { get; set; }

            [DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string? Password { get; set; }

			[DataType(DataType.Password)]
			[Compare("Password", ErrorMessage = "As passwords não coincidem")]
			[Display(Name = "Confirme password")]
			public string? ConfirmPassword { get; set; }
		}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Utilizador utilizador = await _context.Utilizadores.FindAsync(user.Id);

            ViewData["name"] = utilizador.Nome;
            if (utilizador.Tipo != "Admin")
            {
				ViewData["contacto"] = utilizador.Contacto;
                if (utilizador.Tipo != "Bibliotecario")
                {
					ViewData["cidade"] = utilizador.Cidade;
					ViewData["endereco"] = utilizador.Endereco;
					ViewData["codigoPostal"] = utilizador.CodigoPostal;
				}
			}

			RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

		public async Task<IActionResult> OnPostDeleteProfileAsync()
		{


			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (Input.Password == null)
			{
				ModelState.AddModelError(string.Empty, "Por favor introduza a sua password para confirmar a eliminação da sua conta.");
				return Page();
			}

			if (!await _userManager.CheckPasswordAsync(user, Input.Password))
			{
				ModelState.AddModelError(string.Empty, "Password Incorreta.");
				return Page();
			}

			_context.Utilizadores.Remove(await _context.Utilizadores.FindAsync(user.Id));
			_context.SaveChanges();

			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				throw new InvalidOperationException($"Unexpected error occurred deleting user.");
			}

			await _signInManager.SignOutAsync();

			return Redirect("~/");
		}


		public async Task<IActionResult> OnPostAsync()
        {
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			Utilizador utilizador = await _context.Utilizadores.FindAsync(user.Id);

			if (Input.Name != utilizador.Nome)
			{
				utilizador.Nome = Input.Name;
			}
			else
			{
				StatusMessage = "O seu nome novo é igual ao antigo";
			}

			if (Input.Contacto != utilizador.Contacto)
			{
				utilizador.Contacto = Input.Contacto;
			}
			else
			{
				StatusMessage = "O seu contacto novo é igual ao antigo";
			}

			if (Input.Cidade != utilizador.Cidade)
			{
				utilizador.Cidade = Input.Cidade;
			}
			else
			{
				StatusMessage = "A sua cidade nova é igual à antiga";
			}

			if (Input.Endereco != utilizador.Endereco)
			{
				utilizador.Endereco = Input.Endereco;
			}
			else
			{
				StatusMessage = "O seu endereço novo é igual ao antigo";
			}

			if (Input.CodigoPostal != utilizador.CodigoPostal)
			{
				utilizador.CodigoPostal = Input.CodigoPostal;
			}
			else
			{
				StatusMessage = "O seu código postal novo é igual ao antigo";
			}

			_context.Update(utilizador);
			await _context.SaveChangesAsync();
			StatusMessage = "Os seus dados pessoais foram alterados com sucesso";

			ViewData["name"] = utilizador.Nome;
			if (utilizador.Tipo != "Admin")
			{
				ViewData["contacto"] = utilizador.Contacto;
				if (utilizador.Tipo != "Bibliotecario")
				{
					ViewData["cidade"] = utilizador.Cidade;
					ViewData["endereco"] = utilizador.Endereco;
					ViewData["codigoPostal"] = utilizador.CodigoPostal;
				}
			}

			return Page();
		}
    }
}
