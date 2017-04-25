using System.Reactive.Subjects;
using System.Reactive.Disposables;

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

    protected CompositeDisposable Disposables { get; private set; }

    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      //Possible to re-enter OnViewCreated between OnStop and OnDestroyView
      if (!Disposables?.IsDisposed ?? false) 
        Disposables.Dispose();
      Disposables = new CompositeDisposable();

    }

    public override void OnDestroyView()
    {
      Disposables.Dispose();
      base.OnDestroyView();
    }

    protected ISubject<Section> Nav => (Activity as IContact).Nav;

  }
}
