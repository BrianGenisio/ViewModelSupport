using System;
using System.Dynamic;

namespace ViewModelSupport
{
    public interface IViewModelResolver
    {
        object Resolve(string viewModelName);
    }

    public class DefaultViewModelResolver : IViewModelResolver
    {
        public object Resolve(string viewModelName) { return null; }
    }

    public class ViewModelLocator : DynamicObject
    {
        public ViewModelLocator()
        {
            Resolver = new DefaultViewModelResolver();
        }

        public IViewModelResolver Resolver { get; set; }

        public object this[string viewModelName]
        {
            get
            {
                return Resolver.Resolve(viewModelName);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }
    }
}