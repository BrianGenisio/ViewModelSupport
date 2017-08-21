using System;
using ViewModelSupport;

namespace ViewModelExamples
{
    public class DependantMethodsViewModel : ViewModelBase
    {
        public double Score
        {
            get { return Get(() => Score); }
            set { Set(() => Score, value); }
        }

        [DependsUpon("Score")]
        public void WhenScoreChanges()
        {
            // Handle this case
        }
    }
}