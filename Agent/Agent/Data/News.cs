﻿using System.IO;
using Core.GameData;
using Newtonsoft.Json;

namespace Agent.Data
{
    /// <summary>
    ///     Взаимодействие с данными новостей.
    /// </summary>
    internal class News
    {
        private static NewsView Read(string fileName)
        {
            NewsView data;
            using (var file = File.OpenText(fileName))
            {
                var serializer = new JsonSerializer();
                data = (NewsView) serializer.Deserialize(file, typeof(NewsView));
            }

            return data;
        }

        /// <summary>
        ///     Основные данные новостей.
        /// </summary>
        public static NewsView Data;

        /// <summary>
        ///     Загружаем JSON файл с данными новостей.
        /// </summary>
        /// <param name="filename">Путь до JSON файла</param>
        public static void Load(string filename = "temp")
        {
            if (filename == "temp") filename = $"{Settings.Program.Directories.Temp}/NewsData.json";
            Data = Read(filename);
        }
    }
}