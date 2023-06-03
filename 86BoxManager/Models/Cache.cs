using System.Collections.Generic;
using _86boxManager.Model;

namespace _86boxManager.Models
{
    internal static class Cache
    {
        private static readonly IDictionary<string, VMRow> _tags;

        static Cache()
        {
            _tags = new Dictionary<string, VMRow>();
        }
    }
}