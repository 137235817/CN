using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using Color = System.Drawing.Color;

namespace Kalista
{
    public class Config
    {
        private static bool initialized = false;
        private const string MENU_TITLE = "[Hellsing] " + "复仇之矛";

        private static MenuWrapper _menu;

        private static Dictionary<string, MenuWrapper.BoolLink> _boolLinks = new Dictionary<string, MenuWrapper.BoolLink>();
        private static Dictionary<string, MenuWrapper.CircleLink> _circleLinks = new Dictionary<string, MenuWrapper.CircleLink>();
        private static Dictionary<string, MenuWrapper.KeyBindLink> _keyLinks = new Dictionary<string, MenuWrapper.KeyBindLink>();
        private static Dictionary<string, MenuWrapper.SliderLink> _sliderLinks = new Dictionary<string, MenuWrapper.SliderLink>();

        public static MenuWrapper Menu { get { return _menu; } }

        public static Dictionary<string, MenuWrapper.BoolLink> BoolLinks { get { return _boolLinks; } }
        public static Dictionary<string, MenuWrapper.CircleLink> CircleLinks { get { return _circleLinks; } }
        public static Dictionary<string, MenuWrapper.KeyBindLink> KeyLinks { get { return _keyLinks; } }
        public static Dictionary<string, MenuWrapper.SliderLink> SliderLinks { get { return _sliderLinks; } }

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
            _menu = new MenuWrapper(MENU_TITLE);

            // Combo
            var subMenu = _menu.MainMenu.AddSubMenu("连招");
            ProcessLink("comboUseQ", subMenu.AddLinkedBool("使用Q"));
            ProcessLink("comboUseE", subMenu.AddLinkedBool("使用E"));
            ProcessLink("comboNumE", subMenu.AddLinkedSlider("E堆叠层数", 5, 1, 20));
            ProcessLink("comboUseItems", subMenu.AddLinkedBool("使用物品"));
            ProcessLink("comboUseIgnite", subMenu.AddLinkedBool("使用引燃"));
            ProcessLink("comboActive", subMenu.AddLinkedKeyBind("连招", 32, KeyBindType.Press));

            // Harass
            subMenu = _menu.MainMenu.AddSubMenu("骚扰");
            ProcessLink("harassUseQ", subMenu.AddLinkedBool("使用Q"));
            ProcessLink("harassMana", subMenu.AddLinkedSlider("耗蓝控制 (%)", 30));
            ProcessLink("harassActive", subMenu.AddLinkedKeyBind("骚扰", 'C', KeyBindType.Press));

            // WaveClear
            subMenu = _menu.MainMenu.AddSubMenu("清线");
            ProcessLink("waveUseQ", subMenu.AddLinkedBool("使用Q"));
            ProcessLink("waveNumQ", subMenu.AddLinkedSlider("Q|小兵数量", 3, 1, 10));
            ProcessLink("waveUseE", subMenu.AddLinkedBool("使用E"));
            ProcessLink("waveNumE", subMenu.AddLinkedSlider("E|小兵数量", 2, 1, 10));
            ProcessLink("waveMana", subMenu.AddLinkedSlider("耗蓝控制 (%)", 30));
            ProcessLink("waveActive", subMenu.AddLinkedKeyBind("清线", 'V', KeyBindType.Press));

            // JungleClear
            subMenu = _menu.MainMenu.AddSubMenu("清野");
            ProcessLink("jungleUseE", subMenu.AddLinkedBool("使用E"));
            ProcessLink("jungleActive", subMenu.AddLinkedKeyBind("清野", 'V', KeyBindType.Press));

            // Flee
            subMenu = _menu.MainMenu.AddSubMenu("逃跑");
            ProcessLink("fleeWalljump", subMenu.AddLinkedBool("尝试翻墙"));
            ProcessLink("fleeAA", subMenu.AddLinkedBool("平A智能走位"));
            ProcessLink("fleeActive", subMenu.AddLinkedKeyBind("逃跑", 'T', KeyBindType.Press));

            // Misc
            subMenu = _menu.MainMenu.AddSubMenu("其他");
            ProcessLink("miscKillstealE", subMenu.AddLinkedBool("使用E击杀"));
            ProcessLink("miscBigE", subMenu.AddLinkedBool("小兵数量多时使用E"));
            ProcessLink("miscUseR", subMenu.AddLinkedBool("使用大招拯救被控辅助"));

            // Spell settings
            subMenu = _menu.MainMenu.AddSubMenu("技能设置");
            ProcessLink("spellReductionE", subMenu.AddLinkedSlider("E伤害削弱", 20));

            // Items
            subMenu = _menu.MainMenu.AddSubMenu("物品");
            ProcessLink("itemsCutlass", subMenu.AddLinkedBool("使用比尔吉沃特弯刀"));
            ProcessLink("itemsBotrk", subMenu.AddLinkedBool("使用破败"));
            ProcessLink("itemsYoumuu", subMenu.AddLinkedBool("使用幽梦"));

            // Drawings
            subMenu = _menu.MainMenu.AddSubMenu("显示设置");
            ProcessLink("drawDamageE", subMenu.AddLinkedCircle("血条显示E伤害", true, Color.FromArgb(150, Color.Green), 0));
            ProcessLink("drawRangeQ", subMenu.AddLinkedCircle("Q范围", true, Color.FromArgb(150, Color.IndianRed), SpellManager.Q.Range));
            ProcessLink("drawRangeW", subMenu.AddLinkedCircle("W范围", true, Color.FromArgb(150, Color.MediumPurple), SpellManager.W.Range));
            ProcessLink("drawRangeEsmall", subMenu.AddLinkedCircle("E范围(离开时)", false, Color.FromArgb(150, Color.DarkRed), SpellManager.E.Range - 200));
            ProcessLink("drawRangeEactual", subMenu.AddLinkedCircle("E范围(实际)", true, Color.FromArgb(150, Color.DarkRed), SpellManager.E.Range));
            ProcessLink("drawRangeR", subMenu.AddLinkedCircle("R范围", false, Color.FromArgb(150, Color.Red), SpellManager.R.Range));
			
			//Translator
            subMenu = _menu.MainMenu.AddSubMenu("Vee汉化"); 
			
            Game.PrintChat("澶嶄粐涔嬬煕-鍔犺浇鎴愬姛   Vee姹夊寲!");
			
        }
    }
}
