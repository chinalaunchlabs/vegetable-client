using System;

using Xamarin.Forms;
using Vegeta.Models;
//using System.Collections.Generic;

namespace Vegeta
{
	public class FavoritesPage : ContentPage
	{
		StackLayout stack;
		public FavoritesPage ()
		{

			Title = "Favorites";
			stack = new StackLayout {

			};

			Content = new ScrollView {
				Content = stack
			};

			MessagingCenter.Subscribe<VegetableCellPageModel> (this, "FaveAdded", (sender) => {
				ReloadList();
			});
		}

		void ReloadList() {
			var vegetables = App.Favorites;
			stack.Children.Clear ();
			foreach (var veg in vegetables) {
				var viewmodel = new VegetableCellPageModel(veg);
				var page = new VegetableCellPage();
				page.BindingContext = viewmodel;
				viewmodel.IsAddBtnVisible = false;
				viewmodel.FaveBtnText = "Remove";
				viewmodel.FaveBtnColor = Color.Red;
				stack.Children.Add(
					page
				);
			}
		}

		protected override void OnAppearing ()
		{
			ReloadList ();
			base.OnAppearing ();
		}
	}
}


