using Prism.CommonDialogPack.Resources;

namespace Prism.CommonDialogPack
{
    public static class DialogSettings
    {
        static DialogSettings()
        {
            // Settings upgrade.
            if (!Settings.Default.IsUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsUpgrade = true;
                Settings.Default.Save();
            }
        }

        public static void SetCurrentDirectoryPath(string path)
        {
            Settings.Default.CurrentDirectoryPath = path;
            Settings.Default.Save();
        }

        public static void EnableDirectoryMemoization()
        {
            Settings.Default.EnableDirectoryMemoization = true;
            Settings.Default.Save();
        }

        public static void DisableDirectoryMemoization()
        {
            Settings.Default.EnableDirectoryMemoization = false;
            Settings.Default.Save();
        }
    }
}
