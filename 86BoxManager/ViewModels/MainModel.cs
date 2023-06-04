using _86boxManager.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace _86boxManager.ViewModels
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