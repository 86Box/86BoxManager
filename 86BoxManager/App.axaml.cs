using System;
using _86boxManager.ViewModels;
using _86boxManager.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace _86boxManager
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new frmMain
                {
                    DataContext = new MainModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void open86BoxManagerToolStripMenuItem_Click(object sender, EventArgs e)
            => Program.Root.open86BoxManagerToolStripMenuItem_Click(sender, e);

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
            => Program.Root.settingsToolStripMenuItem_Click(sender, e);

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            => Program.Root.exitToolStripMenuItem_Click(sender, e);

        private void trayIcon_MouseClick(object sender, EventArgs e)
            => Program.Root.trayIcon_MouseClick(sender, e);
    }
}