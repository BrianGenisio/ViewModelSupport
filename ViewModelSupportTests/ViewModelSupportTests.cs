using System;
using System.Windows.Input;
using NUnit.Framework;
using System.Collections.Generic;
using ViewModelSupport;

namespace ViewModelSupportTests
{
    [TestFixture]
    public class ActiveViewModelTests
    {
        private class GetterSetter_By_String : ViewModelBase
        {
            public string Foo
            {
                get { return Get<string>("Foo"); }
                set { Set("Foo", value); }
            }

            public int MyInt
            { 
                get { return Get<int>("MyInt"); }
                set { Set("MyInt", value); }
            }

            public int IntWithDefault
            {
                get { return Get<int>("IntWithDefault", 56); }
                set { Set("IntWithDefault", value); }
            }
        }

        [Test]
        public void When_Value_Is_Set_By_String_It_Can_Be_Retrieved()
        {
            var viewModel = new GetterSetter_By_String();

            viewModel.Foo = "Bar";

            Assert.That(viewModel.Foo, Is.EqualTo("Bar"));
        }

        [Test]
        public void Int_Values_Can_Be_Gotten_And_Set()
        {
            var viewModel = new GetterSetter_By_String();

            viewModel.MyInt = 55;

            Assert.That(viewModel.MyInt, Is.EqualTo(55));
        }

        [Test]
        public void Setting_Value_Sends_PropertyChanged_Event()
        {
            var viewModel = new GetterSetter_By_String();
            string changedProperties = string.Empty;
            viewModel.PropertyChanged += (s, e) => changedProperties += e.PropertyName;

            viewModel.Foo = "Bar";

            Assert.That(changedProperties, Is.EqualTo("Foo"));
        }

        [Test]
        public void Setting_Twice_Does_Not_Fail()
        {
            var viewModel = new GetterSetter_By_String();

            viewModel.Foo = "Bar";
            viewModel.Foo = "Baz";

            Assert.That(viewModel.Foo, Is.EqualTo("Baz"));
        }

        [Test]
        public void Setting_To_Same_Value_Does_Not_Fire_PropertyChanged_Twice()
        {
            var viewModel = new GetterSetter_By_String();
            string changedProperties = string.Empty;
            viewModel.PropertyChanged += (s, e) => changedProperties += e.PropertyName;

            viewModel.Foo = "Bar";
            viewModel.Foo = "Bar";

            Assert.That(changedProperties, Is.EqualTo("Foo"));
        }

        [Test]
        public void Setting_To_Same_Null_Value_Does_Not_Fire_PropertyChanged_Twice()
        {
            var viewModel = new GetterSetter_By_String();
            string changedProperties = string.Empty;
            viewModel.PropertyChanged += (s, e) => changedProperties += e.PropertyName;

            viewModel.Foo = "test";
            viewModel.Foo = null;
            viewModel.Foo = null;

            Assert.That(changedProperties, Is.EqualTo("FooFoo"));
        }

        private class ViewModel_With_Mismatched_Get_And_Set_Names : ViewModelBase
        {
            public int Value
            {
                get { return Get<int>("WrongName"); }
                set { Set("Value", value); }
            }
        }

        [Test]
        public void Mismatched_Names_Returns_Default()
        {
            var viewModel = new ViewModel_With_Mismatched_Get_And_Set_Names();

            viewModel.Value = 55;

            Assert.That(viewModel.Value, Is.EqualTo(0));
        }

        [Test]
        public void Default_Values_On_Getter()
        {
            var viewModel = new GetterSetter_By_String();

            Assert.That(viewModel.IntWithDefault, Is.EqualTo(56));
        }

        [Test]
        public void Dynamic_Values_On_Setter()
        {
            var viewModel = new GetterSetter_By_String();

            (viewModel as dynamic).MyDynamicProperty = "Me";

            Assert.That((viewModel as dynamic).MyDynamicProperty, Is.EqualTo("Me"));
        }

        [Test]
        public void Setting_With_A_Default_Getter_Retrieves_Set_Value()
        {
            var viewModel = new GetterSetter_By_String();

            viewModel.IntWithDefault = 99;

            Assert.That(viewModel.IntWithDefault, Is.EqualTo(99));
        }

        public class ViewModel_With_Semantics : ViewModelBase
        {
            public string PropertyWithSemantics
            {
                get { return Get(() => PropertyWithSemantics); }   
                set { Set(() => PropertyWithSemantics, value);}
            }

            public int PropertyWithSemanticsWithDefault
            {
                get { return Get(() => PropertyWithSemanticsWithDefault, 5); }
                set { Set(() => PropertyWithSemanticsWithDefault, value); }
            }
        }

        [Test]
        public void When_Symantics_Are_Used_We_Can_Get_And_Set()
        {
            var viewModel = new ViewModel_With_Semantics();

            viewModel.PropertyWithSemantics = "Semantic";

            Assert.That(viewModel.PropertyWithSemantics, Is.EqualTo("Semantic"));
        }

        [Test]
        public void When_Symantics_Are_Used_We_Can_Get_With_Default_Value()
        {
            var viewModel = new ViewModel_With_Semantics();

            Assert.That(viewModel.PropertyWithSemanticsWithDefault, Is.EqualTo(5));
        }

        private class Initial_Value_On_Properties : ViewModelBase
        {
            public int InitialValueCount = 0;

            private string InitialValue()
            {
                InitialValueCount++;
                return "Default";
            }

            public string TestProperty
            {
                get { return Get(() => TestProperty, InitialValue); }
            }

            public string TestStringProperty
            {
                get { return Get("TestStringProperty", InitialValue); }
            }
        }

        [Test]
        public void Initial_Value_Is_Set_On_String_Getter()
        {
            var viewModel = new Initial_Value_On_Properties();

            var value = viewModel.TestStringProperty;

            Assert.That(value, Is.EqualTo("Default"));
        }

        [Test]
        public void Initial_Value_Is_Set_On_Lambda_Getter()
        {
            var viewModel = new Initial_Value_On_Properties();

            var value = viewModel.TestProperty;

            Assert.That(value, Is.EqualTo("Default"));
        }

        [Test]
        public void Initial_Value_Is_Only_Requested_Once()
        {
            var viewModel = new Initial_Value_On_Properties();

            Assert.That(viewModel.InitialValueCount, Is.EqualTo(0));
            var value = viewModel.TestProperty;

            Assert.That(viewModel.InitialValueCount, Is.EqualTo(1));
            value = viewModel.TestProperty;

            Assert.That(viewModel.InitialValueCount, Is.EqualTo(1));
        }

        private class Automatic_Command_Properties : ViewModelBase
        {
            public bool SomethingWasExecuted { get; set; }
            public bool CanExecuteSomethingWasExecuted { get; set; }

            public void Execute_Something()
            {
                SomethingWasExecuted = true;
            }

            public bool CanExecuteResult { get; set; }

            [DependsUpon("Text")]
            public bool CanExecute_Something()
            {
                CanExecuteSomethingWasExecuted = true;
                return CanExecuteResult;
            }

            public string Text
            {
                get { return Get(() => Text); }
                set { Set(() => Text, value); }
            }
        }

        [Test]
        public void Execute_Method_Generates_ICommand_Property()
        {
            dynamic viewModel = new Automatic_Command_Properties();

            var command = viewModel.Something;

            Assert.That(command is ICommand);
        }

        [Test]
        public void Execute_Method_Is_Wrapped_By_Dynamic_Property()
        {
            dynamic viewModel = new Automatic_Command_Properties();

            viewModel.Something.Execute(null);

            Assert.That(viewModel.SomethingWasExecuted);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CanExecute_Method_Is_Wrapped_By_Dynamic_Property(bool predicateResult)
        {
            dynamic viewModel = new Automatic_Command_Properties();

            viewModel.CanExecuteResult = predicateResult;
            
            Assert.That(viewModel.Something.CanExecute(null), Is.EqualTo(predicateResult));
        }

        [Test]
        public void Changing_Text_Causes_CanExecuteChanged_To_Fire()
        {
            dynamic viewModel = new Automatic_Command_Properties();
            bool CanExecuteChanged_Fired = false;
            (viewModel.Something as ICommand).CanExecuteChanged += (s, e) => CanExecuteChanged_Fired = true;

            viewModel.Text = "Foo";

            Assert.That(CanExecuteChanged_Fired);
         }

        [Test]
        public void When_Dependant_Changes_On_CanExecute_It_Does_Not_Execute()
        {
            dynamic viewModel = new Automatic_Command_Properties();

            viewModel.Text = "Something";

            Assert.That(viewModel.CanExecuteSomethingWasExecuted, Is.False);
        }

        private class Single_Dependency : ViewModelBase
        {
            public int InputA
            {
                get { return Get(() => InputA); }
                set { Set(() => InputA, value); }
            }

            [DependsUpon("InputA")]
            public int InputASquared
            {
                get { return InputA * InputA; }
            }

            public int NotDependent
            {
                get { return 20; }
            }
        }

        [Test]
        public void When_InputA_Changes_Dependent_Notification_Fires()
        {
            var viewModel = new Single_Dependency();
            List<string> propertiesChanged = new List<string>();
            viewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            viewModel.InputA = 5;

            Assert.That(propertiesChanged.Count, Is.EqualTo(2));
            Assert.That(propertiesChanged[0], Is.EqualTo("InputA"));
            Assert.That(propertiesChanged[1], Is.EqualTo("InputASquared"));
        }

        private class Single_Property_Multiple_Dependencies : ViewModelBase
        {
            public int InputA
            {
                get { return Get(() => InputA); }
                set { Set(() => InputA, value); }
            }

            public int InputB
            {
                get { return Get(() => InputB); }
                set { Set(() => InputB, value); }
            }

            [DependsUpon("InputA")]
            [DependsUpon("InputB")]
            public int APlusB
            {
                get { return InputA + InputB; }
            }

            public int NotDependent
            {
                get { return 20; }
            }
        }

        [Test]
        public void When_InputA_And_InputB_Changes_Dependent_Notification_Fires()
        {
            var viewModel = new Single_Property_Multiple_Dependencies();
            List<string> propertiesChanged = new List<string>();
            viewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            viewModel.InputA = 5;
            viewModel.InputB = 6;

            Assert.That(propertiesChanged.Count, Is.EqualTo(4));
            Assert.That(propertiesChanged[0], Is.EqualTo("InputA"));
            Assert.That(propertiesChanged[1], Is.EqualTo("APlusB"));
            Assert.That(propertiesChanged[2], Is.EqualTo("InputB"));
            Assert.That(propertiesChanged[3], Is.EqualTo("APlusB"));
        }

        private class Multiple_Properties_Single_Dependency : ViewModelBase
        {
            public int InputA
            {
                get { return Get(() => InputA); }
                set { Set(() => InputA, value); }
            }

            [DependsUpon("InputA")]
            public int InputASquared
            {
                get { return InputA * InputA; }
            }

            [DependsUpon("InputA")]
            public int InputACubed
            {
                get { return InputA * InputA * InputA; }
            }

            public int NotDependent
            {
                get { return 20; }
            }
        }

        [Test]
        public void When_InputA_Changes_All_Dependent_Notifications_Fires()
        {
            var viewModel = new Multiple_Properties_Single_Dependency();
            List<string> propertiesChanged = new List<string>();
            viewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            viewModel.InputA = 5;

            Assert.That(propertiesChanged.Count, Is.EqualTo(3));
            Assert.That(propertiesChanged[0], Is.EqualTo("InputA"));
            Assert.That(propertiesChanged[1], Is.EqualTo("InputASquared"));
            Assert.That(propertiesChanged[2], Is.EqualTo("InputACubed"));
        }

        private class Chained_Dependencies : ViewModelBase
        {
            public int InputA
            {
                get { return Get(() => InputA); }
                set { Set(() => InputA, value); }
            }

            [DependsUpon("InputA")]
            public int InputASquared
            {
                get { return InputA * InputA; }
            }

            [DependsUpon("InputASquared")]
            public string InputASquaredOutput
            {
                get { return "A Squared = " + InputASquared; }
            }

            public int NotDependent
            {
                get { return 20; }
            }
        }

        [Test]
        public void Dependencies_Chain_Through_The_Graph()
        {
            var viewModel = new Chained_Dependencies();
            List<string> propertiesChanged = new List<string>();
            viewModel.PropertyChanged += (s, e) => propertiesChanged.Add(e.PropertyName);

            viewModel.InputA = 5;

            Assert.That(propertiesChanged.Count, Is.EqualTo(3));
            Assert.That(propertiesChanged[0], Is.EqualTo("InputA"));
            Assert.That(propertiesChanged[1], Is.EqualTo("InputASquared"));
            Assert.That(propertiesChanged[2], Is.EqualTo("InputASquaredOutput"));
        }

        private class Dependent_Methods : ViewModelBase
        {
            public int InputA
            {
                get { return Get(() => InputA); }
                set { Set(() => InputA, value); }
            }

            public int DependentMethodExecuted { get; set; }

            [DependsUpon("InputA")]
            public void ExecuteWhenAChanges()
            {
                DependentMethodExecuted = InputA;
            }

            public void NotDependent()
            {
                DependentMethodExecuted++;
            }
        }

        [Test]
        public void When_InputA_Changes_Dependent_Method_Executes()
        {
            var viewModel = new Dependent_Methods();

            viewModel.InputA = 20;

            Assert.That(viewModel.DependentMethodExecuted, Is.EqualTo(20));
        }

        public class EmptyViewModel : ViewModelBase
        {
        }

        [Test]
        public void Setting_Dynamc_Properties_That_Do_Not_Exist_Can_Be_Retrieved()
        {
            var viewModel = new EmptyViewModel();

            viewModel.Set("Foo", "Bar");

            Assert.That((viewModel as dynamic).Foo, Is.EqualTo("Bar"));
        }

        public class InvalidDependencyCheckingForMethod : ViewModelBase
        {
            [DependsUpon("InputA", VerifyStaticExistence=true)]
            public void ExecuteWhenAChanges()
            {
            }
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void When_Dependant_Propety_For_Method_Does_Not_Exist_And_Verification_Is_Requested_Throw()
        {
            var viewModel = new InvalidDependencyCheckingForMethod();
        }

        public class InvalidDependencyCheckingForProperty : ViewModelBase
        {
            [DependsUpon("InputA", VerifyStaticExistence = true)]
            public string Derived
            {
                get { return string.Empty;}
            }
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void When_Dependant_Propety_For_Property_Does_Not_Exist_And_Verification_Is_Requested_Throw()
        {
            var viewModel = new InvalidDependencyCheckingForProperty();
        }
    }
}
