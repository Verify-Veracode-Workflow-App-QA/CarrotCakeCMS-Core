﻿using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

	public class DuplicateWidgetFromModel {

		public DuplicateWidgetFromModel() {
			this.SelectedPage = null;
			this.SelectedItem = Guid.Empty;
			this.Pages = new List<SiteNav>();
			this.Widgets = new List<Widget>();
			this.StepNumber = 0;
			this.CopyCount = 0;
		}

		public DuplicateWidgetFromModel(Guid id, string ph)
			: this() {
			this.Root_ContentID = id;
			this.PlaceholderName = ph;
			this.CopyCount = 0;
		}

		[Display(Name = "Search For")]
		[Required]
		public string SearchFor { get; set; }

		[Display(Name = "Hide Inactive Results")]
		public bool HideInactive { get; set; }

		public string PlaceholderName { get; set; }
		public Guid Root_ContentID { get; set; }

		public Guid SelectedItem { get; set; }

		public List<SiteNav> Pages { get; set; }
		public int TotalPages { get; set; }
		public ContentPage SelectedPage { get; set; }
		public List<Widget> Widgets { get; set; }

		public int StepNumber { get; set; }

		public int CopyCount { get; set; }

		public void SearchOne() {
			int iTake = 25;
			this.SelectedPage = null;
			this.Widgets = null;
			this.SelectedItem = Guid.Empty;
			this.TotalPages = 0;

			using (ISiteNavHelper navHelper = SiteNavFactory.GetSiteNavHelper()) {
				if (!string.IsNullOrEmpty(this.SearchFor)) {
					this.TotalPages = navHelper.GetSiteSearchCount(SiteData.CurrentSiteID, this.SearchFor, this.HideInactive);
					this.Pages = navHelper.GetLatestContentSearchList(SiteData.CurrentSiteID, this.SearchFor, this.HideInactive, iTake, 0, "NavMenuText", "ASC");
				}
			}
		}

		public void SearchTwo() {
			this.Widgets = null;
			this.Pages = null;

			using (ContentPageHelper pageHelper = new ContentPageHelper()) {
				this.SelectedPage = pageHelper.FindContentByID(SiteData.CurrentSiteID, this.SelectedItem);
				this.Widgets = this.SelectedPage.GetWidgetList();
			}
		}

		public void Save() {
			this.CopyCount = 0;
			if (this.Widgets != null) {
				List<Guid> lstSel = this.Widgets.Where(x => x.Selected).Select(x => x.Root_WidgetID).ToList();

				using (ContentPageHelper pageHelper = new ContentPageHelper()) {
					this.SelectedPage = pageHelper.FindContentByID(SiteData.CurrentSiteID, this.SelectedItem);
					this.Widgets = this.SelectedPage.GetWidgetList();
				}

				using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
					cmsHelper.OverrideKey(this.Root_ContentID);

					if (cmsHelper.cmsAdminWidget != null) {
						List<Widget> cacheWidget = cmsHelper.cmsAdminWidget;

						List<Widget> ww = (from w in this.SelectedPage.GetWidgetList()
										   where lstSel.Contains(w.Root_WidgetID) && w.IsLatestVersion == true
										   select w).ToList();

						if (ww != null) {
							this.CopyCount = ww.Count;

							foreach (var w in ww) {
								Guid newWidget = Guid.NewGuid();

								Widget wCpy = new Widget {
									Root_ContentID = this.Root_ContentID,
									Root_WidgetID = newWidget,
									WidgetDataID = Guid.NewGuid(),
									PlaceholderName = this.PlaceholderName,
									ControlPath = w.ControlPath,
									ControlProperties = w.ControlProperties,
									IsLatestVersion = true,
									IsPendingChange = true,
									IsWidgetActive = true,
									IsWidgetPendingDelete = false,
									WidgetOrder = w.WidgetOrder,
									GoLiveDate = w.GoLiveDate,
									RetireDate = w.RetireDate,
									EditDate = SiteData.CurrentSite.Now
								};

								cacheWidget.Add(wCpy);
							}
						}

						cmsHelper.cmsAdminWidget = cacheWidget;
					}
				}
			}
		}
	}
}