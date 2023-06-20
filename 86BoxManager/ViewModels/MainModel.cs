using ReactiveUI;
using System.Collections.ObjectModel;

namespace _86BoxManager.ViewModels
{
    internal class MainModel : ReactiveObject
    {
        public ObservableCollection<VMRow> Machines { get; } = new();

        private string _vmCount = "# of virtual machines:";

        public string VmCount
        {
            get => _vmCount;
            set
            {
                _vmCount = value;
                this.RaisePropertyChanged();
            }
        }
    }
}