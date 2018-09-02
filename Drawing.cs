using System;
using UnityEngine;

namespace hhax
{
    public static class Drawing
    {
        [Flags]
        public enum TextFlags
        {
            TEXT_FLAG_CENTERED = 1,
            TEXT_FLAG_OUTLINED = 2,
            TEXT_FLAG_DROPSHADOW = 3
        }

        private static Texture2D Texture2D;
        private static Color Texture2DColor;

        public static void DrawString(Vector2 pos, Color color, TextFlags flags, string text)
        {
            var center = (flags & TextFlags.TEXT_FLAG_CENTERED) == TextFlags.TEXT_FLAG_CENTERED;
            if ((flags & TextFlags.TEXT_FLAG_OUTLINED) == TextFlags.TEXT_FLAG_OUTLINED)
            {
                PrivateDrawString(pos + new Vector2(1f, 0f), Color.black, text, center);
                PrivateDrawString(pos + new Vector2(0f, 1f), Color.black, text, center);
                PrivateDrawString(pos + new Vector2(0f, -1f), Color.black, text, center);
            }
            if ((flags & TextFlags.TEXT_FLAG_DROPSHADOW) == TextFlags.TEXT_FLAG_DROPSHADOW) 
                PrivateDrawString(pos + new Vector2(1f, 1f), Color.black, text, center);
                PrivateDrawString(pos, color, text, center);
        }


        private static void PrivateDrawString(Vector2 pos, Color color, string text, bool center)
        {
            var style = new GUIStyle(GUI.skin.label) { normal = { textColor = color }, fontSize = 13 };
            GUI.Label(new Rect(pos.x, pos.y, 264f, 20f), text, style);
        }

        public static void DrawBox(Vector2 pos, Vector2 size, Color color)
        {
                Texture2D = new Texture2D(1, 1);
            if (color != Texture2DColor)
            {
                Texture2DColor = color;
            }
            GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), Texture2D);
        }

            public static void DrawEspWindow()
            {
                GUI.color = Color.red;
                GUI.Window(500000000, new Rect(40f, 100f, 380f, 500f), delegate
            {
                BaseSettings.GetSettings.EspSettings.DrawPlayers = GUI.Toggle        (new Rect(10f, 40f,  130f, 20f), BaseSettings.GetSettings.EspSettings.DrawPlayers, "Игроки");
                BaseSettings.GetSettings.EspSettings.DrawWrecks = GUI.Toggle         (new Rect(10f, 60f, 130f, 20f), BaseSettings.GetSettings.EspSettings.DrawWrecks, "Машины");
                BaseSettings.GetSettings.EspSettings.DrawOwnershipStakes = GUI.Toggle(new Rect(10f, 80f, 130f, 20f), BaseSettings.GetSettings.EspSettings.DrawOwnershipStakes, "Тотемы");
                BaseSettings.GetSettings.EspSettings.StorageLocker = GUI.Toggle      (new Rect(10f, 100f, 130f, 20f), BaseSettings.GetSettings.EspSettings.StorageLocker, "Ящики");
                BaseSettings.GetSettings.EspSettings.FirePit = GUI.Toggle            (new Rect(10f, 120f, 130f, 20f), BaseSettings.GetSettings.EspSettings.FirePit, "Жаровни");


                BaseSettings.GetSettings.EspSettings.LootCache = GUI.Toggle          (new Rect(10f, 140f, 130f, 20f), BaseSettings.GetSettings.EspSettings.LootCache, "Лут на рт");
                BaseSettings.GetSettings.EspSettings.LogResourceNode = GUI.Toggle    (new Rect(10f, 160f, 130f, 20f), BaseSettings.GetSettings.EspSettings.LogResourceNode, "Дерево");
                BaseSettings.GetSettings.EspSettings.FlintRock = GUI.Toggle          (new Rect(10f, 180f, 130f, 20f), BaseSettings.GetSettings.EspSettings.FlintRock, "Кремень");
                BaseSettings.GetSettings.EspSettings.Metal2Resource = GUI.Toggle     (new Rect(10f, 200f, 130f, 20f), BaseSettings.GetSettings.EspSettings.Metal2Resource, "Краснуха");
                BaseSettings.GetSettings.EspSettings.Metal4Resource = GUI.Toggle     (new Rect(10f, 220f, 130f, 20f), BaseSettings.GetSettings.EspSettings.Metal4Resource, "Синька");
                BaseSettings.GetSettings.EspSettings.Metal3Resource = GUI.Toggle     (new Rect(10f, 240f, 130f, 20f), BaseSettings.GetSettings.EspSettings.Metal3Resource, "Зеленка");
                BaseSettings.GetSettings.EspSettings.IronRockResource = GUI.Toggle   (new Rect(10f, 260f, 130f, 20f), BaseSettings.GetSettings.EspSettings.IronRockResource, "Железо");
                BaseSettings.GetSettings.EspSettings.CoalRockResource = GUI.Toggle   (new Rect(10f, 280f, 130f, 20f), BaseSettings.GetSettings.EspSettings.CoalRockResource, "Уголь");
                BaseSettings.GetSettings.EspSettings.SandstoneResource = GUI.Toggle  (new Rect(10f, 300f, 130f, 20f), BaseSettings.GetSettings.EspSettings.SandstoneResource, "Глина");
                BaseSettings.GetSettings.EspSettings.AIShigiForest = GUI.Toggle      (new Rect(10f, 320f, 130f, 20f), BaseSettings.GetSettings.EspSettings.AIShigiForest, "Олени");
                BaseSettings.GetSettings.EspSettings.AIBorProxy = GUI.Toggle         (new Rect(10f, 340f, 130f, 20f), BaseSettings.GetSettings.EspSettings.AIBorProxy, "Кабаны");
                BaseSettings.GetSettings.EspSettings.AIShigiProxy = GUI.Toggle       (new Rect(10f, 360f, 130f, 20f), BaseSettings.GetSettings.EspSettings.AIShigiProxy, "Зайцы");
                BaseSettings.GetSettings.EspSettings.AIYetiForest = GUI.Toggle       (new Rect(10f, 380f, 130f, 20f), BaseSettings.GetSettings.EspSettings.AIYetiForest, "Йети");
                BaseSettings.GetSettings.EspSettings.AITokarProxy = GUI.Toggle       (new Rect(10f, 400f, 130f, 20f), BaseSettings.GetSettings.EspSettings.AITokarProxy, "Токар");
                BaseSettings.GetSettings.EspSettings.WorkbenchDynamic = GUI.Toggle   (new Rect(10f, 420f, 130f, 20f), BaseSettings.GetSettings.EspSettings.WorkbenchDynamic, "Верстак");
                BaseSettings.GetSettings.EspSettings.c4Dynamic = GUI.Toggle          (new Rect(10f, 440f, 130f, 20f), BaseSettings.GetSettings.EspSettings.c4Dynamic, "Цешка");
                BaseSettings.GetSettings.EspSettings.SleeperLootCrate = GUI.Toggle   (new Rect(10f, 460f, 130f, 20f), BaseSettings.GetSettings.EspSettings.SleeperLootCrate, "Слиперы");

                // Aim
                BaseSettings.GetSettings.AimBotSettings.AimAtPlayers = GUI.Toggle    (new Rect(250f, 40f, 130f, 20f), BaseSettings.GetSettings.AimBotSettings.AimAtPlayers, "Игроки");

                // Text
                GUI.color = Color.green;;
                GUI.Label(new Rect(260f, 20f, 130f, 20f), "AimBot");

                GUI.color = Color.green;
                GUI.Label(new Rect(30f, 20f, 130f, 20f), "ESP");

                GUI.color = Color.red;
                GUI.Label(new Rect(10f, 480f, 130f, 20f), "Version 0.3");

            }, "From Russia with Love");

            }

        }

    }
