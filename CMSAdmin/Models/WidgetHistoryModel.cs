﻿using Carrotware.CMS.Core;
using Carrotware.Web.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;

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

	public class WidgetHistoryModel {

		public WidgetHistoryModel() {
			this.History = new PagedData<Widget>();
			this.History.InitOrderBy(x => x.EditDate, false);
		}

		public WidgetHistoryModel(Guid widgetid)
			: this() {
			this.Root_WidgetID = widgetid;

			using (WidgetHelper widgetHelper = new WidgetHelper()) {
				if (this.Widget == null) {
					this.History.DataSource = widgetHelper.GetWidgetVersionHistory(widgetid);
					this.Widget = this.History.DataSource.Where(x => x.IsLatestVersion == true).FirstOrDefault();

					this.Root_ContentID = this.Widget.Root_ContentID;
				}
			}

			GetCtrlName();
		}

		public string WidgetCaption { get; set; }

		protected void GetCtrlName() {
			string sName = string.Empty;

			if (this.Widget != null) {
				using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
					CMSPlugin plug = (from p in cmsHelper.ToolboxPlugins
									  where p.FilePath.ToLowerInvariant() == this.Widget.ControlPath.ToLowerInvariant()
									  select p).FirstOrDefault();

					if (plug != null) {
						sName = plug.Caption;
					}
				}
			}

			this.WidgetCaption = sName;
		}

		public Widget Widget { get; set; }

		public Guid Root_WidgetID { get; set; }
		public Guid Root_ContentID { get; set; }
		public PagedData<Widget> History { get; set; }

		public bool Remove() {
			using (WidgetHelper widgetHelper = new WidgetHelper()) {
				List<Guid> lstDel = this.History.DataSource.Where(x => x.Selected).Select(x => x.WidgetDataID).ToList();

				if (lstDel.Any()) {
					widgetHelper.RemoveVersions(lstDel);
					return true;
				}
			}

			return false;
		}
	}
}