using Prism.CommonDialogPack.Resources;
using Prism.CommonDialogPack.ViewModels;

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
        /// <summary>
        /// Set CurrentDirectoryPath.
        /// <br/>The path set by this method is used in Dialog with ExplorerBase.
        /// </summary>
        /// <param name="path"></param>
        public static void SetCurrentDirectoryPath(string path)
        {
            Settings.Default.CurrentDirectoryPath = path;
            Settings.Default.Save();
        }
        /// <summary>
        /// Enable DirectoryMemoization.
        /// </summary>
        public static void EnableDirectoryMemoization()
        {
            Settings.Default.EnableDirectoryMemoization = true;
            Settings.Default.Save();
        }
        /// <summary>
        /// Disable DirectoryMemoization.
        /// </summary>
        public static void DisableDirectoryMemoization()
        {
            Settings.Default.EnableDirectoryMemoization = false;
            Settings.Default.Save();
        }
        /// <summary>
        /// Enable CustomColorsStorage.
        /// </summary>
        public static void EnableCustomColorsStorage()
        {
            Settings.Default.EnableCustomColorsStorage = true;
            Settings.Default.Save();
        }
        /// <summary>
        /// Disable CustomColorsStorage.
        /// </summary>
        public static void DisableCustomColorsStorage()
        {
            Settings.Default.EnableCustomColorsStorage= false;
            Settings.Default.Save();
        }
        /// <summary>
        /// Get CustomColors.
        /// </summary>
        /// <returns></returns>
        public static RGBWithIndexViewModel[] GetCustomColors()
        {
            Settings settings = Settings.Default;
            return new RGBWithIndexViewModel[]
            {
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor01), 0),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor02), 1),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor03), 2),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor04), 3),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor05), 4),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor06), 5),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor07), 6),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor08), 7),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor09), 8),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor10), 9),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor11), 10),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor12), 11),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor13), 12),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor14), 13),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor15), 14),
                new RGBWithIndexViewModel(Converters.ColorConverter.StringToRGB(settings.CustomColor16), 15),
            };
        }
    }
}
