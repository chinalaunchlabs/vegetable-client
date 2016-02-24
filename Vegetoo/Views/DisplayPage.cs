using System;

using Xamarin.Forms;
using System.Collections.Generic;
using Vegeta.Models;

namespace Vegeta
{
	public class DisplayPage : ContentPage
	{
		StackLayout stack;
		public DisplayPage ()
		{
			Title = "All Vegetables";
			stack = new StackLayout {
				
			};
			ActivityIndicator indicator = new ActivityIndicator {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			indicator.SetBinding (ActivityIndicator.IsVisibleProperty, "IsLoading");
			indicator.SetBinding (ActivityIndicator.IsRunningProperty, "IsLoading");

			stack.Children.Add (indicator);

			Content = new ScrollView {
				Content = stack
			};

			MessagingCenter.Subscribe<DisplayPageModel, List<Vegetable>> (this, "AllVegetablesLoaded", (sender, vegetables) => {
				ReloadList();
			});
			MessagingCenter.Subscribe<VegetableCellPageModel> (this, "FaveAdded", (sender) => {
				ReloadList();
			});
		}

		private void ReloadList() {

			var vegetables = App.FromServer;
			stack.Children.Clear();	// get rid of activity indicator
			foreach (Vegetable veg in vegetables) {
				var viewmodel = new VegetableCellPageModel(veg);
				viewmodel.IsAddBtnVisible = true;
				var page = new VegetableCellPage();
				page.BindingContext = viewmodel;
				stack.Children.Add(
					page
				);
			}
		}
			
	}
}


