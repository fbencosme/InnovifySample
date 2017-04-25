using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using UIKit;

namespace InnovifySample.iOS
{
  public partial class ViewController : UIViewController
  {
    readonly CompositeDisposable disposables = new CompositeDisposable();

    public ViewController(IntPtr handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      Button.SetTitle("Add Contact", UIControlState.Normal);
      Button
        .RXTouchUpInside()
        .Select(_ => new ContactInfoController())
        .SelectMany(vc => {
          this.ShowViewController(vc, null);
          return vc.SucceedStream;
        })
        .Subscribe(_ => {
          DismissViewController(true, () => {
  					var alert = UIAlertController.Create("Succeed", "Contact Info Sent", UIAlertControllerStyle.Alert);
  					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default,(obj) => {}));
  					ShowViewController(alert, null);
            
          });
        })
        .AddTo(disposables);
    }

    public override void DidReceiveMemoryWarning()
    {
      base.DidReceiveMemoryWarning();
      // Release any cached data, images, etc that aren't in use.    
    }

    protected override void Dispose(bool disposing)
    {
      disposables.Dispose();
      base.Dispose(disposing);
    }
  }
}
