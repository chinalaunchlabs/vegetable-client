using System;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using Vegeta.Models;
using System.Collections.Generic;

namespace Vegeta
{
	[ImplementPropertyChanged]
	public class DisplayPageModel : FreshBasePageModel
	{
		public DisplayPageModel ()
		{
		}

		protected override void ViewIsAppearing (object sender, EventArgs e)
		{
			LoadInfo ();
			base.ViewIsAppearing (sender, e);
		}

		private async void LoadInfo() {
			IsLoading = true;
			var response = await App.Client.FetchVegetables();
			if (response != null) {
				App.FromServer.Clear ();
				foreach (var veg in response) {
					if (!App.Favorites.Exists (v => v.Name == veg.Name)) {
						App.FromServer.Add (veg);
					}
				}
				IsLoading = false;
				MessagingCenter.Send<DisplayPageModel, List<Vegetable>> (this, "AllVegetablesLoaded", response);
			} else {
				var msg = App.Client.StatusMessage;
				IsLoading = false;
				await CoreMethods.DisplayAlert ("Error", msg, "OK");
			}
		}

		private bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { _isLoading = value; }
		}

		private bool _loaded;
		public bool Loaded {
			get { return _loaded; }
			set { _loaded = value; }
		}
	}
}

