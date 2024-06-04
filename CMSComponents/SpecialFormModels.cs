﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Web;

/*
* CarrotCake CMS (MVC Core)
* http://www.carrotware.com/
*
* Copyright 2015, 2023, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: June 2023
*/

namespace Carrotware.CMS.UI.Components {

	//==================================================
	public class SiteSearch {

		private IHtmlHelper? _helper;
		private HttpContext? _http;

		public SiteSearch() {
		}

		public SiteSearch(IHtmlHelper helper) {
			_helper = helper;
			_http = _helper.ViewContext.HttpContext;
		}

		public SiteSearch(HttpContext http) {
			_http = http;
		}

		public static string Key {
			get {
				return "CMS_searchform";
			}
		}

		public void RestoreQueryString() {
			if (_http != null) {
				if (_http.Request.Query[SiteData.SearchQueryParameter].FirstOrDefault() != null) {
					this.query = _http.Request.Query[SiteData.SearchQueryParameter].FirstOrDefault() ?? string.Empty;
				}
			} else {
				if (CarrotHttpHelper.QueryString(SiteData.SearchQueryParameter) != null) {
					this.query = CarrotHttpHelper.QueryString(SiteData.SearchQueryParameter).ToString();
				}
			}
		}

		[StringLength(128)]
		public string query { get; set; } = string.Empty;
	}

	//==================================================

	public class ContactInfo : FormModelBase<ContactInfoSettings> {

		public ContactInfo()
			: base() {
			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_contactform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;

			if (this.ValidateSettings != null && this.ValidateSettings is ContactInfoSettings) {
				this.Settings = this.ValidateSettings as ContactInfoSettings;
			}
		}

		public Guid Root_ContentID { get; set; }
		public DateTime CreateDate { get; set; }

		[StringLength(32)]
		[Display(Name = "IP")]
		public string? CommenterIP { get; set; }

		[StringLength(256)]
		[Required]
		[Display(Name = "Name")]
		public string CommenterName { get; set; }

		[StringLength(256)]
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string CommenterEmail { get; set; }

		[StringLength(256)]
		[Display(Name = "URL")]
		public string? CommenterURL { get; set; }

		[StringLength(4096)]
		[Required]
		[Display(Name = "Comment")]
		public string PostCommentText { get; set; }

		public bool IsSaved { get; set; }

		public void SendMail(PostComment pc, ContentPage page) {
			HttpRequest request = CarrotHttpHelper.Request;

			if (this.Settings.NotifyEditors || !string.IsNullOrEmpty(this.Settings.DirectEmailKeyName)) {
				List<string> emails = new List<string>();

				if (this.Settings.NotifyEditors && page != null) {
					emails.Add(page.CreateUser.Email);

					if (page.EditUser.UserId != page.CreateUser.UserId) {
						emails.Add(page.EditUser.Email);
					}

					if (page.CreditUserId.HasValue) {
						emails.Add(page.CreditUser.Email);
					}
				}

				if (!string.IsNullOrEmpty(this.Settings.DirectEmailKeyName)) {
					var email = CarrotHttpHelper.Configuration.GetValue<string>(this.Settings.DirectEmailKeyName).ToString();

					if (!string.IsNullOrEmpty(email)) {
						emails.Add(email);
					}
				}

				string hostName = CarrotHttpHelper.Request.Host.Value.ToLowerInvariant();
				var host = CarrotWebHelper.BuildHttpHost();

				string mailSubject = string.Format("Comment Form From {0}", hostName);

				string sBody = "Name:   " + pc.CommenterName
					+ "\r\nEmail:   " + pc.CommenterEmail
					+ "\r\nURL:   " + pc.CommenterURL
					+ "\r\n-----------------"
					+ "\r\nComment:\r\n" + HttpUtility.HtmlEncode(pc.PostCommentText)
					+ "\r\n=================\r\n"
					+ "\r\nIP:   " + pc.CommenterIP
					+ "\r\nSite URL:   " + string.Format("{0}{1}", host, page.FileName)
					+ "\r\nSite Time:   " + SiteData.CurrentSite.Now.ToString()
					+ "\r\nUTC Time:   " + DateTime.UtcNow.ToString();

				string sEmail = string.Join(";", emails);

				EmailHelper.SendMail(null, sEmail, mailSubject, sBody, false);
			}
		}
	}

	//==================================================

	public class LogoutInfo : FormModelBase<LogoutInfoSettings> {

		public LogoutInfo() {
			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_logoutform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;   // new LogoutInfoSettings();

			if (this.ValidateSettings != null && this.ValidateSettings is LogoutInfoSettings) {
				this.Settings = this.ValidateSettings as LogoutInfoSettings;
			}

			this.IsLoggedIn = SecurityData.IsAuthenticated;
		}

		public bool IsLoggedIn { get; set; }
		public string? RedirectUri { get; set; }
	}

	//==================================================

	public class LoginInfo : FormModelBase<LoginInfoSettings> {

		public LoginInfo()
			: base() {
			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_loginform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;

			if (this.ValidateSettings != null && this.ValidateSettings is LoginInfoSettings) {
				this.Settings = this.ValidateSettings as LoginInfoSettings;
			}

			this.IsLoggedIn = SecurityData.IsAuthenticated;

			this.LogInStatus = this.IsLoggedIn ? SignInResult.Success : SignInResult.Failed;
		}

		[Required]
		[Display(Name = "Username")]
		[StringLength(128)]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		public string? RedirectUri { get; set; }

		public SignInResult LogInStatus { get; set; }
		public bool IsLoggedIn { get; set; }
	}

	//==================================================

	public class ForgotPasswordInfo : FormModelBase<ForgotPasswordInfoSettings> {

		public ForgotPasswordInfo()
			: base() {
			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_forgotform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;

			if (this.ValidateSettings != null && this.ValidateSettings is ForgotPasswordInfoSettings) {
				this.Settings = this.ValidateSettings as ForgotPasswordInfoSettings;
			}
		}

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128)]
		public string Email { get; set; }
	}

	//==================================================

	public class ResetPasswordInfo : FormModelBase<ResetPasswordInfoSettings> {

		public ResetPasswordInfo()
			: base() {
			this.CreationResult = null;

			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_resetform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;

			if (this.ValidateSettings != null && this.ValidateSettings is ResetPasswordInfoSettings) {
				this.Settings = this.ValidateSettings as ResetPasswordInfoSettings;
			}
		}

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

		public IdentityResult CreationResult { get; set; }
	}

	//==================================================

	public class ChangePasswordInfo : FormModelBase<ChangePasswordInfoSettings> {

		public ChangePasswordInfo()
			: base() {
			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_chngpassform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;

			if (this.ValidateSettings != null && this.ValidateSettings is ChangePasswordInfoSettings) {
				this.Settings = this.ValidateSettings as ChangePasswordInfoSettings;
			}
		}

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	//==================================================

	public class ChangeProfileInfo : FormModelBase<ChangeProfileInfoSettings> {

		public ChangeProfileInfo()
			: base() {
			ReconstructSettings();
		}

		public static string Key {
			get {
				return "CMS_chngproform";
			}
		}

		public void ReconstructSettings() {
			base.GetSettings();
			this.Settings = null;

			if (this.ValidateSettings != null && this.ValidateSettings is ChangeProfileInfoSettings) {
				this.Settings = this.ValidateSettings as ChangeProfileInfoSettings;
			}
		}

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		[StringLength(128)]
		public string Email { get; set; }

		[Display(Name = "User nickname")]
		[StringLength(64)]
		public string UserNickName { get; set; }

		[Display(Name = "First name")]
		[StringLength(64)]
		public string FirstName { get; set; }

		[Display(Name = "Last name")]
		[StringLength(64)]
		public string LastName { get; set; }
	}
}