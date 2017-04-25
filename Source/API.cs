using System;
using System.Linq;
using System.Reactive.Threading.Tasks;
using RestSharp.Portable;
using System.Reactive.Linq;

namespace InnovifySample
{
  public class API
  {
    readonly IRestClient client;

    public API(IRestClient client)
    {
      this.client = client;
    }

    internal IObservable<string> SignUp(SignUp signUp)
    {
      var request = new RestRequest("dummy/end/point", Method.POST);
      request.AddBody(signUp);
      return Observable.Return("dummy-tocken");
    }
  }

 public class ContactInfo
  {
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string website { get; set; }
    public string position { get; set; }
    public string message { get; set; }
  }
}
