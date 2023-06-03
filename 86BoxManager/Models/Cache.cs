using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public static IList<VMRow> GetAllItems(this DataGrid view)
        {
            // TODO
            return new List<VMRow>();
        }

        public static void ClearSelect(this DataGrid view)
        {
            // TODO
        }

        public static void ClearAll(this DataGrid view)
        {
            // TODO
        }

        public static void Insert(this DataGrid view, string vmName, VM vm)
        {
            throw new System.NotImplementedException();
        }

        public static VMRow FindItemWithText(string invVmName)
        {
            throw new System.NotImplementedException();
        }
    }
}