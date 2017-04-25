
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace InnovifySample.Droid
{
	public class Welcome : BaseFragment
	{
    protected override int LayoutRes => Resource.Layout.Welcome;

	}

  public class Contact : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Contact;

  }

  public class Bye : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Bye;

  }
}
