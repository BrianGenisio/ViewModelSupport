using System;
using ViewModelSupport;

namespace ViewModelExamples
{
    public interface IRandomNumberGenerator
    {
        int Get();
    }

    public class ViewModelWithDependancy : ViewModelBase
    {
        public ViewModelWithDependancy(IRandomNumberGenerator generator)
        {
            Value = generator.Get();
        }

        public int Value { get; private set; }
    }

    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public int Get() { return new Random().Next(); }
    }
}