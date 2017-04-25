using System;
using System.Reactive.Disposables;

namespace InnovifySample
{
  public static class Extensions
  {
    public static IDisposable AddTo(this IDisposable disposable, CompositeDisposable disposables) {
      disposables.Add(disposable);
      return disposable;
    }
  }
}
