using System;

namespace GeronimoGames.Kit.Utilities
{
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }

        public static string ToJson<T>(T[] array, bool prettyPrint = true)
        {
            var wrapper = new Wrapper<T> { items = array };
            return UnityEngine.JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}