using System.Collections.Generic;

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