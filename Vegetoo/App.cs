using System;

using Xamarin.Forms;
using FreshMvvm;
using Vegeta.Models;
using System.Collections.Generic;

namespace Vegeta
{
	public class App : Application
	{
		static VegetableClient _client;
		public static VegetableClient Client {
			get {
				_client = _client ?? new VegetableClient ();
				return _client;
			}
		}

		static List<Vegetable> _faves;
		public static List<Vegetable> Favorites {
			get {
				return _faves ?? (_faves = new List<Vegetable>());
			}
			set {
				_faves = value;
			}
		}

		static List<Vegetable> _fromServer;
		public static List<Vegetable> FromServer {
			get { return _fromServer ?? (_fromServer = new List<Vegetable> ()); }
			set { _fromServer = value; }
		}


		public App ()
		{
			var tabbedNav = new FreshTabbedNavigationContainer ();
			tabbedNav.AddTab<DisplayPageModel> ("All Vegetables", null);
			tabbedNav.AddTab<FavoritesPageModel> ("Fave Vegetables", null);
			tabbedNav.AddTab<NewVegetablePageModel> ("Add Vegetable", null);
			MainPage = tabbedNav;

		}

	}
}

