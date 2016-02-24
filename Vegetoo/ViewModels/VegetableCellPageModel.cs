using System;
using FreshMvvm;
using PropertyChanged;
using Vegeta.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Vegeta
{
	[ImplementPropertyChanged]
	public class VegetableCellPageModel: FreshBasePageModel
	{
		private Vegetable _vegetable;
		public VegetableCellPageModel (Vegetable vegetable)
		{
			_vegetable = vegetable;
		}

		public string Name {
			get { return _vegetable.Name; }
		}

		public string PhotoURL {
			get { return _vegetable.Photo.Url; }
		}

		public Uri PhotoUri {
			get { 
				if (_vegetable != null)
					return new Uri (PhotoURL);
				else
					return null;
			}
		}

		private bool _isBtnVisible;
		public bool IsAddBtnVisible {
			get { 
				return _isBtnVisible;
			}
			set {
				_isBtnVisible = value; 
			}
		}

		private string _faveBtnText = "Favorite";
		public string FaveBtnText {
			get { return _faveBtnText; }
			set { _faveBtnText = value; }
		}

		private Color _faveBtnClr = Color.Green;
		public Color FaveBtnColor {
			get { return _faveBtnClr; }
			set { _faveBtnClr = value; }
		}

		public ICommand AddToFavesCommand {
			get {
				return new Command (() => {
					if (App.Favorites.Exists( v => v.Name == _vegetable.Name)) {
						App.Favorites.Remove(_vegetable);
						App.FromServer.Add(_vegetable);
					}
					else {
						App.FromServer.Remove(_vegetable);
						App.Favorites.Add(_vegetable);
					}
					MessagingCenter.Send<VegetableCellPageModel>(this, "FaveAdded");
				});
			}
		}
	}
}

