using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Biblioteca_LAB.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Biblioteca_LAB.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;

namespace LABW_Biblioteca2024.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IUserStore<IdentityUser> _userStore;
		private readonly IUserEmailStore<IdentityUser> _emailStore;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;
		private readonly ApplicationDbContext _dbcontext;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RegisterModel(
			UserManager<IdentityUser> userManager,
			IUserStore<IdentityUser> userStore,
			SignInManager<IdentityUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender,
			ApplicationDbContext dbcontext,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
			_dbcontext = dbcontext;
			_roleManager = roleManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel
		{
			[Required(ErrorMessage = "O nome é necessário para criar a sua conta.")]
			[Display(Name = "Nome")]
			[MaxLength(40, ErrorMessage = "O {0} pode ter no máximo {1} caractéres.")]
			public string Name { get; set; }

			[Required(ErrorMessage = "O username é necessário para criar a sua conta.")]
			[Display(Name = "User Name")]
			[MaxLength(40, ErrorMessage = "O {0} pode ter no máximo {1} caractéres.")]
			public string UserName { get; set; }

			[Required(ErrorMessage = "O email é necessário para criar a sua conta.")]
			[EmailAddress(ErrorMessage = "O email introduzido não é um email válido.")]
			[MaxLength(30, ErrorMessage = "O {0} pode ter no máximo {1} caractéres.")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Display(Name = "Cidade")]
			[MaxLength(30, ErrorMessage = "O {0} pode ter no máximo {1} caractéres.")]
			public string? City { get; set; }

			[StringLength(8, ErrorMessage = "O {0} deve ser composto por {1} caractéres.", MinimumLength = 8)]
			[Display(Name = "Código Postal")]
			[DataType(DataType.PostalCode, ErrorMessage = "O Código Postal introduzido não é válido.")]
			public string? PostalCode { get; set; }

			[Display(Name = "Endereço")]
			[MaxLength(30, ErrorMessage = "O {0} pode ter no máximo {1} caractéres.")]
			public string? Address { get; set; }

			[Display(Name = "Contacto")]
			[StringLength(9, ErrorMessage = "O {0} deve ser composto por {1} caractéres.", MinimumLength = 9)]
			public string? Contact { get; set; }

			[Required(ErrorMessage = "A password é necessária para criar a sua conta.")]
			[StringLength(20, ErrorMessage = "O {0} deve ter no mínimo {2} caractéres.", MinimumLength = 8)]
			[DataType(DataType.Password, ErrorMessage = "A password introduzida não´é válida.")]
			[Display(Name = "Password")]
			[MaxLength(20, ErrorMessage = "O {0} pode ter no máximo {1} caractéres.")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirmar password")]
			[Compare("Password", ErrorMessage = "As passwords não são iguais.")]
			public string ConfirmPassword { get; set; }

			[Required]
			[Display(Name = "Tipo")]
			public string Role { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
			{
				ViewData["Roles"] = _roleManager.Roles.OrderBy(x => x.NormalizedName).ToList();

			}
			else
			{
				ViewData["Roles"] = _roleManager.Roles.Where(x => x.Name != "Admin").OrderByDescending(x => x.NormalizedName).ToList();

			}
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid)
			{
				var user = CreateUser();
				
				
				user.EmailConfirmed = true;
				await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (Input.Role == "Bibliotecario" && !User.Identity.IsAuthenticated)
				{
					user.LockoutEnabled = true;
				}
				else
				{
					user.LockoutEnabled = false;
				}
				await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					await _userManager.AddToRoleAsync(user, Input.Role);

					var userId = await _userManager.GetUserIdAsync(user);

					Utilizador newUser = new Utilizador
					{
						Id = userId,
						Nome = Input.Name,
						Username = Input.UserName,
						Email = Input.Email,
						Password = user.PasswordHash,
						Tipo = Input.Role,
						Contacto = Input.Role == "Admin" ? null : Input.Contact,
						Cidade = Input.Role == "Admin" || Input.Role == "Bibliotecario" ? null : Input.City,
						Endereco = Input.Role == "Admin" || Input.Role == "Bibliotecario" ? null : Input.Address,
						CodigoPostal = Input.Role == "Admin" || Input.Role == "Bibliotecario" ? null : Input.PostalCode,
					};

					if (Input.Role == "Bibliotecario" && !User.Identity.IsAuthenticated)
					{
						newUser.Block = true;
					}
					else
					{
						newUser.Block = false;
					}

					_dbcontext.Utilizadores.Add(newUser);
					await _dbcontext.SaveChangesAsync();

					if(User.Identity.IsAuthenticated && User.IsInRole("Admin"))
					{
						return RedirectToPage("~/Backoffice/Index");
					}

					if(newUser.Tipo == "Leitor")
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
					else
					{
						return RedirectToPage("~/Home/Index");
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
			{
				ViewData["Roles"] = _roleManager.Roles.OrderBy(x => x.NormalizedName).ToList();

			}
			else
			{
				ViewData["Roles"] = _roleManager.Roles.Where(x => x.Name != "Admin").OrderByDescending(x => x.NormalizedName).ToList();

			}
			return Page();
		}

		private IdentityUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<IdentityUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
					$"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		public IUserEmailStore<IdentityUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<IdentityUser>)_userStore;
		}
	}
}
