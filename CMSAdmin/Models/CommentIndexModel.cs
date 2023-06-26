﻿using Carrotware.CMS.Core;
using Carrotware.Web.UI.Components;
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

	public class CommentIndexModel {

		public CommentIndexModel() {
			this.PageSizes = new List<int>();
			this.PageSizes.Add(10);
			this.PageSizes.Add(25);
			this.PageSizes.Add(50);
			this.PageSizes.Add(100);

			this.FilterOptions = Helper.CreateBoolFilter();

			this.Comments = new PagedData<PostComment>();
			this.Comments.InitOrderBy(x => x.CreateDate, false);
		}

		public Guid? Root_ContentID { get; set; }
		public ContentPageType.PageType PageType { get; set; }
		public PagedData<PostComment> Comments { get; set; }

		public bool? IsApproved { get; set; }

		public bool? IsSpam { get; set; }

		public List<int> PageSizes { get; set; }

		public Dictionary<bool, string> FilterOptions { get; set; }
	}
}