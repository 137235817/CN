using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace PerplexedEzreal
{
    class Config
    {
        public static Menu Settings = new Menu("[Perplexed]伊泽瑞尔", "menu", true);
        public static Orbwalking.Orbwalker Orbwalker;

        public static string[] Marksmen = { "Kalista", "Jinx", "Lucian", "Quinn", "Draven",  "Varus", "Graves", "Vayne", "Caitlyn",
                                                                    "Urgot", "Ezreal", "KogMaw", "Ashe", "MissFortune", "Tristana", "Teemo", "Sivir",
                                                                    "Twitch", "Corki"};

        public static void Initialize()
        {
            //Orbwalker
            Settings.AddSubMenu(new Menu("走砍", "orbMenu"));
            Orbwalker = new Orbwalking.Orbwalker(Settings.SubMenu("orbMenu"));
            //Target Selector
            Settings.AddSubMenu(new Menu("目标选择", "ts"));
            TargetSelector.AddToMenu(Settings.SubMenu("ts"));
            //Combo
            Settings.AddSubMenu(new Menu("连招", "menuCombo"));
            Settings.SubMenu("menuCombo").AddItem(new MenuItem("comboQ", "Q").SetValue(true));
            Settings.SubMenu("menuCombo").AddItem(new MenuItem("comboW", "W").SetValue(true));
            //Harass
            Settings.AddSubMenu(new Menu("骚扰", "menuHarass"));
            Settings.SubMenu("menuHarass").AddItem(new MenuItem( "harassQ", "Q").SetValue(true));
            Settings.SubMenu("menuHarass").AddItem(new MenuItem("harassW", "W").SetValue(true));
            //Last Hit
            Settings.AddSubMenu(new Menu("补刀设置", "menuLastHit"));
            Settings.SubMenu("menuLastHit").AddItem(new MenuItem("lastHitQ", "Q").SetValue(true));
            //Auto
            Settings.AddSubMenu(new Menu("自动", "menuAuto"));
            Settings.SubMenu("menuAuto").AddItem(new MenuItem("toggleAuto", "锁定自动").SetValue(new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle, true)));
            Settings.SubMenu("menuAuto").AddSubMenu(new Menu("英雄", "autoChamps"));
            foreach(Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValid && hero.IsEnemy))
                Settings.SubMenu("menuAuto").SubMenu("autoChamps").AddItem(new MenuItem("auto" + hero.ChampionName, hero.ChampionName).SetValue(Marksmen.Contains(hero.ChampionName)));
            Settings.SubMenu("menuAuto").AddItem(new MenuItem("autoQ", "Q").SetValue(true));
            Settings.SubMenu("menuAuto").AddItem(new MenuItem("autoW", "W").SetValue<bool>(false));
            Settings.SubMenu("menuAuto").AddItem(new MenuItem("manaER", "为 E/R留蓝").SetValue(true));
            Settings.SubMenu("menuAuto").AddItem(new MenuItem("autoTurret", "自动攻击塔下敌人").SetValue<bool>(false));
            //Ultimate
            Settings.AddSubMenu(new Menu("大招设置", "menuUlt"));
            Settings.SubMenu("menuUlt").AddItem(new MenuItem("ultLowest", "对血量最低目标使用大招").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            Settings.SubMenu("menuUlt").AddItem(new MenuItem("ks", "使用大招击杀").SetValue(true));
            Settings.SubMenu("menuUlt").AddItem(new MenuItem("ultRange", "大招范围").SetValue<Slider>(new Slider(1000, 1000, 5000)));
            //Summoners
            Settings.AddSubMenu(new Menu("召唤师技能", "menuSumms"));
            Settings.SubMenu("menuSumms").AddSubMenu(new Menu("治疗", "summHeal"));
            Settings.SubMenu("menuSumms").SubMenu("summHeal").AddItem(new MenuItem("useHeal", "启用").SetValue(true));
            Settings.SubMenu("menuSumms").SubMenu("summHeal").AddItem(new MenuItem("healPct", "血量低于%使用").SetValue(new Slider(35, 10, 90)));
            Settings.SubMenu("menuSumms").AddSubMenu(new Menu("点燃", "summIgnite"));
            Settings.SubMenu("menuSumms").SubMenu("summIgnite").AddItem(new MenuItem("useIgnite", "启用").SetValue(true));
            Settings.SubMenu("menuSumms").SubMenu("summIgnite").AddItem(new MenuItem("igniteMode", "使用引燃").SetValue(new StringList(new string[] { "总是", "连招" })));
            //Items
            Settings.AddSubMenu(new Menu("物品", "menuItems"));
            Settings.SubMenu("menuItems").AddSubMenu(new Menu("进攻", "offItems"));
            foreach (var offItem in ItemManager.Items.Where(item => item.Type == ItemType.Offensive))
                Settings.SubMenu("menuItems").SubMenu("offItems").AddItem(new MenuItem("use" + offItem.ShortName, offItem.Name).SetValue(true));
            Settings.SubMenu("menuItems").AddSubMenu(new Menu("保护", "defItems"));
            foreach (var defItem in ItemManager.Items.Where(item => item.Type == ItemType.Defensive))
            {
                Settings.SubMenu("menuItems").SubMenu("defItems").AddSubMenu(new Menu(defItem.Name, "menu" + defItem.ShortName));
                Settings.SubMenu("menuItems").SubMenu("defItems").SubMenu("menu" + defItem.ShortName).AddItem(new MenuItem("use" + defItem.ShortName, "Enable").SetValue(true));
                Settings.SubMenu("menuItems").SubMenu("defItems").SubMenu("menu" + defItem.ShortName).AddItem(new MenuItem("pctHealth" + defItem.ShortName, "Use On % Health").SetValue(new Slider(35, 10, 90)));
            }
            Settings.SubMenu("menuItems").AddSubMenu(new Menu("解控", "cleanseItems"));
            foreach (var cleanseItem in ItemManager.Items.Where(item => item.Type == ItemType.Cleanse))
                Settings.SubMenu("menuItems").SubMenu("cleanseItems").AddItem(new MenuItem("use" + cleanseItem.ShortName, cleanseItem.Name).SetValue(true));
            //Drawing
            Settings.AddSubMenu(new Menu("显示设置", "menuDrawing"));
            Settings.SubMenu("menuDrawing").AddSubMenu(new Menu("伤害指示器", "menuDamage"));
            Settings.SubMenu("menuDrawing").SubMenu("menuDamage").AddItem(new MenuItem("drawAADmg", "显示自动攻击伤害").SetValue(true));
            Settings.SubMenu("menuDrawing").SubMenu("menuDamage").AddItem(new MenuItem("drawQDmg", "显示Q伤害").SetValue(true));
            Settings.SubMenu("menuDrawing").SubMenu("menuDamage").AddItem(new MenuItem("drawWDmg", "显示W伤害").SetValue(true));
            Settings.SubMenu("menuDrawing").SubMenu("menuDamage").AddItem(new MenuItem("drawEDmg", "显示E伤害").SetValue(true));
            Settings.SubMenu("menuDrawing").SubMenu("menuDamage").AddItem(new MenuItem("drawRDmg", "显示R伤害").SetValue(true));
            Settings.SubMenu("menuDrawing").AddItem(new MenuItem("drawQ", "显示Q范围").SetValue(new Circle(true, Color.Yellow)));
            Settings.SubMenu("menuDrawing").AddItem(new MenuItem("drawW", "显示W范围").SetValue(new Circle(true, Color.Yellow)));
            Settings.SubMenu("menuDrawing").AddItem(new MenuItem("drawR", "显示R范围").SetValue(new Circle(true, Color.Yellow)));
            //Other
            Settings.AddItem(new MenuItem("dmgMode", "伤害模式").SetValue(new StringList(new string[] { "AD", "AP" })));
            Settings.AddItem(new MenuItem("recallBlock", "打断回城").SetValue(true));
            Settings.AddItem(new MenuItem("usePackets", "使用封包").SetValue(true));
            Settings.AddToMainMenu();
        }

        public static bool ComboQ { get { return Settings.Item("comboQ").GetValue<bool>(); } }
        public static bool ComboW { get { return Settings.Item("comboW").GetValue<bool>(); } }

        public static bool HarassQ { get { return Settings.Item("harassQ").GetValue<bool>(); } }
        public static bool HarassW { get { return Settings.Item("harassW").GetValue<bool>(); } }

        public static bool LastHitQ { get { return Settings.Item("lastHitQ").GetValue<bool>(); } }

        public static KeyBind UltLowest { get { return Settings.Item("ultLowest").GetValue<KeyBind>(); } }
        public static bool KillSteal { get { return Settings.Item("ks").GetValue<bool>(); } }
        public static int UltRange { get { return Settings.Item("ultRange").GetValue<Slider>().Value; } }

        public static KeyBind ToggleAuto { get { return Settings.Item("toggleAuto").GetValue<KeyBind>(); } }
        public static bool ShouldAuto(string championName)
        {
            return Settings.Item("auto" + championName).GetValue<bool>();
        }
        public static bool AutoQ { get { return Settings.Item("autoQ").GetValue<bool>(); } }
        public static bool AutoW { get { return Settings.Item("autoW").GetValue<bool>(); } }
        public static bool ManaER { get { return Settings.Item("manaER").GetValue<bool>(); } }
        public static bool AutoTurret { get { return Settings.Item("autoTurret").GetValue<bool>(); } }

        public static bool UseHeal { get { return Settings.Item("useHeal").GetValue<bool>(); } }
        public static int HealPct { get { return Settings.Item("healPct").GetValue<Slider>().Value; } }
        public static bool UseIgnite { get { return Settings.Item("useIgnite").GetValue<bool>(); } }
        public static string IgniteMode { get { return Settings.Item("igniteMode").GetValue<StringList>().SelectedValue; } }

        public static bool ShouldUseItem(string shortName)
        {
            return Settings.Item("use" + shortName).GetValue<bool>();
        }
        public static int UseOnPercent(string shortName)
        {
            return Settings.Item("pctHealth" + shortName).GetValue<Slider>().Value;
        }

        public static bool DrawAADmg { get { return Settings.Item("drawAADmg").GetValue<bool>(); } }
        public static bool DrawQDmg { get { return Settings.Item("drawQDmg").GetValue<bool>(); } }
        public static bool DrawWDmg { get { return Settings.Item("drawWDmg").GetValue<bool>(); } }
        public static bool DrawEDmg { get { return Settings.Item("drawEDmg").GetValue<bool>(); } }
        public static bool DrawRDmg { get { return Settings.Item("drawRDmg").GetValue<bool>(); } }
        public static bool DrawQ { get { return Settings.Item("drawQ").GetValue<Circle>().Active; } }
        public static bool DrawW { get { return Settings.Item("drawW").GetValue<Circle>().Active; } }
        public static bool DrawR { get { return Settings.Item("drawR").GetValue<Circle>().Active; } }

        public static string DamageMode { get { return Settings.Item("dmgMode").GetValue<StringList>().SelectedValue; } }
        public static bool RecallBlock { get { return Settings.Item("recallBlock").GetValue<bool>(); } }
        public static bool UsePackets { get { return Settings.Item("usePackets").GetValue<bool>(); } }
    }
}
