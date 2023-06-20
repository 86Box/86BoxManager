using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#pragma warning disable SYSLIB0011

namespace _86BoxManager.Core
{
    public static class Serializer
    {
        private static readonly BinaryFormatter Bf = new();

        public static T Read<T>(byte[] array)
        {
            using var ms = new MemoryStream(array);
            var res = (T)Bf.Deserialize(ms);
            ms.Close();
            return res;
        }

        public static byte[] Write(object obj)
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            var data = ms.ToArray();
            return data;
        }
    }
}