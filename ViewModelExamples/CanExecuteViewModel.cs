using ViewModelSupport;

namespace ViewModelExamples
{
    public class CanExecuteViewModel : ViewModelBase
    {
        public void Execute_MakeLower()
        {
            Output = Input.ToLower();
        }

        [DependsUpon("Input")]
        public bool CanExecute_MakeLower()
        {
            return !string.IsNullOrWhiteSpace(Input);
        }

        public string Input
        {
            get { return Get(() => Input); }
            set { Set(() => Input, value);}
        }

        public string Output
        {
            get { return Get(() => Output); }
            set { Set(() => Output, value); }
        }
    }
}