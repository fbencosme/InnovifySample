using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace InnovifySample
{
  using static DateTime;
  using static Observable;

  public static class Extensions
  {
    public static IDisposable AddTo(this IDisposable disposable, CompositeDisposable disposables) {
      disposables.Add(disposable);
      return disposable;
    }

    public static IObservable<Result> ComposeLatest<Source, Sample, Result>(
     this IObservable<Source> source,
     IObservable<Sample> sample,
     Func<Source, Sample, Result> selector) =>
     Defer(() =>
     {
       var lastIndex = -1;
       return source.CombineLatest(
           sample.Select(Tuple.Create<Sample, int>),
           (a, b) =>
           new
           {
             Index = b.Item2,
             Sample = b.Item1,
             Source = a
           })
         .Where(x => x.Index != lastIndex)
         .Do(x => lastIndex = x.Index)
         .Select(x => selector(x.Source, x.Sample));
     });

    public static IObservable<T> Debounce<T>(this IObservable<T> source, TimeSpan span)
    {
      var lastSent = Now - span;
      return source
        .Where(t =>
        {
          var valid = (Now - lastSent) >= span;
          if (valid)
            lastSent = Now;
          return valid;
        });
    }

    public static IObservable<Source> SampleLatest<Source, Sample>(
        this IObservable<Source> source,
        IObservable<Sample> sample) =>
       source.ComposeLatest(sample, (p, _) => p);
  }
}
