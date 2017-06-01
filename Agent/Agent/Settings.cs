﻿using Core;

namespace Agent
{
    public enum Themes
    {
        Light, Dark
    }

    #region ViewModel Setting

    /// <summary>
    ///     ViewModel для настроек.
    /// </summary>
    internal class MainSettings : SettingCore<MainSettings>
    {
        public Themes Theme { get; set; }
        public bool RandomBackground { get; set; } = true;
        public int BackgroundId { get; set; } = 1;
        public Directories Directories = new Directories();
        public Urls Urls = new Urls();
    }

    internal class Directories
    {
        public string Data { get; set; } = "Data";
        public string Temp { get; set; } = "Data/Temp";
    }

    internal class Urls
    {
        public string Game { get; set; } = "http://content.warframe.com/dynamic/worldState.php";
        public string News { get; set; } = "https://www.warframe.com/ru/news/get_posts?page=1&category=pc";
        public Filters Filters = new Filters();
    }

    internal class Filters
    {
        public string Items { get; set; } = "https://raw.githubusercontent.com/arrer/WarframeAgent/master/Filters/Items.json";
        public string Missions { get; set; } = "https://raw.githubusercontent.com/arrer/WarframeAgent/master/Filters/Missions.json";
        public string Planets { get; set; } = "https://raw.githubusercontent.com/arrer/WarframeAgent/master/Filters/Planets.json";
        public string Race { get; set; } = "https://raw.githubusercontent.com/arrer/WarframeAgent/master/Filters/Race.json";
        public string Sorties { get; set; } = "https://raw.githubusercontent.com/arrer/WarframeAgent/master/Filters/Sorties.json";
        public string Void { get; set; } = "https://github.com/arrer/WarframeAgent/blob/master/Filters/Void.json";
    }
    #endregion

    #region Settings class 

    /// <summary>
    ///     Класс настроек приложения
    /// </summary>
    internal static class Settings
    {
        /// <summary>
        ///     Настройки программы.
        /// </summary>
        public static MainSettings Program;

        /// <summary>
        ///     Выполняем загрузку всех необходимых настроек из файла/сервера.
        /// </summary>
        public static void Load()
        {
            Program = MainSettings.Load();
        }
    }

    #endregion
}
