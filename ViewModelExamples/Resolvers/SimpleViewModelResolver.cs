using System;
using System.Linq;
using ViewModelSupport;

namespace ViewModelExamples.Resolvers
{
    public class SimpleViewModelResolver : IViewModelResolver
    {
        public object Resolve(string viewModelNameName)
        {
            var foundType = GetType().Assembly.GetTypes().FirstOrDefault(type => type.Name == viewModelNameName);
            if (foundType == null)
                return null;

            return Activator.CreateInstance(foundType);
        }
    }
}