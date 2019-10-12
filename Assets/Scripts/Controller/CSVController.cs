using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Controller
{
    public class CSVController
    {
        /// <summary>
        /// リソース内にあるcsvファイルをロード
        /// 
        /// ファイルネームに.csvは不要
        /// ファイルパスはファイルネームを含まないこと
        /// 例："CSV/"のみなど
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string[]> LoadCSV(string fileName, string filePath = "")
        {
            List<string[]> csvDates = new List<string[]>();
            TextAsset csvFile = Resources.Load(filePath + fileName) as TextAsset;

            if (csvFile == null) return null;

            StringReader render = new StringReader(csvFile.text);

            while (render.Peek() > -1)
            {
                string line = render.ReadLine();
                csvDates.Add(line?.Split(','));
            }

            return csvDates;
        }
    }
}