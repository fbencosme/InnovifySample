using System;
using System.Reactive;
using System.Reactive.Linq;
using UIKit;

namespace InnovifySample.iOS
{
  public static class Extensions
  {
    public static IObservable<Unit> RXTouchUpInside(this UIButton button) => Observable
      .FromEventPattern(
        h => button.TouchUpInside += h,
        h => button.TouchUpInside -= h)
      .Select(_ => Unit.Default);
  }
}
