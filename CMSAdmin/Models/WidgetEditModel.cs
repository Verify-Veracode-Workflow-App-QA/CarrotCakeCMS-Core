﻿using Carrotware.CMS.Core;
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

	public class WidgetEditModel {

		public WidgetEditModel() {
			this.Root_WidgetID = Guid.Empty;
			this.Root_ContentID = null;
			this.Widget = null;
			this.CachedWidget = false;
		}

		public WidgetEditModel(Guid widgetid)
			: this(null, widgetid) {
			this.Root_WidgetID = widgetid;
			this.Root_ContentID = null;
			this.Widget = null;
		}

		public WidgetEditModel(Guid? rootcontentid, Guid widgetid) {
			this.Root_WidgetID = widgetid;
			this.Root_ContentID = rootcontentid;
			this.CachedWidget = rootcontentid.HasValue;

			LoadData();
		}

		public void Save() {
			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				Widget ww = null;

				if (this.CachedWidget) {
					cmsHelper.OverrideKey(this.Root_ContentID.Value);
					this.Widgets = cmsHelper.cmsAdminWidget;

					ww = (from w in this.Widgets
						  where w.Root_WidgetID == this.Root_WidgetID
						  select w).FirstOrDefault();
				} else {
					using (WidgetHelper widgetHelper = new WidgetHelper()) {
						ww = widgetHelper.Get(this.Root_WidgetID);
					}
				}

				if (ww != null) {
					ww.IsPendingChange = true;
					ww.IsWidgetActive = this.Widget.IsWidgetActive;
					ww.IsWidgetPendingDelete = this.Widget.IsWidgetPendingDelete;

					ww.EditDate = SiteData.CurrentSite.Now;
					ww.GoLiveDate = this.Widget.GoLiveDate;
					ww.RetireDate = this.Widget.RetireDate;

					if (this.CachedWidget) {
						this.Widgets.RemoveAll(x => x.Root_WidgetID == this.Root_WidgetID);
						this.Widgets.Add(ww);
						cmsHelper.cmsAdminWidget = this.Widgets.OrderBy(x => x.WidgetOrder).ToList();
					} else {
						ww.Save();
					}
				}
			}
		}

		public Widget Widget { get; set; }

		public List<Widget> Widgets { get; set; }

		public string WidgetCaption { get; set; }

		public Guid Root_WidgetID { get; set; }

		public Guid? Root_ContentID { get; set; }

		public bool CachedWidget { get; set; }

		protected void LoadData() {
			if (this.Root_ContentID.HasValue) {
				using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
					cmsHelper.OverrideKey(this.Root_ContentID.Value);
					this.Widgets = cmsHelper.cmsAdminWidget;
					this.CachedWidget = true;

					if (this.Widget == null) {
						this.Widget = (from w in this.Widgets
									   where w.Root_WidgetID == this.Root_WidgetID
									   select w).FirstOrDefault();
					}
				}
			} else {
				using (WidgetHelper widgetHelper = new WidgetHelper()) {
					if (this.Widget == null) {
						this.Widget = widgetHelper.Get(this.Root_WidgetID);
						this.Root_ContentID = this.Widget.Root_ContentID;
					}
				}
			}

			GetCtrlName();
		}

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
	}
}