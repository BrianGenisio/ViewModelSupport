using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using ViewModelSupport;

namespace ViewModelExamples.Resolvers
{
    // This code is loosely based of of John Papa's and Glen Block's example
    // of using MEF for a ViewModel Locator
    // http://johnpapa.net/silverlight/simple-viewmodel-locator-for-mvvm-the-patients-have-left-the-asylum/

    public class MEFViewModelResolver : IViewModelResolver
    {
        public MEFViewModelResolver()
        {
            var catalog = new AssemblyCatalog(GetType().Assembly);
            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }
        
        [ImportMany("ViewModel", AllowRecomposition = true)]
        public IEnumerable<ExportFactory<object, IViewModelMetadata>> ViewModelsFactories { get; set; }

        public object Resolve(string viewModelName)
        {
            var context = ViewModelsFactories.Single(v => v.Metadata.Name.Equals(viewModelName)).CreateExport();
            return context.Value;
        }
    }

    public interface IViewModelMetadata
    {
        string Name { get; }
    }

    public interface IViewModel {}

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExportViewModel : ExportAttribute
    {
        public string Name { get; private set; }

        public ExportViewModel(string name)
            : base("ViewModel")
        {
            Name = name;
        }
    }
}