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

namespace Carrotware.CMS.UI.Components {

	public interface ICmsChildrenComponent {
		string CssHasChildren { get; set; }
		string CssItem { get; set; }
		string CssULClassLower { get; set; }
		string CssULClassTop { get; set; }

		bool MultiLevel { get; }

		List<SiteNav> GetChildren(Guid rootContentID);

		List<SiteNav> GetTopNav();

		List<SiteNav> NavigationData { get; set; }
	}

	//==============

	public interface ICmsMainComponent {
		string ElementId { get; set; }
		string CssClass { get; set; }
		string CssSelected { get; set; }
	}
}