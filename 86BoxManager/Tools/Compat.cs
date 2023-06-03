using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace _86boxManager.Tools
{
    public static class Compat
    {
        public static bool IsActive(this ToggleButton toggle)
            => toggle.IsChecked == true;

        public static bool IsEditable(this TextBox box, bool value)
        {
            box.IsReadOnly = !value;
            return value;
        }
    }
}