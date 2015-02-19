using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using Color = System.Drawing.Color;

namespace Gnar
{
    public class Config
    {
        public const string MENU_NAME = "[Hellsing] " + "迷失之牙";
        private static MenuWrapper _menu;

        private static Dictionary<string, MenuWrapper.BoolLink> _boolLinks = new Dictionary<string, MenuWrapper.BoolLink>();
        private static Dictionary<string, MenuWrapper.CircleLink> _circleLinks = new Dictionary<string, MenuWrapper.CircleLink>();
        private static Dictionary<string, MenuWrapper.KeyBindLink> _keyLinks = new Dictionary<string, MenuWrapper.KeyBindLink>();
        private static Dictionary<string, MenuWrapper.SliderLink> _sliderLinks = new Dictionary<string, MenuWrapper.SliderLink>();

        public static MenuWrapper Menu
        {
            get { return _menu; }
        }

        public static Dictionary<string, MenuWrapper.BoolLink> BoolLinks
        {
            get { return _boolLinks; }
        }
        public static Dictionary<string, MenuWrapper.CircleLink> CircleLinks
        {
            get { return _circleLinks; }
        }
        public static Dictionary<string, MenuWrapper.KeyBindLink> KeyLinks
        {
            get { return _keyLinks; }
        }
        public static Dictionary<string, MenuWrapper.SliderLink> SliderLinks
        {
            get { return _sliderLinks; }
        }

        private static void ProcessLink(string key, object value)
        {
            if (value is MenuWrapper.BoolLink)
                _boolLinks.Add(key, value as MenuWrapper.BoolLink);
            else if (value is MenuWrapper.CircleLink)
                _circleLinks.Add(key, value as MenuWrapper.CircleLink);
            else if (value is MenuWrapper.KeyBindLink)
                _keyLinks.Add(key, value as MenuWrapper.KeyBindLink);
            else if (value is MenuWrapper.SliderLink)
                _sliderLinks.Add(key, value as MenuWrapper.SliderLink);
        }

        static Config()
        {
            // Create menu
            _menu = new MenuWrapper(MENU_NAME);

            // ----- Combo
            var subMenu = _menu.MainMenu.AddSubMenu("连招");
            // Mini
            var subSubMenu = subMenu.AddSubMenu("纳尔");
            ProcessLink("comboUseQ", subSubMenu.AddLinkedBool("使用Q"));
            ProcessLink("comboUseE", subSubMenu.AddLinkedBool("使用E"));
            // Mega
            subSubMenu = subMenu.AddSubMenu("巨型纳尔");
            ProcessLink("comboUseQMega", subSubMenu.AddLinkedBool("使用Q"));
            ProcessLink("comboUseWMega", subSubMenu.AddLinkedBool("使用W"));
            ProcessLink("comboUseEMega", subSubMenu.AddLinkedBool("使用E"));
            ProcessLink("comboUseRMega", subSubMenu.AddLinkedBool("使用R"));
            // General
            ProcessLink("comboUseItems", subMenu.AddLinkedBool("使用物品"));
            ProcessLink("comboUseIgnite", subMenu.AddLinkedBool("使用引燃"));
            ProcessLink("comboActive", subMenu.AddLinkedKeyBind("连招", 32, KeyBindType.Press));


            // ----- Harass
            subMenu = _menu.MainMenu.AddSubMenu("骚扰");
            // Mini
            subSubMenu = subMenu.AddSubMenu("纳尔");
            ProcessLink("harassUseQ", subSubMenu.AddLinkedBool("使用Q"));
            // Mega
            subSubMenu = subMenu.AddSubMenu("巨型纳尔");
            ProcessLink("harassUseQMega", subSubMenu.AddLinkedBool("使用Q"));
            ProcessLink("harassUseWMega", subSubMenu.AddLinkedBool("使用W"));
            // General
            ProcessLink("harassActive", subMenu.AddLinkedKeyBind("骚扰", 'C', KeyBindType.Press));


            // ----- WaveClear
            subMenu = _menu.MainMenu.AddSubMenu("清线");
            // Mini
            subSubMenu = subMenu.AddSubMenu("纳尔");
            ProcessLink("waveUseQ", subSubMenu.AddLinkedBool("使用Q"));
            // Mega
            subSubMenu = subMenu.AddSubMenu("巨型纳尔");
            ProcessLink("waveUseQMega", subSubMenu.AddLinkedBool("使用Q"));
            ProcessLink("waveUseWMega", subSubMenu.AddLinkedBool("使用W"));
            ProcessLink("waveUseEMega", subSubMenu.AddLinkedBool("使用E"));
            // Gernal
            ProcessLink("waveUseItems", subMenu.AddLinkedBool("使用物品"));
            ProcessLink("waveActive", subMenu.AddLinkedKeyBind("清线", 'V', KeyBindType.Press));


            // ----- JungleClear
            subMenu = _menu.MainMenu.AddSubMenu("清野");
            // Mini
            subSubMenu = subMenu.AddSubMenu("纳尔");
            ProcessLink("jungleUseQ", subSubMenu.AddLinkedBool("Use Q"));
            // Mega
            subSubMenu = subMenu.AddSubMenu("巨型纳尔");
            ProcessLink("jungleUseQMega", subSubMenu.AddLinkedBool("使用Q"));
            ProcessLink("jungleUseWMega", subSubMenu.AddLinkedBool("使用W"));
            ProcessLink("jungleUseEMega", subSubMenu.AddLinkedBool("使用E"));
            // General
            ProcessLink("jungleUseItems", subMenu.AddLinkedBool("使用物品"));
            ProcessLink("jungleActive", subMenu.AddLinkedKeyBind("清野", 'V', KeyBindType.Press));

            // ----- Flee
            subMenu = _menu.MainMenu.AddSubMenu("逃跑");
            ProcessLink("fleeNothing", subMenu.AddLinkedBool("没有支持技能"));
            ProcessLink("fleeActive", subMenu.AddLinkedKeyBind("逃跑", 'T', KeyBindType.Press));

            // ----- Items
            subMenu = _menu.MainMenu.AddSubMenu("物品");
            ProcessLink("itemsTiamat", subMenu.AddLinkedBool("使用提亚马特"));
            ProcessLink("itemsHydra", subMenu.AddLinkedBool("使用贪欲九头蛇"));
            ProcessLink("itemsCutlass", subMenu.AddLinkedBool("使用比尔吉沃特完蛋"));
            ProcessLink("itemsBotrk", subMenu.AddLinkedBool("使用破败"));
            ProcessLink("itemsYoumuu", subMenu.AddLinkedBool("使用幽梦"));
            ProcessLink("itemsRanduin", subMenu.AddLinkedBool("使用奥戴恩的面纱"));
            ProcessLink("itemsFace", subMenu.AddLinkedBool("使用山岳之容"));

            // ----- Drawings
            subMenu = _menu.MainMenu.AddSubMenu("显示");
            // Mini
            subSubMenu = subMenu.AddSubMenu("纳尔");
            ProcessLink("drawRangeQ", subSubMenu.AddLinkedCircle("Q范围", true, Color.FromArgb(150, Color.IndianRed), SpellManager.QMini.Range));
            ProcessLink("drawRangeE", subSubMenu.AddLinkedCircle("E范围", true, Color.FromArgb(150, Color.Azure), SpellManager.EMini.Range));
            // Mega
            subSubMenu = subMenu.AddSubMenu("巨型纳尔");
            ProcessLink("drawRangeQMega", subSubMenu.AddLinkedCircle("Q范围", true, Color.FromArgb(150, Color.IndianRed), SpellManager.QMega.Range));
            ProcessLink("drawRangeWMega", subSubMenu.AddLinkedCircle("W范围", false, Color.FromArgb(150, Color.Azure), SpellManager.EMega.Range));
            ProcessLink("drawRangeEMega", subSubMenu.AddLinkedCircle("E范围", true, Color.FromArgb(150, Color.IndianRed), SpellManager.QMega.Range));
            ProcessLink("drawRangeRMega", subSubMenu.AddLinkedCircle("R范围", true, Color.FromArgb(150, Color.Azure), SpellManager.EMega.Range));
        }
    }
}
