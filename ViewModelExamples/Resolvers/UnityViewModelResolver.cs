using System.Linq;
using Microsoft.Practices.Unity;
using ViewModelSupport;

namespace ViewModelExamples.Resolvers
{
    public class UnityViewModelResolver : IViewModelResolver
    {
        private readonly UnityContainer container = new UnityContainer();

        public UnityViewModelResolver()
        {
            container.RegisterType<IRandomNumberGenerator, RandomNumberGenerator>();
        }

        public object Resolve(string viewModelNameName)
        {
            var foundType = GetType().Assembly.GetTypes().FirstOrDefault(type => type.Name == viewModelNameName);
            if (foundType == null)
                return null;

            return container.Resolve(foundType);
        }
    }
}