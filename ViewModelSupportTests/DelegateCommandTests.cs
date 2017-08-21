
using NUnit.Framework;
using System;
using ViewModelSupport;

namespace ViewModelSupportTests
{
    [TestFixture]
    public class DelegateCommandTests
    {
        [Test]
        public void Calling_Execute_Runs_Delegate()
        {
            bool commandWasRun = false;
            var command = new DelegateCommand(() => commandWasRun = true);

            command.Execute(null);

            Assert.That(commandWasRun);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void DelegateCommand_With_Null_Execute_Delegate_Throws()
        {
            new DelegateCommand(null);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Calling_CanExecute_Evaluates_Predicate(bool predicateResult)
        {
            var command = new DelegateCommand(() => { }, () => predicateResult);

            Assert.That(command.CanExecute(null), Is.EqualTo(predicateResult));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void DelegateCommand_With_Null_CanExecute_Delegate_Throws()
        {
            new DelegateCommand(() => { }, null);
        }

        [Test]
        public void Default_CanExecute_Returns_True()
        {
            var command = new DelegateCommand(() => { });

            Assert.That(command.CanExecute(true));
        }

        [Test]
        public void RaiseCanExecuteChanged_Fires_Event()
        {
            var command = new DelegateCommand(() => { });
            bool changedWasRaised = false;
            command.CanExecuteChanged += (s, e) => changedWasRaised = true;

            command.RaiseCanExecuteChanged();

            Assert.That(changedWasRaised);
        }

        [Test]
        public void Generic_DelegateCommand_Receives_Execute_Parameter()
        {
            int executeParameter = 0;
            var command = new DelegateCommand<int>(x => executeParameter = x);

            command.Execute(55);

            Assert.That(executeParameter, Is.EqualTo(55));
        }

        [Test]
        public void Generic_DelegateCommand_Receives_CanExecute_Parameter()
        {
            int canExecuteParameter = 0;
            var command = new DelegateCommand<int>(x => { }, x => { canExecuteParameter = x; return true; });

            command.CanExecute(66);

            Assert.That(canExecuteParameter, Is.EqualTo(66));
        }

    }
}