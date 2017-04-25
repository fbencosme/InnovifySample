using Android.Support.V4.App;

namespace InnovifySample.Droid
{
  
  public class StepsAdapter : FragmentPagerAdapter
  {

    public StepsAdapter(FragmentManager fm) : base(fm) { }

    public override int Count => 3;

    public override Fragment GetItem(int position)
    {
      switch ((Section) position)
      {
        case Section.Welcome : return new Welcome();
        case Section.Contact : return new Contact();
        case Section.Bye     : return new Bye();
        default              : return null;
      }
    }
  }
}
