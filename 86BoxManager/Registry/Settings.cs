using static _86boxManager.Registry.ValueKind;

namespace _86boxManager.Registry
{
    public static class Settings
    {
        public static Setting Default { get; } = new();

        public sealed class Setting
        {
            private readonly ConfigKey _key;

            public Setting()
            {
                _key = Configs.Open86BoxKey(true);
            }

            private ConfigKey GetKey() => _key;

            public int NameColWidth
            {
                get => GetKey().GetValue<int?>(nameof(NameColWidth)) ?? 10;
                set => GetKey().SetValue(nameof(NameColWidth), value, DWord);
            }

            public int StatusColWidth
            {
                get => GetKey().GetValue<int?>(nameof(StatusColWidth)) ?? 10;
                set => GetKey().SetValue(nameof(StatusColWidth), value, DWord);
            }

            public int DescColWidth
            {
                get => GetKey().GetValue<int?>(nameof(DescColWidth)) ?? 10;
                set => GetKey().SetValue(nameof(DescColWidth), value, DWord);
            }

            public int PathColWidth
            {
                get => GetKey().GetValue<int?>(nameof(PathColWidth)) ?? 10;
                set => GetKey().SetValue(nameof(PathColWidth), value, DWord);
            }

            public void Save()
            {
                _key.Close();
            }
        }
    }
}