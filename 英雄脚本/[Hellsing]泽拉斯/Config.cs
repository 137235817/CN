using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using Color = System.Drawing.Color;

namespace Xerath
{
    public class Config
    {
        public const string MENU_NAME = "[Hellsing] " + "泽拉斯";
        private static MenuWrapper _menu;

        private static Dictionary<string, MenuWrapper.BoolLink> _boolLinks = new Dictionary<string, MenuWrapper.BoolLink>();
        private static Dictionary<string, MenuWrapper.CircleLink> _circleLinks = new Dictionary<string, MenuWrapper.CircleLink>();
        private static Dictionary<string, MenuWrapper.KeyBindLink> _keyLinks = new Dictionary<string, MenuWrapper.KeyBindLink>();
        private static Dictionary<string, MenuWrapper.SliderLink> _sliderLinks = new Dictionary<string, MenuWrapper.SliderLink>();
        private static Dictionary<string, MenuWrapper.StringListLink> _stringListLinks = new Dictionary<string, MenuWrapper.StringListLink>();

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
        public static Dictionary<string, MenuWrapper.StringListLink> StringListLinks
        {
            get { return _stringListLinks; }
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
            else if (value is MenuWrapper.StringListLink)
                _stringListLinks.Add(key, value as MenuWrapper.StringListLink);
        }

        public static void Initialize()
        {
            // Create menu
            _menu = new MenuWrapper(MENU_NAME);

            // ----- Combo
            var subMenu = _menu.MainMenu.AddSubMenu("连招");
            var subSubMenu = subMenu.AddSubMenu("使用Q");
            ProcessLink("comboUseQ", subSubMenu.AddLinkedBool("启用"));
            ProcessLink("comboExtraRangeQ", subSubMenu.AddLinkedSlider("Q额外范围", 200, 0, 200));
            ProcessLink("comboUseW", subMenu.AddLinkedBool("使用W"));
            ProcessLink("comboUseE", subMenu.AddLinkedBool("使用E"));
            ProcessLink("comboUseR", subMenu.AddLinkedBool("使用R", false));
            //ProcessLink("comboUseItems", subMenu.AddLinkedBool("Use items"));
            //ProcessLink("comboUseIgnite", subMenu.AddLinkedBool("Use Ignite"));
            ProcessLink("comboActive", subMenu.AddLinkedKeyBind("连招", 32, KeyBindType.Press));

            // ----- Harass
            subMenu = _menu.MainMenu.AddSubMenu("骚扰");
            subSubMenu = subMenu.AddSubMenu("使用Q");
            ProcessLink("harassUseQ", subSubMenu.AddLinkedBool("启用"));
            ProcessLink("harassExtraRangeQ", subSubMenu.AddLinkedSlider("Q额外范围", 200, 0, 200));
            ProcessLink("harassUseW", subMenu.AddLinkedBool("使用W"));
            ProcessLink("harassUseE", subMenu.AddLinkedBool("使用E"));
            ProcessLink("harassMana", subMenu.AddLinkedSlider("耗蓝控制 (%)", 30));
            ProcessLink("harassActive", subMenu.AddLinkedKeyBind("骚扰", 'C', KeyBindType.Press));

            // ----- WaveClear
            subMenu = _menu.MainMenu.AddSubMenu("清线");
            ProcessLink("waveUseQ", subMenu.AddLinkedBool("使用Q"));
            ProcessLink("waveNumQ", subMenu.AddLinkedSlider("Q|小兵数量", 3, 1, 10));
            ProcessLink("waveUseW", subMenu.AddLinkedBool("使用W"));
            ProcessLink("waveNumW", subMenu.AddLinkedSlider("W|小兵数量", 3, 1, 10));
            ProcessLink("waveMana", subMenu.AddLinkedSlider("耗蓝控制 (%)", 30));
            ProcessLink("waveActive", subMenu.AddLinkedKeyBind("清线", 'V', KeyBindType.Press));

            // ----- JungleClear
            subMenu = _menu.MainMenu.AddSubMenu("清野");
            ProcessLink("jungleUseQ", subMenu.AddLinkedBool("使用Q"));
            ProcessLink("jungleUseW", subMenu.AddLinkedBool("使用W"));
            ProcessLink("jungleUseE", subMenu.AddLinkedBool("使用E"));
            ProcessLink("jungleActive", subMenu.AddLinkedKeyBind("清野", 'V', KeyBindType.Press));

            // ----- Flee
            subMenu = _menu.MainMenu.AddSubMenu("逃跑");
            ProcessLink("fleeNothing", subMenu.AddLinkedBool("无支持技能"));
            ProcessLink("fleeActive", subMenu.AddLinkedKeyBind("逃跑", 'T', KeyBindType.Press));

            // ----- Ultimate Settings
            subMenu = _menu.MainMenu.AddSubMenu("大招设置");
            ProcessLink("ultSettingsEnabled", subMenu.AddLinkedBool("启用"));
            ProcessLink("ultSettingsMode", subMenu.AddLinkedStringList("模式:", new[] { "智能选择目标", "脚本控制", "鼠标附近", "按键 (自动)", "按键 (鼠标附近)" }));
            ProcessLink("ultSettingsKeyPress", subMenu.AddLinkedKeyBind("按键控制", 'T', KeyBindType.Press));

            // ----- Items
            subMenu = _menu.MainMenu.AddSubMenu("物品");
            ProcessLink("itemsOrb", subMenu.AddLinkedBool("使用占卜宝珠 (饰品)"));

            // ----- Misc
            subMenu = _menu.MainMenu.AddSubMenu("其他");
            ProcessLink("miscGapcloseE", subMenu.AddLinkedBool("使用E防突进"));
            ProcessLink("miscInterruptE", subMenu.AddLinkedBool("使用E打断危险技能"));
            ProcessLink("miscAlerter", subMenu.AddLinkedBool("大招能击杀时聊天框提醒"));

            // ----- Single Spell Casting
            subMenu = _menu.MainMenu.AddSubMenu("单独技能施放");
            ProcessLink("castEnabled", subMenu.AddLinkedBool("启用"));
            ProcessLink("castW", subMenu.AddLinkedKeyBind("施放W", 'A', KeyBindType.Press));
            ProcessLink("castE", subMenu.AddLinkedKeyBind("施放E", 'S', KeyBindType.Press));

            // ----- Drawings
            subMenu = _menu.MainMenu.AddSubMenu("显示设置");
            ProcessLink("drawRangeQ", subMenu.AddLinkedCircle("Q范围", true, Color.FromArgb(150, Color.IndianRed), SpellManager.Q.Range));
            ProcessLink("drawRangeW", subMenu.AddLinkedCircle("W范围", true, Color.FromArgb(150, Color.PaleVioletRed), SpellManager.W.Range));
            ProcessLink("drawRangeE", subMenu.AddLinkedCircle("E范围", true, Color.FromArgb(150, Color.IndianRed), SpellManager.E.Range));
            ProcessLink("drawRangeR", subMenu.AddLinkedCircle("R范围", true, Color.FromArgb(150, Color.DarkRed), SpellManager.R.Range));
			
			//Translator
            subMenu = _menu.MainMenu.AddSubMenu("Vee汉化"); 
			
            Game.PrintChat("杩滃彜宸伒-鍔犺浇鎴愬姛   Vee姹夊寲!");			
        }
    }
}
