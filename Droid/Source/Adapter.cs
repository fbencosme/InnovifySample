using System;
using Android.Support.V4.App;
using System.Runtime.Remoting.Messaging;

namespace InnovifySample.Droid
{
  public class Adapter : FragmentPagerAdapter
  {

    public Adapter(FragmentManager fm) : base(fm) { }

    public override int Count => 1;

    public override Fragment GetItem(int position)
    {
      switch (position)
      {
        case 0 : return new Contact();
        default: return null;
      }
    }
  }
}
