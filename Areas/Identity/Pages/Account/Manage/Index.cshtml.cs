// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Biblioteca_LAB.Data;
using Biblioteca_LAB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SQLitePCL;

namespace Biblioteca_LAB.Areas.Identity.Pages.Account.Manage
{
	public class IndexModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public IndexModel(
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

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[TempData]
		public string StatusMessage { get; set; } = null;

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		/// 

		public bool IsEmailConfirmed { get; set; }


		public string Email { get; set; }


		[BindProperty]
		public InputModel Input { get; set; }

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

			[StringLength(30)]
			[Unicode(false)]
			public string? Username { get; set; }



			[DataType(DataType.Password)]
			[Display(Name = "Password atual")]
			public string? OldPassword { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[StringLength(100, ErrorMessage = "A {0} deve ter no mínmo {2} e no máximo {1} caractéres.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password nova")]
			public string? NewPassword { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirme password")]
			[Compare("NewPassword", ErrorMessage = "As passwords não coincidem.")]
			public string? ConfirmPassword { get; set; }

			[EmailAddress]
			[Display(Name = "Email novo")]
			public string? NewEmail { get; set; }
		}

		private async Task LoadAsync(IdentityUser user)
		{
			Utilizador utilizador = await _context.Utilizadores.FindAsync(user.Id);


			ViewData["username"] = utilizador.Username;

			Input = new InputModel
			{
				Username = utilizador.Username,
			};

			Email = utilizador.Email;

			IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await LoadAsync(user);
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadAsync(user);
				return Page();
			}

			Utilizador utilizador = await _context.Utilizadores.FindAsync(user.Id);

			if (Input.Username != utilizador.Username && Input.Username != null)
			{
				utilizador.Username = Input.Username;

				_context.Utilizadores.Update(utilizador);

				await _signInManager.UserManager.SetUserNameAsync(user, Input.Username);
			}
			else
			{
				StatusMessage = "Introduza um Username válido";
			}


			if (Input.NewEmail != utilizador.Email && Input.NewEmail != null)
			{
				utilizador.Email = Input.NewEmail;
				await _signInManager.UserManager.SetEmailAsync(user, Input.NewEmail);
				user.EmailConfirmed = true;
			}
			else
			{
				StatusMessage = "Introduza um Email válido";
			}

			if (Input.NewPassword != null) 
			{
				var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
				if (!changePasswordResult.Succeeded)
				{
					foreach (var error in changePasswordResult.Errors)
					{
						StatusMessage = "Ocorreu um erro ao alterar a sua password";
					}
					return Page();

				}
				utilizador.Password = user.PasswordHash;
			}

			_context.Update(utilizador);
			await _context.SaveChangesAsync();
			await _signInManager.RefreshSignInAsync(user);

			StatusMessage = "O seu perfil foi atualizado com sucesso!";
			return RedirectToPage();
		}
	}
}
