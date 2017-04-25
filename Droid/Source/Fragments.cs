using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.App;

namespace InnovifySample.Droid
{
  /*
   * Contact Form View.
   */
  public class Contact : BaseFragment, IForm
  {

    protected override int LayoutRes => Resource.Layout.Contact;

    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);

      var progess  = new ProgressDialog
        .Builder(Context)
        .SetMessage(Resource.String.contacting)
        .Create();

      var name     = view.FindViewById<EditText>(Resource.Id.name);
      var email    = view.FindViewById<EditText>(Resource.Id.email);
      var phone    = view.FindViewById<EditText>(Resource.Id.phone);
      var website  = view.FindViewById<EditText>(Resource.Id.website);
      var position = view.FindViewById<EditText>(Resource.Id.position);
      var message  = view.FindViewById<EditText>(Resource.Id.message);
      var send     = view.FindViewById<Button>  (Resource.Id.send);

      var submit = Observable.Merge(
        name    .RxKeyPressed(),
        email   .RxKeyPressed(),
        phone   .RxKeyPressed(),
        website .RxKeyPressed(),
        position.RxKeyPressed(),
        message .RxKeyPressed()
      ).Where(_ => _ == Keycode.Enter)
       .Select(_ => Unit.Default)
       .Merge(send.RxClick());
      
      Observable.CombineLatest(
        name    .RxTextChanged().StartWith(string.Empty),
        email   .RxTextChanged().StartWith(string.Empty),
        phone   .RxTextChanged().StartWith(string.Empty),
        website .RxTextChanged().StartWith(string.Empty),
        position.RxTextChanged().StartWith(string.Empty),
        message .RxTextChanged().StartWith(string.Empty)
      , Tuple.Create)
      .SampleLatest(submit)
      .Do(_ =>
         OnClean(name, email, phone, website, position, message))
      .Do(_ => progess.Show())
      .SelectMany(_ =>
        OnValidate(
          Tuple.Create(name    , _.Item1),
          Tuple.Create(email   , _.Item2),
          Tuple.Create(phone   , _.Item3),
          Tuple.Create(website , _.Item4),
          Tuple.Create(position, _.Item5),
          Tuple.Create(message , _.Item6))
        .Catch<ContactInfo, Exception>(e => {
          OnError(progess);
          return Observable.Empty<ContactInfo>();
      }))
      .Select(_ => Section.Welcome)
      .Do   (_ => progess.Dismiss())
      .Do   (_ => Toast.MakeText(Context, Resource.String.contact_sent, ToastLength.Long).Show())
      .Subscribe(Nav)
      .AddTo(Disposables);

    }

    void OnError(AlertDialog progess) { 
      progess.Dismiss();
      Toast.MakeText(Context, Resource.String.invalid_form, ToastLength.Long).Show();
    }

    /**
     * Clean the error inputs
     */
    void OnClean(
     EditText name,
     EditText email,
     EditText phone,
     EditText website,
     EditText position,
     EditText message)
    {
      name    .Error =
      email   .Error =
      phone   .Error =
      website .Error =
      position.Error =
      message .Error = null;
    }

    /**
    * Basic validator.
    */
    IObservable<ContactInfo> OnValidate(
      Tuple<EditText, string> name,
      Tuple<EditText, string> email,
      Tuple<EditText, string> phone,
      Tuple<EditText, string> website,
      Tuple<EditText, string> position,
      Tuple<EditText, string> message) =>

        OnValidate(name)     ||
        OnValidate(email)    ||
        OnValidate(phone)    ||
        OnValidate(website)  ||
        OnValidate(position) ||
        OnValidate(message)
    
        ? Observable.Throw<ContactInfo>(new Exception("invalid form"))
        : Observable.Return(new ContactInfo { 
          name     = name.Item2,
          email    = email.Item2,
          phone    = phone.Item2,
          website  = website.Item2,
          position = position.Item2,
          message  = message.Item2
        });

    // Input validator.
    bool OnValidate(Tuple<EditText, string> t) {
      var invalid = string.IsNullOrEmpty(t.Item2);
      if (invalid)
      {
        t.Item1.Error = GetString(Resource.String.required);
        t.Item1.RequestFocus();
      }
      return invalid;
    }

    public void Clean() {
      if (this.View != null) { 
        View.FindViewById<EditText>(Resource.Id.name    ).Text = string.Empty;
        View.FindViewById<EditText>(Resource.Id.email   ).Text = string.Empty;
        View.FindViewById<EditText>(Resource.Id.phone   ).Text = string.Empty;
        View.FindViewById<EditText>(Resource.Id.website ).Text = string.Empty;
        View.FindViewById<EditText>(Resource.Id.position).Text = string.Empty;
        View.FindViewById<EditText>(Resource.Id.message ).Text = string.Empty;
        View.FindViewById<Button>  (Resource.Id.send    ).Text = string.Empty;
      }
    }

  }

  /*
   *  Welcome or first displayd View.
   */
  public class Welcome : BaseFragment
	{
    protected override int LayoutRes => Resource.Layout.Welcome;

    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      view.FindViewById<Button>(Resource.Id.contactUs)
          .RxClick()
          .Select(_ => Section.Contact)
          .Subscribe(Nav)
          .AddTo(Disposables);
    }

	} 

  /*
   *  Bye or last step View.
   */
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
