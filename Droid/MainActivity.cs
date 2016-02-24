using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XLabs.Ioc;
using XLabs.Forms;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Mvvm;

namespace Vegeta.Droid
{
	[Activity (Label = "Vegetoo.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : XFormsApplicationDroid
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			if (!Resolver.IsSet) {
				this.SetIoc ();
			} else {
				var app = Resolver.Resolve<IXFormsApp>() as IXFormsApp<XFormsApplicationDroid>;
				if (app != null) app.AppContext = this;
			}

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		private void SetIoc() {
			var resolverContainer = new SimpleContainer();
			var app = new XFormsAppDroid ();

			app.Init (this);

			resolverContainer.Register<IMediaPicker, MediaPicker>();

			Resolver.SetResolver (resolverContainer.GetResolver ());
		}
	}
}

