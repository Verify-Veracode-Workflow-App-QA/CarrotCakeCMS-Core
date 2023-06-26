﻿using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;

/*
* CarrotCake CMS (MVC Core)
* http://www.carrotware.com/
*
* Copyright 2015, 2023, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: June 2023
*/

namespace Carrotware.CMS.CoreMVC.UI.Admin.Models {

	public class ModuleInfo {

		public ModuleInfo() {
			this.OpenTab = 0;
			this.SelectedTab = 0;
			this.SelectedCssClass = "notSelectedModule";

			this.SelectedArea = CMSConfigHelper.PluginAreaPath;
			this.SelectedAreaName = string.Empty;
			this.SelectedPluginAreaName = string.Empty;
			this.SelectedPluginActionName = string.Empty;

			this.CurrentAction = string.Empty;
			this.CurrentController = string.Empty;
			this.CurrentActionFull = string.Empty;

			this.Modules = new List<CMSAdminModule>();
			this.RouteValues = new RouteValueDictionary();

			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				this.Modules = cmsHelper.AdminModules;
			}
		}

		public ModuleInfo(ViewContext viewContext)
			: this() {
			LoadContext(viewContext);
		}

		public void LoadContext(ViewContext viewContext) {
			this.RouteValues = viewContext.RouteData.Values;

			if (this.RouteValues["action"] != null) {
				this.CurrentAction = this.RouteValues["action"].ToString();
			}
			if (this.RouteValues["controller"] != null) {
				this.CurrentController = this.RouteValues["controller"].ToString();
			}

			this.CurrentActionFull = string.Format("{0}", this.CurrentAction);

			string currentQueryString = string.Empty;
			var request = CarrotHttpHelper.HttpContext.Request;

			if (request.QueryString.HasValue) {
				currentQueryString = request.QueryString.ToString();
				if (!string.IsNullOrEmpty(currentQueryString)) {
					this.CurrentActionFull = string.Format("{0}?{1}", this.CurrentAction, currentQueryString);
				}
			}
		}

		public bool EvalModule(CMSAdminModule mod) {
			if (mod.AreaKey.ToLowerInvariant() == this.SelectedArea.ToLowerInvariant()) {
				this.SelectedTab = this.OpenTab;
				this.SelectedAreaName = mod.AreaKey;
				this.SelectedPluginAreaName = mod.PluginName;

				return true;
			}

			return false;
		}

		public bool EvalPlug(CMSAdminModuleMenu plug) {
			this.SelectedCssClass = "notSelectedModule";
			if (plug.AreaKey == this.SelectedArea && plug.Action == this.CurrentActionFull
						&& plug.Controller == this.CurrentController) {
				this.SelectedCssClass = "selectedModule";
				this.SelectedPluginActionName = plug.Caption;

				return true;
			}

			return false;
		}

		public string GetPluginCaption() {
			if (!string.IsNullOrEmpty(this.SelectedPluginAreaName)) {
				this.GetCurrentPlug();

				if (!string.IsNullOrEmpty(this.SelectedPluginActionName)) {
					return string.Format("{0} : {1}", this.SelectedPluginAreaName, this.SelectedPluginActionName);
				} else {
					return string.Format("{0}", this.SelectedPluginAreaName);
				}
			}

			return string.Empty;
		}

		public CMSAdminModuleMenu GetCurrentPlug() {
			CMSAdminModuleMenu plug = null;

			CMSAdminModule mod = this.Modules.Where(x => x.AreaKey.ToLowerInvariant() == this.SelectedArea.ToLowerInvariant()).FirstOrDefault();

			if (mod != null) {
				plug = mod.PluginMenus.Where(x => x.Action.ToLowerInvariant() == this.CurrentActionFull.ToLowerInvariant()
								&& x.Controller.ToLowerInvariant() == this.CurrentController.ToLowerInvariant()).FirstOrDefault();

				if (plug == null) {
					plug = mod.PluginMenus.Where(x => x.Action.ToLowerInvariant() == this.CurrentAction.ToLowerInvariant()
								&& x.Controller.ToLowerInvariant() == this.CurrentController.ToLowerInvariant()).FirstOrDefault();
				}
			}

			if (plug != null) {
				this.SelectedPluginActionName = plug.Caption;
			}

			return plug;
		}

		public int OpenTab { get; set; }
		public int SelectedTab { get; set; }
		public string SelectedCssClass { get; set; }

		public string SelectedArea { get; set; }
		public string SelectedAreaName { get; set; }
		public string SelectedPluginAreaName { get; set; }
		public string SelectedPluginActionName { get; set; }

		public string CurrentAction { get; set; }
		public string CurrentController { get; set; }
		public string CurrentActionFull { get; set; }

		public List<CMSAdminModule> Modules { get; set; }
		public RouteValueDictionary RouteValues { get; set; }
	}
}