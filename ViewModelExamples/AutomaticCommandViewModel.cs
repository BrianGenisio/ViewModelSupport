using System.Windows.Input;
using ViewModelSupport;

namespace ViewModelExamples
{
    //public class AutomaticCommandViewModel
    //{
    //    public AutomaticCommandViewModel()
    //    {
    //        MyCommand = new DelegateCommand(Execute_MyCommand, CanExecute_MyCommand);
    //    }

    //    public void Execute_MyCommand()
    //    {
    //        // Do something
    //    }

    //    public bool CanExecute_MyCommand()
    //    {
    //        // Are we in a state to do something?
    //        return true;
    //    }

    //    public ICommand MyCommand { get; private set; }
    //}


    public class AutomaticCommandViewModel : ViewModelBase
    {
        public void Execute_MyCommand()
        {
            // Do something
        }

        public bool CanExecute_MyCommand()
        {
            // Are we in a state to do something?
            return true;
        }
    }
}