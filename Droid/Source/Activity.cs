
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System.Reactive.Subjects;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace InnovifySample.Droid
{
  [Activity(
    Label        = "InnovifySample",
    MainLauncher = true,
    Icon         = "@mipmap/icon",
    Theme        = "@android:style/Theme.Material")]
  public class MainActivity : FragmentActivity, IContact
  {
    protected CompositeDisposable Disposables { get; private set; }

    readonly ISubject<Section> nav = new Subject<Section>();

    public ISubject<Section> Nav => nav;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      SetContentView(Resource.Layout.Main);

      Disposables = new CompositeDisposable();

      var adapter   = new Adapter(SupportFragmentManager);
      var pager     = FindViewById<ViewPager>(Resource.Id.pager);
      pager.Adapter = adapter;

      Nav
        .Select(_ => (int) _)
        .Subscribe(_ => pager.SetCurrentItem(_, true))
        .AddTo(Disposables);

    }

    protected override void OnDestroy()
    {
      Disposables.Dispose();
      base.OnDestroy();
      Dispose();
    }
  }
}

