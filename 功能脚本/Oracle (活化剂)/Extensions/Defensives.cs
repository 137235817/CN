using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using OC = Oracle.Program;

namespace Oracle.Extensions
{
    internal static class Defensives
    {
        private static Menu _mainMenu, _menuConfig;
        private static readonly Obj_AI_Hero Me = ObjectManager.Player;

        public static void Initialize(Menu root)
        {
            Game.OnGameUpdate += Game_OnGameUpdate;

            _mainMenu = new Menu("防御", "dmenu");
            _menuConfig = new Menu("防御配置", "dconfig");

            foreach (var x in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsAlly))
                _menuConfig.AddItem(new MenuItem("DefenseOn" + x.SkinName, "Use for " + x.SkinName)).SetValue(true);
            _mainMenu.AddSubMenu(_menuConfig);

            CreateMenuItem("兰顿之兆", "兰顿", "selfcount", 40, 40);
            CreateMenuItem("炽天使之翼", "大天使",  "selfhealth", 55, 40);
            CreateMenuItem("中亚沙漏", "中亚", "selfzhonya", 35, 40);
            CreateMenuItem("山岳之容", "山岳", "allyhealth", 20, 40);
            CreateMenuItem("钢铁烈阳之匣", "鸟盾", "allyhealth", 45, 40);

            var tMenu = new Menu("飞升护符", "tboost");
            tMenu.AddItem(new MenuItem("useTalisman", "使用")).SetValue(true);
            tMenu.AddItem(new MenuItem("useAllyPct", "友方血量低于%")).SetValue(new Slider(50, 1));
            tMenu.AddItem(new MenuItem("useEnemyPct", "敌方血量低于%")).SetValue(new Slider(50, 1));
            tMenu.AddItem(new MenuItem("talismanMode", "模式: ")).SetValue(new StringList(new[] {"总是使用", "连招"}));
            _mainMenu.AddSubMenu(tMenu);

            var bMenu = new Menu("号令之旗", "bannerc");
            bMenu.AddItem(new MenuItem("useBanner", "使用")).SetValue(true);
            _mainMenu.AddSubMenu(bMenu);

            CreateMenuItem("沃格勒特的巫师帽", "巫师帽", "selfzhonya", 35, 40);
            CreateMenuItem("奥黛恩的面纱", "面纱", "selfcount", 40, 40);

            var oMenu = new Menu("扫描透镜", "olens");
            oMenu.AddItem(new MenuItem("useOracles", "对隐形单位使用")).SetValue(true);
            oMenu.AddItem(new MenuItem("oracleMode", "模式: ")).SetValue(new StringList(new[] { "总是使用", "连招" }));
            _mainMenu.AddSubMenu(oMenu);

            root.AddSubMenu(_mainMenu);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!Me.IsValidTarget(300, false))
            {
                return;
            }

            UseItemCount("Odyns", 3180, 450f);
            UseItemCount("Randuins", 3143, 450f);

            if (OC.IncomeDamage >= 1)
            {
                UseItem("allyshieldlocket", "Locket", 3190, 600f);
                UseItem("selfshieldseraph", "Seraphs", 3040);
                UseItem("selfshieldzhonya", "Zhonyas", 3157);
                UseItem("allyshieldmountain", "Mountain", 3401, 700f);
                UseItem("selfshieldzhonya", "Wooglets", 3090);
            }

            // Oracle's Lens 
            if (Items.HasItem(3364) && Items.CanUseItem(3364) && _mainMenu.Item("useOracles").GetValue<bool>())
            {
                if (OC.Origin.Item("usecombo").GetValue<KeyBind>().Active ||
                    _mainMenu.Item("oracleMode").GetValue<StringList>().SelectedIndex != 1)
                {
                    var target = OC.Friendly();
                    if (target.Distance(Me.ServerPosition, true) <= 600*600 && OC.Stealth ||
                        target.HasBuff("RengarRBuff", true))
                    {
                        Items.UseItem(3364, target.ServerPosition);
                        OC.Logger(OC.LogType.Action, "Using oracle's lens near " + target.SkinName + " (stealth)");
                    }
                }
            }

            // Banner of command (basic)
            if (Items.HasItem(3060) && Items.CanUseItem(3060) && _mainMenu.Item("useBanner").GetValue<bool>())
            {
                var minionList = MinionManager.GetMinions(Me.Position, 1000);

                foreach (
                    var minyone in 
                        minionList.Where(minion => minion.IsValidTarget(1000) && minion.BaseSkinName.Contains("MechCannon")))
                {
                    if (minyone.Health > minyone.Health/minyone.MaxHealth*50)
                    {
                        Items.UseItem(3060, minyone);
                        OC.Logger(OC.LogType.Action, "Using banner of command item on MechCannon!");
                    }
                }
            }

            // Talisman of Ascension
            if (Items.HasItem(3069) && Items.CanUseItem(3069) && _mainMenu.Item("useTalisman").GetValue<bool>())
            {
                if (!OC.Origin.Item("usecombo").GetValue<KeyBind>().Active &&
                    _mainMenu.Item("talismanMode").GetValue<StringList>().SelectedIndex == 1)
                {
                    return;
                }

                var target = OC.Friendly();
                if (target.Distance(Me.ServerPosition, true) > 600*600)
                {
                    return;
                }

                var lowTarget =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .OrderBy(ex => ex.Health/ex.MaxHealth*100)
                        .First(x => x.IsValidTarget(1000));

                var aHealthPercent = target.Health/target.MaxHealth*100;
                var eHealthPercent = lowTarget.Health/lowTarget.MaxHealth*100;

                if (lowTarget.Distance(target.ServerPosition, true) <= 900*900 &&
                    (target.CountHerosInRange(false) > target.CountHerosInRange(true) &&
                     eHealthPercent <= _mainMenu.Item("useEnemyPct").GetValue<Slider>().Value))
                {
                    Items.UseItem(3069);
                    OC.Logger(OC.LogType.Action, "Using speed item on enemy " + lowTarget.SkinName + " (" +
                                                 lowTarget.Health/lowTarget.MaxHealth*100 + "%) is low!");
                }

                if (target.CountHerosInRange(false) > target.CountHerosInRange(true) &&
                    aHealthPercent <= _mainMenu.Item("useAllyPct").GetValue<Slider>().Value)
                {
                    Items.UseItem(3069);
                    OC.Logger(OC.LogType.Action,
                        "Using speed item on ally " + target.SkinName + " (" + aHealthPercent + "%) is low!");
                }
            }
        }

        private static void UseItemCount(string name, int itemId, float itemRange)
        {
            if (!Items.HasItem(itemId) || !Items.CanUseItem(itemId))
            {
                return;
            }

            if (_mainMenu.Item("use" + name).GetValue<bool>())
            {
                if (Me.CountHerosInRange(true, itemRange) >=
                    _mainMenu.Item("use" + name + "Count").GetValue<Slider>().Value)
                {
                    Items.UseItem(itemId);
                    OC.Logger(OC.LogType.Action, "Used " + name + " on me ! (Item count)");
                }
            }
        }

        private static void UseItem(string menuvar, string name, int itemId, float itemRange = float.MaxValue)
        {
            if (!Items.HasItem(itemId) || !Items.CanUseItem(itemId))
                return;

            if (!_mainMenu.Item("use" + name).GetValue<bool>())
                return;

            var target = itemRange > 5000 ? Me : OC.Friendly();
            if (target.Distance(Me.ServerPosition, true) > itemRange*itemRange)
            {
                return;
            }
            
            var aHealthPercent = (int) ((target.Health/target.MaxHealth)*100);
            var iDamagePercent = (int) (OC.IncomeDamage/target.MaxHealth*100);

            if (!_mainMenu.Item("DefenseOn" + target.SkinName).GetValue<bool>())
            {
                return;
            }

            if (target.CountHerosInRange(false) + 1 >= target.CountHerosInRange(true)) // +1 to allow potential counterplay
            {
                if (_mainMenu.Item("use" + name + "Ults").GetValue<bool>())
                {
                    if (OC.DangerUlt || OC.IncomeDamage >= target.Health || target.Health/target.MaxHealth*100 <= 15)
                    {
                        if (OC.AggroTarget.NetworkId == target.NetworkId)
                        {
                            Items.UseItem(itemId, target);
                            OC.Logger(OC.LogType.Action,
                                "Used " + name + " on " + target.SkinName + " (" + aHealthPercent + "%)! (Dangerous Ult)");
                        }
                    }
                }

                if (_mainMenu.Item("use" + name + "Zhy").GetValue<bool>())
                {
                    if (OC.Danger || OC.IncomeDamage >= target.Health || target.Health/target.MaxHealth*100 <= 15)
                    {
                        if (OC.AggroTarget.NetworkId == target.NetworkId)
                        {
                            Items.UseItem(itemId, target);
                            OC.Logger(OC.LogType.Action,
                                "Used " + name + " on " + target.SkinName + " (" + aHealthPercent + "%)! (Dangerous Spell)");
                        }
                    }
                }
            }

            if (menuvar.Contains("shield"))
            {
                if (menuvar.Contains("zhonya"))
                {
                    if (_mainMenu.Item("use" + name + "Only").GetValue<bool>() &&
                        !(target.Health/target.MaxHealth*100 <= 20))
                    {
                        return;
                    }
                }
                if (aHealthPercent <= _mainMenu.Item("use" + name + "Pct").GetValue<Slider>().Value)
                {
                    if ((iDamagePercent >= 1 || OC.IncomeDamage >= target.Health))
                    {
                        if (OC.AggroTarget.NetworkId == target.NetworkId)
                        {
                            Items.UseItem(itemId, target);
                            OC.Logger(OC.LogType.Action,
                                "Used " + name + " on " + target.SkinName + " (" + aHealthPercent + "%)! (Low HP)");
                        }
                    }

                    if (iDamagePercent >= _mainMenu.Item("use" + name + "Dmg").GetValue<Slider>().Value)
                    {
                        if (OC.AggroTarget.NetworkId == target.NetworkId)
                        {
                            Items.UseItem(itemId, target);
                            OC.Logger(OC.LogType.Action,
                                "Used " + name + " on " + target.SkinName + " (" + aHealthPercent + "%)! (Damage Chunk)");
                        }
                    }                    
                }
            }
        }

        private static void CreateMenuItem(string displayname, string name, string type, int hpvalue, int dmgvalue)
        {
            var menuName = new Menu(name, name.ToLower());
            menuName.AddItem(new MenuItem("use" + name, "Use " + displayname)).SetValue(true);

            if (!type.Contains("count"))
            {
                menuName.AddItem(new MenuItem("use" + name + "Pct", "生命值低于%使用")).SetValue(new Slider(hpvalue));
                menuName.AddItem(new MenuItem("use" + name + "Dmg", "伤害超过%使用")).SetValue(new Slider(dmgvalue));
            }

            if (type.Contains("count"))
                menuName.AddItem(new MenuItem("use" + name + "Count", "使用数量")).SetValue(new Slider(3, 1, 5));

            if (!type.Contains("count"))
            {
                menuName.AddItem(new MenuItem("use" + name + "Zhy", "危险时使用(所有技能)")).SetValue(false);
                menuName.AddItem(new MenuItem("use" + name + "Ults", "危险时使用(大招)")).SetValue(true);

                if (type.Contains("zhonya"))
                    menuName.AddItem(new MenuItem("use" + name + "Only", "仅在危险时使用")).SetValue(true);
            }
  
            _mainMenu.AddSubMenu(menuName);
        }      
    }
}