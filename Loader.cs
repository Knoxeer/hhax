using UnityEngine;

namespace hhax
{
    public class Loader
    {
        public static GameObject LoadObject;

        public static void Load()
        {
            LoadObject = new GameObject();
            LoadObject.AddComponent<Menu>();
            Object.DontDestroyOnLoad(LoadObject);
        }

        public static void Unload()
        {
            Object.DestroyImmediate(LoadObject);
        }
    }
}