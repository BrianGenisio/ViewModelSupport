using ViewModelSupport;

namespace ViewModelExamples
{
    public class DependantPropertiesViewModel : ViewModelBase
    {
        public DependantPropertiesViewModel()
        {
            if(IsInDesignMode)
            {
                Score = .5;
            }
        }

        public double Score
        {
            get { return Get(() => Score); }
            set { Set(() => Score, value); }
        }

        [DependsUpon("Score")]
        public int Percentage
        {
            get { return (int)(100 * Score); }
        }

        [DependsUpon("Percentage")]
        public string Output
        {
            get { return "You scored " + Percentage + "%."; }
        }
    }
}