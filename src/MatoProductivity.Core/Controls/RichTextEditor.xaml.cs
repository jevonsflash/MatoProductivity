namespace MatoProductivity.Core.Controls;

public partial class RichTextEditor : ContentView
{
    public RichTextEditor()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextProperty =
      BindableProperty.Create("Text", typeof(string), typeof(RichTextEditor), default, propertyChanged: (bindable, oldValue, newValue) =>
      {
          var obj = (RichTextEditor)bindable;
          obj.MainEditor.Text=newValue as string;
      });

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty =
      BindableProperty.Create("Placeholder", typeof(string), typeof(RichTextEditor), default, propertyChanged: (bindable, oldValue, newValue) =>
      {
          var obj = (RichTextEditor)bindable;
          obj.MainEditor.Placeholder=newValue as string;
      });

    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    private void BoldButton_Clicked(object sender, EventArgs e)
    {
        this.MainEditor.BoldChanged();
    }

    private void ItalicButton_Clicked(object sender, EventArgs e)
    {
        this.MainEditor.ItalicChanged();

    }

    private void UnderLineButton_Clicked(object sender, EventArgs e)
    {
        this.MainEditor.UnderlineChanged();

    }
}