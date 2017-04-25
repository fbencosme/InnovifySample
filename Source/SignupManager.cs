using System;
namespace InnovifySample
{
  public static class SignupManager
  {
    public static IObservable<string> SignUp(API api, ContactInfo signUp) => 
    api.SignUp(signUp);
  }
}
