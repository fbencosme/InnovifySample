using System;
using System.Reactive.Linq;
using System.Reactive;
using System.Reactive.Subjects;

using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace InnovifySample.Droid
{
  using static Observable;

  public enum Section { Welcome, Contact, Bye }

  public interface IContact { 
    
    ISubject<Section> Nav { get ;}
  }

  public static class UiExtensions
  {
    public static IObservable<Keycode> RxKeyPressed(this EditText tv, bool ignoreBackKey = true) =>
      Create((IObserver<Keycode> obs) => {
        tv.SetOnKeyListener(new KeyUpPressedListener(obs, ignoreBackKey));
        return () => tv.SetOnKeyListener(null);
      });

    public static IObservable<string> RxTextChanged(this TextView tv) =>
      FromEventPattern<TextChangedEventArgs>(
        ev => tv.TextChanged += ev,
        ev => tv.TextChanged -= ev
      ).Select(_ => _.EventArgs.Text.ToString());

    public static IObservable<Unit> RxClick(this View v) =>
      FromEventPattern(
        ev => v.Click += ev,
        ev => v.Click -= ev
      ).Select(_ => Unit.Default);

  }

  class KeyUpPressedListener : Java.Lang.Object, View.IOnKeyListener
  {
    readonly IObserver<Keycode> Observer;
    readonly bool               IgnoreBackKey;

    public KeyUpPressedListener(IObserver<Keycode> obs, bool ignoreBackKey)
    {
      Observer = obs;
      IgnoreBackKey = ignoreBackKey;
    }

    public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
    {
      if (IgnoreBackKey && e.KeyCode == Keycode.Back)
        return false;

      if (e.Action == KeyEventActions.Up)
      {
        Observer.OnNext(keyCode);
        return true;
      }
      return false;
    }
  }

}
