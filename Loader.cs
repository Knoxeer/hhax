using UnityEngine;

namespace hhax
{
    public class Loader
    {
        public static bool HaxLoaded = false;

        public static GameObject LoadObject;

        public static void Load()
        {
            if (HaxLoaded) return;

            HaxLoaded = false;
            try
            {
                LoadObject = new GameObject();
                LoadObject.AddComponent<Menu>();
                Object.DontDestroyOnLoad(LoadObject);
                HaxLoaded = true;
            }
            catch { };
        }

        public static void Unload()
        {
            Object.DestroyImmediate(LoadObject);
        }
    }
}