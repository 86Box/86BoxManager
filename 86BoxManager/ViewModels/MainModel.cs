using _86boxManager.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace _86boxManager.ViewModels
{
    internal class MainModel : ReactiveObject
    {
        public MainModel()
        {
            // TODO
            Machines = new ObservableCollection<VMRow>
            {
                new VMRow { Tag = new VM("Hello", "First time", "/unix/a") { Status = 0 } },
                new VMRow { Tag = new VM("Goodbye", "Second time", "/unix/b") { Status = 1 } },
                new VMRow { Tag = new VM("Thanks", "Third time", "/unix/c") { Status = 2 } },
                new VMRow { Tag = new VM("Gracias", "Fourth time", "/unix/d") { Status = 3 } }
            };
            // TODO
        }

        public ObservableCollection<VMRow> Machines { get; }
    }
}