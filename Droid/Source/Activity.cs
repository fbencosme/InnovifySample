using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System.Linq;

namespace InnovifySample.Droid
{
  [Activity(
    Label        = "Innovify Sample",
    MainLauncher = true,
    Icon         = "@mipmap/icon",
    Theme        = "@android:style/Theme.Material")]
  public class MainActivity : FragmentActivity, IContact
  {
    protected CompositeDisposable Disposables { get; private set; }

    readonly ISubject<Section> nav = new Subject<Section>();
    int CurrStep = 0;

    public ISubject<Section> Nav => nav;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      SetContentView(Resource.Layout.Main);

      Disposables = new CompositeDisposable();

      var adapter   = new StepsAdapter(SupportFragmentManager);
      var pager     = FindViewById<ViewPager>(Resource.Id.pager);
      pager.Adapter = adapter;

      Nav
        .Select   (_ => (int) _)
        .StartWith(CurrStep)
        .Do       (_ => CurrStep = _) 
        .Do(_ => 
            SupportFragmentManager?
            .Fragments?
            .OfType<IForm>()?
            .ToList()?
            .ForEach(f => f.Clean()))
        .Subscribe(_ => pager.SetCurrentItem(_, true))
        .AddTo(Disposables);

    }

    protected override void OnDestroy()
    {
      Disposables.Dispose();
      base.OnDestroy();
      Dispose();
    }

    protected override void OnSaveInstanceState(Bundle outState)
    {
      outState.PutInt(nameof(CurrStep), CurrStep);

      base.OnSaveInstanceState(outState);
    }

    protected override void OnRestoreInstanceState(Bundle savedInstanceState)
    {
      base.OnRestoreInstanceState(savedInstanceState);
      CurrStep = savedInstanceState?.GetInt(nameof(CurrStep), CurrStep) ?? CurrStep;
    }
  }
}

