using ViewModelSupport;

namespace ViewModelExamples
{
    public class DynamicPropertyViewModel : ViewModelBase
    {
        public DynamicPropertyViewModel()
        {
            Set("Foo", "Bar");
        }
    }
}