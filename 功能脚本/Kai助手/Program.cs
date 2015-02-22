using System;
using System.Reflection;
using KaiHelper.Misc;
using KaiHelper.Tracker;
using LeagueSharp;
using LeagueSharp.Common;

namespace KaiHelper
{
    internal class Program
    {
        public static Menu MainMenu;

        private static void Main(string[] args)
        {
            MainMenu = new Menu("Kai助手", "KaiHelp", true);
            //Menu Tracker = MainMenu.AddSubMenu(new Menu("Tracker", "Tracker"));
            new SkillBar(MainMenu);
            new GankDetector(MainMenu);
            new WayPoint(MainMenu);
            new WardDetector(MainMenu);
            new HealthTurret(MainMenu);
            //Menu Timer = MainMenu.AddSubMenu(new Menu("Timer", "Timer"));
            //Menu Range = MainMenu.AddSubMenu(new Menu("Range", "Range"));
            new Vision(MainMenu);
            MainMenu.AddToMainMenu();
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            bool hasUpdate = Helper.HasNewVersion(Assembly.GetExecutingAssembly().GetName().Name);
            Game.PrintChat("Kai鍔╂墜 -鍔犺浇鎴愬姛  Vee姹夊寲");
        }
    }
}