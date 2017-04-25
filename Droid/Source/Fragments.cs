
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
using System.Reactive.Linq;

namespace InnovifySample.Droid
{
	public class Welcome : BaseFragment
	{
    protected override int LayoutRes => Resource.Layout.Welcome;

    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      view.FindViewById<Button>(Resource.Id.ok)
          .RxClick()
          .Select(_ => Section.Contact)
          .Subscribe(Nav)
          .AddTo(Disposables);
    }

	}

  public class Contact : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Contact;
    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      view.RxClick()
          .Select(_ => Section.Contact)
          .Subscribe(Nav)
          .AddTo(Disposables);
    }

  }

  public class Bye : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Bye;
    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      view.FindViewById<Button>(Resource.Id.ok)
          .RxClick()
          .Select(_ => Section.Contact)
          .Subscribe(Nav)
          .AddTo(Disposables);
    }

  }
}
