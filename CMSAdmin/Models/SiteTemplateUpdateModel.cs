﻿using Carrotware.CMS.Core;
using System;
using System.Collections.Generic;

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

	public class SiteTemplateUpdateModel {

		public SiteTemplateUpdateModel() {
			using (CMSConfigHelper cmsHelper = new CMSConfigHelper()) {
				this.SiteTemplateList = cmsHelper.Templates;
			}
		}

		public List<CMSTemplate> SiteTemplateList { get; set; }

		public string HomePageLink { get; set; }
		public string HomePageTitle { get; set; }
		public string HomePage { get; set; }
		public string AllContent { get; set; }
		public string TopPages { get; set; }
		public string SubPages { get; set; }

		public Guid? IndexPageID { get; set; }
		public string IndexPage { get; set; }
		public string BlogPages { get; set; }

		public string AllPages { get; set; }
	}
}