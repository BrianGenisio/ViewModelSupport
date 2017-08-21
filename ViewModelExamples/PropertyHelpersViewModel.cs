using System.ComponentModel;
using ViewModelSupport;

namespace ViewModelExamples
{
    //public class PropertyHelpersViewModel : INotifyPropertyChanged
    //{
    //    private string text;
    //    public string Text
    //    {
    //        get { return text; }
    //        set
    //        {
    //            if(text != value)
    //            {
    //                text = value;
    //                RaisePropertyChanged("Text");
    //            }
    //        }
    //    }

    //    protected void RaisePropertyChanged(string propertyName)
    //    {
    //        var handlers = PropertyChanged;
    //        if(handlers != null)
    //            handlers(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //}

    //public class PropertyHelpersViewModel : ViewModelBase
    //{
    //    public string Text
    //    {
    //        get { return Get<string>("Text"); }
    //        set { Set("Text", value);}
    //    }
    //}

    public class PropertyHelpersViewModel : ViewModelBase
    {
        public string Text
        {
            get { return Get(() => Text); }
            set { Set(() => Text, value); }
        }
    }
}