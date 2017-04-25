using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using CoreGraphics;
using Foundation;
using UIKit;

namespace InnovifySample.iOS
{
  public class ContactInfoController : UITableViewController
  {
    public readonly IObservable<Unit> SucceedStream;

    enum Cells { Image, Name, Email, Phone, Website, Position, Action }
    readonly int cellCount = Enum.GetNames(typeof(Cells)).Length;
		
    readonly API api;

    readonly CompositeDisposable disposables = new CompositeDisposable();

    readonly ContactInfo contactInfo = new ContactInfo();

    ButtonCell buttonCell;
    ImageCell logoCell;

    ISubject<Unit> subject = new Subject<Unit>();

    public ContactInfoController()
    {
      api = new API(null);
      SucceedStream = subject.AsObservable();
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      TableView.BackgroundColor = UIColor.FromRGB(238/255.0f, 91/255.0f, 51/255.0f);

      TableView.RegisterClassForCellReuse(typeof(TextFieldCell), "TextFieldCell");
      TableView.RowHeight = UITableView.AutomaticDimension;
      TableView.TableFooterView = new UIView();

      logoCell = new ImageCell();
      logoCell
        .CloseStream
        .Subscribe(_ => PresentingViewController.DismissViewController(true, null))
        .AddTo(disposables);

      buttonCell = new ButtonCell();
      buttonCell
        .ActionStream
        .SelectMany(_ => SignupManager.SignUp(api, null))
        .ObserveOn(SynchronizationContext.Current)
        .Subscribe(Succeed)
        .AddTo(disposables);
    }

    #region Logic
    IObservable<string> SignUp() =>
    SignupManager.SignUp(api, null)
                   .ObserveOn(SynchronizationContext.Current)
                   .Catch<string, Exception>(ex => {
      Failed(ex.Message);
      return Observable.Empty<string>();
    });

    void Succeed(string token) 
    {
      subject.OnNext(Unit.Default);
    }

    void Failed(string msg) 
    {
      Console.WriteLine(msg);
    }
    #endregion

    #region Table View
    public override nint NumberOfSections(UITableView tableView) => 1;

    public override nint RowsInSection(UITableView tableView, nint section) => cellCount;

    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
    {
      switch((Cells) indexPath.Row) {
        case Cells.Image:    return logoCell;
        case Cells.Name:     return ReuseTextFieldCell(tableView, indexPath).Update(contactInfo.name,    "YOUR NAME",(ci, txt) => ci.name     = txt);
        case Cells.Email:    return ReuseTextFieldCell(tableView, indexPath).Update(contactInfo.email,   "EMAIL",    (ci, txt) => ci.email    = txt);
        case Cells.Phone:    return ReuseTextFieldCell(tableView, indexPath).Update(contactInfo.phone,   "PHONE NO.",(ci, txt) => ci.phone    = txt);
        case Cells.Website:  return ReuseTextFieldCell(tableView, indexPath).Update(contactInfo.website, "WEBSITE",  (ci, txt) => ci.website  = txt);
        case Cells.Position: return ReuseTextFieldCell(tableView, indexPath).Update(contactInfo.position,"POSITION", (ci, txt) => ci.position = txt);
        case Cells.Action :  return buttonCell;
          default: return new UITableViewCell();
      }
    }

    TextFieldCell ReuseTextFieldCell (UITableView tableView, NSIndexPath indexPath) =>
    tableView.DequeueReusableCell(new NSString("TextFieldCell"), indexPath) as TextFieldCell;

    public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
    {
      switch((Cells) indexPath.Row) {
        case Cells.Image:    return 50;
        case Cells.Name:
        case Cells.Email:
        case Cells.Phone:
        case Cells.Website:
        case Cells.Position: return 64;
        case Cells.Action: 
        default          :   return 64;
      }
    }
    #endregion

    #region Cells
    class TextFieldCell : UITableViewCell 
    {

      readonly UITextField textField;
      public TextFieldCell(IntPtr handle) : base(handle) 
      {
        SelectionStyle = UITableViewCellSelectionStyle.None;
        ContentView.BackgroundColor = UIColor.Clear;
        BackgroundColor = UIColor.Clear;

        var textFieldHeight = 44;

        textField = new UITextField {
          TranslatesAutoresizingMaskIntoConstraints = false,
          BackgroundColor = UIColor.White,
          LeftView = new UIView(new CGRect(0, 0, 25, 20)),
          LeftViewMode = UITextFieldViewMode.Always
        };
        textField.Layer.CornerRadius = textFieldHeight / 2;
        textField.Layer.BorderColor = UIColor.Clear.CGColor;

        var views = new NSDictionary(new NSString("textField"), textField);
        var metrics = new NSDictionary(new NSString("tfh"), textFieldHeight);

        ContentView.Add(textField);

        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-20-[textField]-20-|", 0, metrics, views));
        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-10-[textField(tfh)]-10-|", 0, metrics, views));
      }

      public TextFieldCell Update(string text, string placeholder, Action<ContactInfo, string> action)
      {
        textField.Text = text;
        textField.Placeholder = placeholder;
        return this;
      }

    }

    class ButtonCell : UITableViewCell 
    {
      public IObservable<Unit> ActionStream;

			readonly UIButton button;

      public ButtonCell() 
      {
        SelectionStyle = UITableViewCellSelectionStyle.None;
        ContentView.BackgroundColor = UIColor.Clear;
        BackgroundColor = UIColor.Clear;

        var buttonHeight = 44;

        button = UIButton.FromType(UIButtonType.System);
				button.TranslatesAutoresizingMaskIntoConstraints = false;
        button.SetTitle("Done", UIControlState.Normal);
        button.SetTitleColor(UIColor.White.ColorWithAlpha(0.8f), UIControlState.Normal);
        button.Font = UIFont.FromName(button.Font.Name, 20);
        //button.Layer.CornerRadius = buttonHeight / 2;
        //button.Layer.BorderColor = UIColor.Clear.CGColor;

        var views = new NSDictionary("button", button);
        var metrics = new NSDictionary(new NSString("bh"), buttonHeight);

        ContentView.Add(button);

        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-35-[button]-35-|", 0, metrics, views));
        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-10-[button(bh)]-10-|", 0, metrics, views));

        ActionStream = button.RXTouchUpInside();
      }
    }

    class ImageCell : UITableViewCell 
    {
      public IObservable<Unit> CloseStream;

      public ImageCell() 
      {
        SelectionStyle = UITableViewCellSelectionStyle.None;
        ContentView.BackgroundColor = UIColor.Clear;
        BackgroundColor = UIColor.Clear;

        var contenView = new UIView {
          Alpha = 0.5f,
          BackgroundColor = UIColor.Black,
          TranslatesAutoresizingMaskIntoConstraints = false
        };

        var imageView = new UIImageView(UIImage.FromBundle("logo")) {
          TranslatesAutoresizingMaskIntoConstraints = false
        };

        var closeButton = UIButton.FromType(UIButtonType.System);
        closeButton.Frame = new CoreGraphics.CGRect(20, 30, 50, 50);
        closeButton.SetTitle("x", UIControlState.Normal);
        closeButton.SetTitleColor(UIColor.White.ColorWithAlpha(0.8f), UIControlState.Normal);
        closeButton.Font = UIFont.FromName(closeButton.Font.Name, 35);
        CloseStream = closeButton.RXTouchUpInside();

        var views = new NSDictionary("contenView", contenView, "imageView", imageView, "closeButton", closeButton);

        ContentView.Add(contenView);
        ContentView.Add(imageView);
        ContentView.Add(closeButton);

        ContentView.AddConstraint(NSLayoutConstraint.Create(contenView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, ContentView, NSLayoutAttribute.CenterX, 1f, 0f));
        ContentView.AddConstraint(NSLayoutConstraint.Create(contenView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, ContentView, NSLayoutAttribute.CenterY, 1f, 0f));
        ContentView.AddConstraint(NSLayoutConstraint.Create(contenView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, ContentView, NSLayoutAttribute.Width, 1f, 0f));
        ContentView.AddConstraint(NSLayoutConstraint.Create(contenView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, ContentView, NSLayoutAttribute.Height, 1f, 0f));

        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[imageView(200)]", 0, null, views));
				ContentView.AddConstraint(NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, ContentView, NSLayoutAttribute.CenterX, 1f, 0f));
        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-50-[imageView(60)]-30-|", 0, null, views));

        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-10-[closeButton]", 0, null, views));
        ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-60-[closeButton]", 0, null, views));
      }
    }
    #endregion
  }

  class ContactInfo {
    public string name     { get; set; }
    public string email    { get; set; }
    public string phone    { get; set; }
    public string website  { get; set; }
    public string position { get; set; }
    public string message  { get; set; }
  }
}
