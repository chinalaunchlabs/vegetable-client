using System;
using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using Vegeta.Models;
using Wiggin.Drawing;

namespace Vegeta
{
	[ImplementPropertyChanged]
	public class NewVegetablePageModel: FreshBasePageModel
	{
		private Vegetable _vegetable;
		private IMediaPicker _mediaPicker;
		private MediaFile _mediaFile;

		public NewVegetablePageModel ()
		{
			_vegetable = new Vegetable ();
			Name = "";
		}

		public string Name {
			get { return _vegetable.Name; }
			set { _vegetable.Name = value; RaisePropertyChanged ("CanUpload"); }
		}

		private ImageSource _imgSrc = "placeholder.png";
		public ImageSource ImgSource {
			get { return _imgSrc; }
			set { _imgSrc = value; RaisePropertyChanged ("CanUpload"); }
		}

		public ICommand BrowseImage {
			get {
				return new Command (async () => {
					_mediaPicker = DependencyService.Get<IMediaPicker>();
					await SelectPicture();
				});
			}
		}

		public bool CanUpload {
			get {
				return Name.Length > 0 && _mediaFile != null;
			}
		}

		public ICommand UploadVeg {
			get {
				return new Command (async (blah) => {
					// convert stream to bytes in preparation for base64 conversion
					var resized = await DependencyService.Get<IPhotoTransformer>().ResizePhotoAsync(50, 50, _mediaFile.Path);

					byte[] inBase64 = resized.ToByteArray();
					string outBase64 = inBase64.Base64Convert();

					_vegetable.Photo = new Photo {
						Url = outBase64
					};

					string msg = await App.Client.UploadVegetable(_vegetable);
					if (msg == null)
						msg = App.Client.StatusMessage;

					await CoreMethods.DisplayAlert("Status", msg, "OK");

					ResetFields();

				}, (obj) => { return CanUpload; });
			}
		}

		private async Task SelectPicture() {
			try {
				_mediaFile = await this._mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions {
					DefaultCamera = CameraDevice.Front,
					MaxPixelDimension = 300
				});
//				var transformed = await DependencyService.Get<IPhotoTransformer>().TransformPhotoAsync(ImagingOperations.ConvertToGreyscale, _mediaFile.Path);
//				var resized = await DependencyService.Get<IPhotoTransformer>().ResizePhotoAsync(50, 50, _mediaFile.Path);
				ImgSource = ImageSource.FromStream(() => _mediaFile.Source);
//				ImgSource = ImageSource.FromStream(() => resized );

			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("The task was canceled");
			}
		}

		private void ResetFields() {
			if (_mediaFile != null)
				_mediaFile.Dispose ();
			Name = "";
			ImgSource = "placeholder.png";
		}
	}
}

