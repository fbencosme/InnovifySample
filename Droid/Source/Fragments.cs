
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
  public class Contact : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Contact;
    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);

      var name     = view.FindViewById<EditText>(Resource.Id.name);
      var email    = view.FindViewById<EditText>(Resource.Id.email);
      var phone    = view.FindViewById<EditText>(Resource.Id.phone);
      var website  = view.FindViewById<EditText>(Resource.Id.website);
      var position = view.FindViewById<EditText>(Resource.Id.position);
      var message  = view.FindViewById<EditText>(Resource.Id.message);

      Observable.CombineLatest(
        name    .RxTextChanged().StartWith(string.Empty),
        email   .RxTextChanged().StartWith(string.Empty),
        phone   .RxTextChanged().StartWith(string.Empty),
        website .RxTextChanged().StartWith(string.Empty),
        position.RxTextChanged().StartWith(string.Empty),
        message .RxTextChanged().StartWith(string.Empty),
        Tuple.Create);

    }

    void OnValidate(
      Tuple<EditText, string> name,
      Tuple<EditText, string> email,
      Tuple<EditText, string> phone,
      Tuple<EditText, string> website,
      Tuple<EditText, string> position,
      Tuple<EditText, string> message) {

    }

  }

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
