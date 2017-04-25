using System;
using RestSharp.Portable;

namespace InnovifySample
{
  public class ContactService
  {
    public ContactService()
    {
    }

    public void SignUp(IRestClient client, SignUp auth)
    {
      var request = new RestRequest("api/users/signup", Method.POST);

    }
  }

  public class SignUp 
  {
    public string firstName { get; set; }
    public string lastName  { get; set; }
    public string email     { get; set; }
    public string password  { get; set; }
  }
}
