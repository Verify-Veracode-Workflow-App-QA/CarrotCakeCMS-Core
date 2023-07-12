﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

/*
* CarrotCake CMS (MVC Core)
* http://www.carrotware.com/
*
* Copyright 2015, 2023, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: June 2023
*/

namespace Carrotware.CMS.Security.Models {

	public class ExternalLoginConfirmationViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128)]
		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel {
		public string? ReturnUrl { get; set; }
	}

	public class SendCodeViewModel {
		public string SelectedProvider { get; set; }
		public ICollection<SelectListItem> Providers { get; set; }
		public string? ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}

	public class VerifyCodeViewModel {

		[Required]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }

		public string? ReturnUrl { get; set; }

		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }
	}

	public class ForgotViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128)]
		public string Email { get; set; }
	}

	public class LoginViewModel {

		public LoginViewModel() { }

		[Required]
		[Display(Name = "Username")]
		[StringLength(128)]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; } = true;

		public string? ReturnUrl { get; set; }
	}

	public class RegisterViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Username")]
		[StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
		public string UserName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class ResetPasswordViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128)]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class ForgotPasswordViewModel {

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128)]
		public string Email { get; set; }
	}
}