using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;
using Vegeta;

using Wiggin.Drawing.iOS;

namespace Vegetoo.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : XFormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			this.SetIoc ();

			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}

		private void SetIoc() {
			var resolverContainer = new SimpleContainer ();

			var app = new XFormsAppiOS ();
			app.Init (this);

			resolverContainer.Register<IMediaPicker, MediaPicker> ();
			Resolver.SetResolver (resolverContainer.GetResolver ());
		}
	}
}

