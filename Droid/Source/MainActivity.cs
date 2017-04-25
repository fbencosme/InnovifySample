using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace InnovifySample.Droid
{
  [Activity(
    Label        = "InnovifySample",
    MainLauncher = true, Icon = "@mipmap/icon")]
  public class MainActivity : FragmentActivity
  {

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      SetContentView(Resource.Layout.Main);

      FindViewById<ViewPager>(Resource.Id.pager).Adapter = new Adapter(SupportFragmentManager);
    }
  }
}

