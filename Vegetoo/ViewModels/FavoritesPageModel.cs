using System;
using FreshMvvm;
using PropertyChanged;
using System.Collections.Generic;
using Vegeta.Models;

namespace Vegeta
{
	[ImplementPropertyChanged]
	public class FavoritesPageModel: FreshBasePageModel
	{
		public FavoritesPageModel() {
		}

		protected override void ViewIsAppearing (object sender, EventArgs e)
		{
			base.ViewIsAppearing (sender, e);
		}
	}
}

