
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace InnovifySample.Droid
{
  public abstract class BaseFragment : Fragment
  {
    abstract protected int LayoutRes { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
      inflater.Inflate(LayoutRes, container, false);
  }
}
