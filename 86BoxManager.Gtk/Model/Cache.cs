using System.Collections.Generic;
using Gtk;
using System.Linq;
using _86BoxManager.Models;

namespace _86BoxManager.Model
{
    internal static class Cache
    {
        private static readonly IDictionary<string, VMRow> _tags;

        static Cache()
        {
            _tags = new Dictionary<string, VMRow>();
        }

        public static void ClearAll(this TreeView view)
        {
            var store = view.GetStore();
            store.Clear();
            _tags.Clear();
        }

        private static ListStore GetStore(this TreeView view)
        {
            var model = GetStore(view.Model);
            return model;
        }

        private static ListStore GetStore(this ITreeModel view)
        {
            var model = (ListStore)view;
            return model;
        }

        public static void ClearSelect(this TreeView view)
        {
            view.Selection.UnselectAll();
        }

        public static List<VMRow> GetSelItems(this TreeView view)
        {
            return GetSelItems(view.Selection);
        }

        public static List<VMRow> GetSelItems(this TreeSelection selection)
        {
            var res = new List<VMRow>();
            selection.SelectedForeach((model, _, it) =>
            {
                var store = model.GetStore();
                var key = GetKey(it, store);
                var vm = _tags[key];
                res.Add(vm);
            });
            return res;
        }

        public static List<VMRow> GetAllItems(this TreeView view)
        {
            var store = view.GetStore();
            var res = store.OfType<object[]>().Select(row =>
            {
                var key = GetKey(row);
                var vm = _tags[key];
                return vm;
            }).ToList();
            return res;
        }

        private static string GetKey(this IReadOnlyList<object> array)
            => (string)array[1];

        private static string GetKey(this TreeIter it, ITreeModel store)
            => (string)store.GetValue(it, 1);

        public static void RemoveItem(this TreeView view, object item)
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public static VMRow FindItemWithText(string text)
        {
            if (!_tags.TryGetValue(text, out var row))
                return null;

            return row;
        }

        public static VMRow Insert(this TreeView view, string key, VM vm)
        {
            var icon = VMRow.GetIcon(vm.Status);
            var name = vm.Name;
            var status = vm.GetStatusString();
            var desc = vm.Desc;
            var path = vm.Path;

            var nv = new object[] { icon, name, status, desc, path };
            var model = view.GetStore();
            var it = model.AppendValues(nv);

            return _tags[key] = new VMRow(vm, it, model);
        }

        public static VMRow FindItemWithText(this TreeView view, string vmName)
        {
            var rows = view.GetAllItems();
            var row = rows.FirstOrDefault(r => r.Tag.Name.Equals(vmName));
            return row;
        }
    }
}