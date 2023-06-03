using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RegistryValueKind = _86boxManager.Registry.ValueKind;

namespace _86boxManager.Registry
{
    public sealed class ConfigKey
    {
        private readonly JObject _content;
        private readonly Action<object> _closer;

        public ConfigKey(object o, Action<object> closer)
        {
            _content = (JObject)o;
            _closer = closer;
        }

        public void SetValue(string key, object value, RegistryValueKind kind)
        {
            if (kind == RegistryValueKind.String)
            {
                _content[key] = value.ToString();
                return;
            }
            if (kind == RegistryValueKind.DWord)
            {
                _content[key] = value is bool vb ? vb ? 1 : 0 : (int)value;
                return;
            }
            if (kind == RegistryValueKind.Binary)
            {
                _content[key] = (byte[])value;
                return;
            }
            throw new InvalidOperationException($"{key} => {kind}");
        }

        public object GetValue(string key)
        {
            var raw = _content[key];
            if (raw is JValue jv)
            {
                if (jv.Type == JTokenType.Integer)
                {
                    return jv.Value<int>();
                }
                return jv.Value;
            }
            return raw;
        }

        public T GetValue<T>(string key)
        {
            var raw = _content[key];
            if (raw == null)
                return default;
            var conv = raw.ToObject<T>();
            return conv;
        }

        private void Save()
        {
            _closer?.Invoke(_content);
        }

        public void Close()
        {
            Save();
        }

        public void DeleteValue(string key)
        {
            _content.Remove(key);
        }

        public IEnumerable<string> GetValueNames()
        {
            foreach (var pair in _content)
                yield return pair.Key;
        }
    }
}