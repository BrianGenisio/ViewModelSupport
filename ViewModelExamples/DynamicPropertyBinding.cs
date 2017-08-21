using System.ComponentModel.Composition;
using ViewModelExamples.Resolvers;
using ViewModelSupport;

namespace ViewModelExamples
{
    [ExportViewModel("DynamicPropertyBinding")]
    public class DynamicPropertyBinding : ViewModelBase, IViewModel
    {
        public DynamicPropertyBinding()
        {
            Set("Friend", "Brian");
        }

        [DependsUpon("Friend")]
        public string FriendSentance
        {
            get { return "My friend is " + Get<string>("Friend") + "."; }
        }

        public void Execute_UpdateFriend(string name)
        {
            Set("Friend", name);
        }
    }
}