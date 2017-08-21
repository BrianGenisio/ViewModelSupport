using ViewModelSupport;

namespace ViewModelExamples
{
    public class DefaultValuesViewModel : ViewModelBase
    {
        public string Text
        {
            get { return Get(() => Text, "This is the default value"); }
            set { Set(() => Text, value);}
        }
    }
}