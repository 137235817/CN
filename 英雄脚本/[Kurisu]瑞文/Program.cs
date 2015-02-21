using System;
using LeagueSharp.Common;

namespace KurisuRiven
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine("[Kurisu]瑞文加载成功  Vee汉化..");
            CustomEvents.Game.OnGameLoad += Base.Initialize;
        }
    }
}