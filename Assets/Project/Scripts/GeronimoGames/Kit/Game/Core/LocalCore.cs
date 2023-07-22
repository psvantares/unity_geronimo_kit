using System.IO;
using GeronimoGames.Kit.Utilities;
using UnityEngine;

namespace GeronimoGames.Kit.Game
{
    public static class LocalCore
    {
        public static T[] GetDataFromJson<T>(string fileName)
        {
            var path = Application.streamingAssetsPath + "/" + fileName + ".json";
            var file = File.ReadAllText(path);
            var data = JsonHelper.FromJson<T>(file);
            return data;
        }

        public static T[] GetDataFromJsonFromPath<T>(string fileName, string pathFolder)
        {
            string realPath = null;

#if UNITY_EDITOR || UNITY_IOS
            realPath = Application.streamingAssetsPath + "/" + pathFolder + "/" + fileName + ".json";
#elif UNITY_ANDROID
            realPath = string.Format("{0}/{1}", Application.persistentDataPath, fileName + ".json");
            string originalPath = "jar:file://" + Application.dataPath + "!/assets/" + pathFolder + "/" + fileName + ".json";

            WWW reader = new WWW(originalPath);
            while ( ! reader.isDone) {}

            File.WriteAllBytes(realPath, reader.bytes);
#endif

            var file = File.ReadAllText(realPath);
            var data = JsonHelper.FromJson<T>(file);
            return data;
        }
    }
}