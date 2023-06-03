using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _86boxManager.ViewModels;
using Avalonia.Controls;

namespace _86boxManager.Models
{
    internal static class Cache
    {
        private static readonly IDictionary<string, VMRow> _tags;

        static Cache()
        {
            _tags = new Dictionary<string, VMRow>();
        }

        public static IList<VMRow> GetSelItems(this DataGrid view)
        {
            var items = view.SelectedItems.OfType<VMRow>().ToList();
            return items;
        }

        public static IList<VMRow> GetAllItems(this DataGrid view)
        {
            var model = (ObservableCollection<VMRow>)view.Items;
            return model;
        }

        public static void ClearSelect(this DataGrid view)
        {
            view.SelectedIndex = -1;
        }

        public static void ClearAll(this DataGrid view)
        {
            var model = (ObservableCollection<VMRow>)view.Items;
            // TODO model.Clear();
        }

        public static VMRow Insert(this DataGrid view, string vmName, VM vm)
        {
            throw new System.NotImplementedException();
        }

        public static VMRow FindItemWithText(string invVmName)
        {
            throw new System.NotImplementedException();
        }

        public static void RemoveItem(this DataGrid view, VMRow item)
        {
            var model = (ObservableCollection<VMRow>)view.Items;
            model.Remove(item);
        }

        public static VMRow FindItemWithText(this DataGrid view, string vmName)
        {
            var rows = view.GetAllItems();
            var row = rows.FirstOrDefault(r => r.Tag.Name.Equals(vmName));
            return row;
        }
    }
}