using Gtk;

namespace _86boxManager.Tools
{
    public static class ToolTips
    {
        public static void SetToolTip(this Widget widget, string text)
        {
            widget.TooltipText = text;
            widget.HasTooltip = true;
        }

        public static void UnsetToolTip(this Widget widget)
        {
            widget.TooltipText = string.Empty;
            widget.HasTooltip = false;
        }
    }
}