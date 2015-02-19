using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using OC = Oracle.Program;

namespace Oracle.Extensions
{
    internal static class AutoSpells
    {
        private static Menu _mainMenu, _menuConfig;
        private static readonly Obj_AI_Hero Me = ObjectManager.Player;

        public static void Initialize(Menu root)
        {
            Game.OnGameUpdate += Game_OnGameUpdate;

            _mainMenu = new Menu("自动使用技能", "asmenu");
            _menuConfig = new Menu("自动使用技能配置", "asconfig");

            foreach (var x in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsAlly))
                _menuConfig.AddItem(new MenuItem("ason" + x.SkinName, "Use for " + x.SkinName)).SetValue(true);
            _mainMenu.AddSubMenu(_menuConfig);

            // auto shields
            CreateMenuItem(95, "braume", "坚不可摧(布隆)", "braumshield", SpellSlot.E);
            CreateMenuItem(95, "dianaorbs", "苍白之瀑(皎月)", "dianashield", SpellSlot.W);
            CreateMenuItem(95, "galiobulwark", "坚强壁垒(哨兵之殇)", "galioshield", SpellSlot.W);
            CreateMenuItem(95, "garenw", "勇气(盖伦)", "garenshield", SpellSlot.W, false);
            CreateMenuItem(95, "eyeofthestorm", "风暴之眼(风女)", "jannashield", SpellSlot.E);
            CreateMenuItem(95, "karmasolkimshield", "鼓舞(天启者)", "karmashield", SpellSlot.E);
            CreateMenuItem(95, "lulue", "帮忙，皮克斯(璐璐)", "lulushield", SpellSlot.E);
            CreateMenuItem(95, "luxprismaticwave", "曲光屏障(光辉)", "luxshield", SpellSlot.W);
            CreateMenuItem(95, "nautiluspiercinggaze", "泰坦之怒(泰坦)", "nautshield", SpellSlot.W);
            CreateMenuItem(95, "orianaredactcommand", "指令：防卫(发条)", "oriannashield", SpellSlot.E);
            CreateMenuItem(95, "shenfeint", "奥义！空我(慎)", "shenshield", SpellSlot.W, false);
            CreateMenuItem(95, "moltenshield", "熔岩护盾(安妮)", "annieshield", SpellSlot.E);
            CreateMenuItem(95, "jarvanivgoldenaegis", "黄金圣盾(皇子)", "j4shield", SpellSlot.W);
            CreateMenuItem(95, "blindmonkwone", "金钟罩/铁布衫(瞎子)", "leeshield", SpellSlot.W, false);
            CreateMenuItem(95, "rivenfeint", "勇往直前(瑞文)", "rivenshield", SpellSlot.E, false);
            CreateMenuItem(95, "fiorariposte", "劳伦特心眼刀(剑姬)", "fiorashield", SpellSlot.W, false);
            CreateMenuItem(95, "rumbleshield", "法术护盾(轮子妈)", "rumbleshield", SpellSlot.W, false);
            CreateMenuItem(95, "sionw", "灵魂熔炉(塞恩)", "sionshield", SpellSlot.W);
            CreateMenuItem(95, "skarnerexoskeleton", "水晶蝎甲(蝎子)", "skarnershield", SpellSlot.W);
            CreateMenuItem(95, "urgotterrorcapacitoractive2", "恐怖电容(厄加特)", "urgotshield", SpellSlot.W);
            CreateMenuItem(95, "obduracy", "野蛮打击(熔岩)", "malphshield", SpellSlot.W);
            CreateMenuItem(95, "defensiveballcurl", "尖刺防御(龙龟)", "rammusshield", SpellSlot.W);

            // auto heals
            CreateMenuItem(80, "triumphantroar", "胜利怒吼(牛头)", "alistarheal", SpellSlot.E);
            CreateMenuItem(80, "primalsurge", "野性奔腾(豹女)", "nidaleeheal", SpellSlot.E);
            CreateMenuItem(80, "removescurvy", "坏血病疗法(船长)", "gangplankheal", SpellSlot.W);
            CreateMenuItem(80, "judicatordivineblessing", "神圣祝福(天使)", "kayleheal", SpellSlot.W);
            CreateMenuItem(80, "namie", "冲击之潮(娜美)", "namiheal", SpellSlot.W);
            CreateMenuItem(80, "sonaw", "坚毅咏叹调(琴女)", "sonaheal", SpellSlot.W);
            CreateMenuItem(80, "sorakaw", "星之灌注(索拉卡)", "sorakaheal", SpellSlot.W, false);
            CreateMenuItem(80, "Imbue", "神圣洗礼(宝石)", "taricheal", SpellSlot.Q);

            // auto ultimates
            CreateMenuItem(25, "lulur", "狂野生长(璐璐)", "luluult", SpellSlot.R, false);
            CreateMenuItem(25, "sadism", "背水一战（蒙多）", "drmundoult", SpellSlot.R, false);
            CreateMenuItem(15, "undyingrage", "无尽怒火(泰达米尔)", "tryndult", SpellSlot.R, false);
            CreateMenuItem(15, "chronoshift", "时光倒流（时光）", "zilult", SpellSlot.R, false);
            CreateMenuItem(15, "yorickreviveally", "死亡预兆（掘墓）", "yorickult", SpellSlot.R, false);
            CreateMenuItem(15, "kalistarx", "命运的召唤（滑板鞋）", "kalistault", SpellSlot.R, false);
            CreateMenuItem(15, "sorakar", "祈愿（星妈）", "sorakault", SpellSlot.R, false);

            // slow removers
            CreateMenuItem(0, "evelynnw", "暗黑狂暴(寡妇)", "eveslow", SpellSlot.W, false);
            CreateMenuItem(0, "garenq", "致命打击(盖伦)", "garenslow", SpellSlot.Q, false);
            CreateMenuItem(0, "highlander", "Highlander", "masteryislow", SpellSlot.R, false);

            // untargetable/evade spells           
            CreateMenuItem(0, "judicatorintervention", "神圣庇护(天使)", "teamkaylezhonya", SpellSlot.R, false);
            CreateMenuItem(0, "fioradance", "利刃华尔兹(剑姬)", "herofiorazhonya", SpellSlot.R, false);
            CreateMenuItem(0, "elisespidereinitial", "从天而降(蜘蛛)", "teamelisezhonya", SpellSlot.E, false);
            CreateMenuItem(0, "fizzjump", "古灵/精怪(小鱼人)", "teamfizzzhonyaCC", SpellSlot.E);
            CreateMenuItem(0, "lissandrar", "冰封陵墓(冰女)", "teamlissandrazhonya", SpellSlot.R, false);
            CreateMenuItem(0, "maokaiunstablegrowth", "扭曲突刺(树精)", "heromaokaizhonya", SpellSlot.W);
            CreateMenuItem(0, "alphastrike", "阿尔法突袭(剑圣)", "heromasteryizhonyaCC", SpellSlot.Q);
            CreateMenuItem(0, "blackshield", "黑暗之盾(莫甘娜)", "teammorganazhonyaCC", SpellSlot.E);
            CreateMenuItem(0, "hallucinatefull", "幻象(小丑)", "teamshacozhonya", SpellSlot.R, false);
            CreateMenuItem(0, "sivire", "法术护盾(轮子妈)", "teamsivirzhonyaCC", SpellSlot.E, false);
            CreateMenuItem(0, "vladimirsanguinepool", "血红之池(吸血鬼)", "teamvladimirzhonya", SpellSlot.W, false);
            CreateMenuItem(0, "zedult", "禁奥义!瞬狱影杀阵(劫)", "herozedzhonya", SpellSlot.R, false);
            CreateMenuItem(0, "nocturneshroudofdarkness", "黑暗庇护(梦魇)", "teamnocturnezhonyaCC", SpellSlot.W, false);

            root.AddSubMenu(_mainMenu);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            // prevent errors before spawning to the rift or dead
            if (!Me.IsValidTarget(300, false))
            {
                return;
            }

            // slow removals
            UseSpell("garenq", "garenslow", float.MaxValue, false);
            UseSpell("evelynnw", "eveslow", float.MaxValue, false);
            UseSpell("highlander", "masteryislow", float.MaxValue, false);

            // spell shields
            UseSpellShield("blackshield", "teammorganazhonyaCC", 600f);
            UseSpellShield("sivire", "teamsivirzhonyaCC", float.MaxValue, false);
            UseSpellShield("nocturneshroudofdarkness", "teamnocturnezhonyaCC", float.MaxValue, false);

            // auto heals
            UseSpell("triumphantroar", "alistarheal", 575f);
            UseSpell("primalsurge", "nidaleeheal", 600f);
            UseSpell("removescurvy", "gangplankheal");
            UseSpell("judicatordivineblessing", "kayleheal", 900f);
            UseSpell("namie", "namiheal", 725f);
            UseSpell("sonaw", "sonaheal", 1000f);
            UseSpell("sorakaw", "sorakaheal", 450f, false);
            UseSpell("imbue", "taricheal", 750f);

            if (!(OC.IncomeDamage >= 1))
            {
                return;
            }

            // untargable/evade spells            
            UseEvade("judicatorintervention", "teamkaylezhonya", 900f);
            UseEvade("fioradance", "herofiorazhonya", 300f);
            UseEvade("elisespidereinitial", "teamelisezhonya");
            UseEvade("fizzjump", "teamfizzzhonyaCC");
            UseEvade("lissandrar", "teamlissandrazhonya");
            UseEvade("maokaiunstablegrowth", "heromaokaizhonya", 525f);
            UseEvade("alphastrike", "heromasteryizhonyaCC", 600f);
            UseEvade("hallucinatefull", "teamshacozhonya");
            UseEvade("vladimirsanguinepool", "teamvladimirzhonya");
            UseEvade("zedult", "herozedzhonya", 625f);

            // auto shields
            UseSpell("braume", "braumshield");
            UseSpell("dianaorbs", "dianashield");
            UseSpell("galiobulwark", "galioshield", 800f);
            UseSpell("garenw", "garenshield", float.MaxValue, false);
            UseSpell("eyeofthestorm", "jannashield", 800f);
            UseSpell("karmasolkimshield", "karmashield", 800f);
            UseSpell("luxprismaticwave", "luxshield", 1075f);
            UseSpell("nautiluspiercinggaze", "nautilusshield");
            UseSpell("orianaredactcommand", "oriannashield", 1100f);
            UseSpell("shenfeint", "shenshield", float.MaxValue, false);
            UseSpell("jarvanivgoldenaegis", "jarvanivshield");
            UseSpell("blindmonkwone", "leeshield", 700f, false);
            UseSpell("rivenfeint", "rivenshield", float.MaxValue, false);
            UseSpell("rumbleshield", "rumbleshield");
            UseSpell("sionw", "sionshield");
            UseSpell("skarnerexoskeleton", "skarnershield");
            UseSpell("urgotterrorcapacitoractive2", "urgotshield");
            UseSpell("moltenshield", "annieshield");
            UseSpell("fiorariposte", "fiorashield", float.MaxValue, false);
            UseSpell("obduracy", "malphshield");
            UseSpell("defensiveballcurl", "rammusshield");

            // auto ults
            UseSpell("lulur", "luluult", 900f, false);
            UseSpell("undyingrage", "tryndult", float.MaxValue, false);
            UseSpell("chronoshift", "zilult", 900f, false);
            UseSpell("yorickreviveally", "yorickult", 900f, false);
            UseSpell("sadism", "drmundoult", float.MaxValue, false);

            // soraka global heal
            if (OC.ChampionName == "Soraka")
            {
                var sorakaslot = Me.GetSpellSlot("sorakar");
                var sorakar = new Spell(sorakaslot);
                if (!sorakar.IsReady())
                {
                    return;
                }

                if (sorakaslot == SpellSlot.Unknown && !_mainMenu.Item("usesorakault").GetValue<bool>())
                {
                    return;
                }

                var target =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .First(huro => huro.IsValidTarget(float.MaxValue, false) && huro.IsAlly);

                if (!_menuConfig.Item("ason" + target.SkinName).GetValue<bool>())
                {
                    return;
                }

                var aHealthPercent = target.Health/target.MaxHealth*100;
                if (aHealthPercent <= _mainMenu.Item("usesorakaultPct").GetValue<Slider>().Value)
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        sorakar.Cast();
                        OC.Logger(Program.LogType.Action,
                            "(Auto Spell: Ult) Saving ally target: " + target.SkinName + " (" + aHealthPercent + "%)");
                    }
                }
            }

            // kalista save soulbound
            if (OC.ChampionName != "Kalista")
            {
                return;
            }

            var slot = Me.GetSpellSlot("kalistarx");
            var kalistar = new Spell(slot, 1200f);
            if (!kalistar.IsReady())
            {
                return;
            }

            if (slot == SpellSlot.Unknown && !_mainMenu.Item("usekalistault").GetValue<bool>())
            {
                return;
            }

            var cooptarget =
                ObjectManager.Get<Obj_AI_Hero>()
                    .FirstOrDefault(hero => hero.HasBuff("kalistacoopstrikeally", true));
             
            if (cooptarget.IsValidTarget(1200, false) && cooptarget.IsAlly && 
                _menuConfig.Item("ason" + cooptarget.SkinName).GetValue<bool>())
            {
                var aHealthPercent = (int) ((cooptarget.Health/cooptarget.MaxHealth)*100);
                if (aHealthPercent <= _mainMenu.Item("usekalistaultPct").GetValue<Slider>().Value)
                {
                    if (OC.AggroTarget.NetworkId == cooptarget.NetworkId)
                    {
                        kalistar.Cast();
                        OC.Logger(Program.LogType.Action,
                            "Saving soulbound target: " + cooptarget.SkinName + " (" + aHealthPercent + "%)");
                    }
                }
            }
        }

        private static void UseSpellShield(string sname, string menuvar, float range = float.MaxValue, bool usemana = true)
        {
            if (!menuvar.Contains(OC.ChampionName.ToLower()))
            {
                return;
            }

            var slot = Me.GetSpellSlot(sname);
            if (slot != SpellSlot.Unknown && !_mainMenu.Item("use" + menuvar).GetValue<bool>())
            {
                return;
            }
          
            var spell = new Spell(slot, range);

            var target = range < 5000 ? OC.Friendly() : Me;
            if (target.Distance(Me.ServerPosition, true) > range * range)
            {
                return;
            }

            var aHealthPercent = target.Health/target.MaxHealth*100;
            if (!spell.IsReady() || !_menuConfig.Item("ason" + target.SkinName).GetValue<bool>() || Me.IsRecalling())
            {
                return;
            }

            if (_mainMenu.Item("use" + menuvar + "Ults").GetValue<bool>())
            {
                if (OC.DangerUlt && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        spell.CastOnUnit(target);
                        OC.Logger(Program.LogType.Action,
                            "(SCC) Casting " + spell.Slot + "(Dangerous Ult) on " + target.SkinName + " (" + aHealthPercent + "%)");
                    }
                }
            }

            if (_mainMenu.Item("use" + menuvar + "CC").GetValue<bool>())
            {
                if (OC.Dangercc && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        spell.CastOnUnit(target);
                        OC.Logger(Program.LogType.Action,
                            "(SCC) Casting " + spell.Slot + "(Dangerous CC) on " + target.SkinName +" (" + aHealthPercent + "%)");
                    }
                }
            }

            if (_mainMenu.Item("use" + menuvar + "Norm").GetValue<bool>())
            {
                 if (OC.Danger && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        spell.CastOnUnit(target);
                        OC.Logger(Program.LogType.Action,
                            "(SCC) Casting " + spell.Slot + "(Dangerous Spell) on " + target.SkinName + " (" + aHealthPercent + "%)");
                    }
                }               
            }

            if (_mainMenu.Item("use" + menuvar + "Any").GetValue<bool>())
            {
                if (OC.Spell && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        spell.CastOnUnit(target);
                        OC.Logger(Program.LogType.Action,
                            "(SCC) Casting " + spell.Slot + "(Any Spell) on " + target.SkinName + " (" + aHealthPercent + "%)");
                    }
                }
            }
        }


        private static void UseEvade(string sdataname, string menuvar, float range = float.MaxValue)
        {
            if (!menuvar.Contains(OC.ChampionName.ToLower()))
                return;

            var slot = Me.GetSpellSlot(sdataname);
            if (slot != SpellSlot.Unknown && !_mainMenu.Item("use" + menuvar).GetValue<bool>())
                return;
            
            var spell = new Spell(slot, range);

            var target = range < 5000 ? OC.Friendly() : Me;
            if (target.Distance(Me.ServerPosition, true) > range * range)
                return;

            var aHealthPercent = target.Health / target.MaxHealth * 100;
            if (!spell.IsReady() || !_menuConfig.Item("ason" + target.SkinName).GetValue<bool>() || Me.IsRecalling())
                return;

            if (_mainMenu.Item("use" + menuvar + "Norm").GetValue<bool>())
            {
                if ((OC.Danger || OC.IncomeDamage >= target.Health || target.Health/target.MaxHealth*100 <= 20) &&
                    menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        spell.CastOnUnit(target);
                        OC.Logger(Program.LogType.Action,
                            "(Evade) Casting " + spell.Slot + "(Dangerous Spell) on " + target.SkinName + " (" + aHealthPercent + "%)");
                    }
                }

                if ((OC.Danger || OC.IncomeDamage >= target.Health || target.Health/target.MaxHealth*100 <= 20) &&
                    menuvar.Contains("hero"))
                {
                    // +1 to allow for potential counterplay
                    if (target.CountHerosInRange(false) + 1 >= target.CountHerosInRange(true)) 
                    {
                        if (OC.AggroTarget.NetworkId == Me.NetworkId)
                        {
                            foreach (
                                var ene in
                                    ObjectManager.Get<Obj_AI_Hero>()
                                        .Where(x => x.IsValidTarget(range))
                                        .OrderByDescending(ene => ene.Health/ene.MaxHealth*100))
                            {
                                spell.CastOnUnit(ene);
                                OC.Logger(Program.LogType.Action, "(Evade) Casting " + spell.Slot + "(DS) on " + ene.SkinName);
                                OC.Logger(OC.LogType.Info, "Evade target info: ");
                                OC.Logger(OC.LogType.Info, "HP %: " + ene.Health/ene.MaxHealth*100);
                                OC.Logger(OC.LogType.Info, "Current HP %: " + ene.Health);
                            }
                        }
                    }
                }
            }

            if (_mainMenu.Item("use" + menuvar + "Ults").GetValue<bool>())
            {
                if ((OC.DangerUlt || OC.IncomeDamage >= target.Health || target.Health/target.MaxHealth*100 <= 18) &&
                    menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        spell.CastOnUnit(target);
                        OC.Logger(Program.LogType.Action,
                            "(Evade) Casting " + spell.Slot + "(DS) on " + target.SkinName + " (" + aHealthPercent + "%)");
                    }
                }

                if ((OC.DangerUlt || OC.IncomeDamage >= target.Health || target.Health/target.MaxHealth*100 <= 18) &&
                    menuvar.Contains("hero"))
                {
                    if (Me.CountHerosInRange(false) + 1 > Me.CountHerosInRange(true))
                    {
                        if (OC.AggroTarget.NetworkId == Me.NetworkId)
                        {
                            foreach (
                                var ene in
                                    ObjectManager.Get<Obj_AI_Hero>()
                                        .Where(x => x.IsValidTarget(range))
                                        .OrderByDescending(ene => ene.Health/ene.MaxHealth*100))
                            {
                                spell.CastOnUnit(ene);
                                OC.Logger(Program.LogType.Action, "(Evade) Casting " + spell.Slot + "(DS) on " + ene.SkinName);
                                OC.Logger(OC.LogType.Info, "Evade target info: ");
                                OC.Logger(OC.LogType.Info, "HP %: " + ene.Health / ene.MaxHealth * 100);
                                OC.Logger(OC.LogType.Info, "Current HP %: " + ene.Health);
                            }
                        }
                    }
                }
            }
        }

        private static void UseSpell(string sdataname, string menuvar, float range = float.MaxValue, bool usemana = true)
        {
            if (!menuvar.Contains(OC.ChampionName.ToLower()))
                return;
            
            var slot = Me.GetSpellSlot(sdataname);        
            if (slot != SpellSlot.Unknown && !_mainMenu.Item("use" + menuvar).GetValue<bool>())
                return;
         
            var spell = new Spell(slot, range);
            var target = range < 5000 ? OC.Friendly() : Me;

            if (target.Distance(Me.ServerPosition, true) > range*range)
                return;

            if (!spell.IsReady() || !_menuConfig.Item("ason" + target.SkinName).GetValue<bool>() ||
                Me.IsRecalling() ||  Me.InFountain())
            {
                return;
            }
         
            var manaPercent = (int) (Me.Mana/Me.MaxMana*100);
            var mHealthPercent = (int)(Me.Health / Me.MaxHealth * 100);
            var aHealthPercent = (int)((target.Health / target.MaxHealth) * 100);
            var iDamagePercent = (int)((OC.IncomeDamage / target.MaxHealth) * 100);

            if (menuvar.Contains("slow") && Me.HasBuffOfType(BuffType.Slow))
            {
                spell.Cast();
                OC.Logger(Program.LogType.Action,  "(Auto Spell: Slow) Im slowed, casting " + spell.Slot);
            }

            if (menuvar.Contains("slow")) 
                return;

            if (menuvar.Contains("shield") || menuvar.Contains("ult"))
            {
                
                if (aHealthPercent > _mainMenu.Item("use" + menuvar + "Pct").GetValue<Slider>().Value)
                    return;

                if (usemana && manaPercent <= _mainMenu.Item("use" + menuvar + "Mana").GetValue<Slider>().Value)
                    return;

                if (iDamagePercent >= 1 || OC.IncomeDamage >= target.Health)
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                    {
                        switch (menuvar)
                        {
                            case "rivenshield":
                                spell.Cast(Game.CursorPos);
                                OC.Logger(OC.LogType.Action,
                                    "(Auto Spell: Shield/Ult) Casting " + spell.Slot + " to game cursor! (Low HP)");
                                OC.Logger(OC.LogType.Action, "Target HP %: " + aHealthPercent);
                                break;
                            case "luxshield":
                                spell.Cast(target.IsMe ? Game.CursorPos : target.ServerPosition);
                                break;
                            default:
                                spell.CastOnUnit(target);
                                OC.Logger(OC.LogType.Action,
                                    "(Auto Spell: Shield/Ult) Casting " + spell.Slot + " on " + target.SkinName + " (Low HP)");
                                OC.Logger(OC.LogType.Action, "Target HP %: " + aHealthPercent);
                                break;
                        }
                    }
                }
            }

            else if (menuvar.Contains("heal"))
            {
                if (aHealthPercent > _mainMenu.Item("use" + menuvar + "Pct").GetValue<Slider>().Value)
                    return;

                if (menuvar.Contains("soraka"))   
                {
                    if (mHealthPercent <= _mainMenu.Item("useSorakaMana").GetValue<Slider>().Value || target.IsMe)
                        return;
                }

                if (usemana && manaPercent <= _mainMenu.Item("use" + menuvar + "Mana").GetValue<Slider>().Value)
                    return;

                if (OC.ChampionName == "Sona") 
                    spell.Cast(); 
                else
                {
                    spell.Cast(target);
                    OC.Logger(OC.LogType.Action, "(Auto Spell: Heal) Casting " + spell.Slot + " on " + target.SkinName + " (Low HP)");
                    OC.Logger(OC.LogType.Action, "Target HP %: " + aHealthPercent);
                }
            }

            if (!menuvar.Contains("zhonya"))
            {
                if (iDamagePercent >= _mainMenu.Item("use" + menuvar + "Dmg").GetValue<Slider>().Value)
                {
                    spell.Cast(target);
                    OC.Logger(OC.LogType.Action, "(SS) Casting " + spell.Slot + " on " + target.SkinName + " (Damage Chunk)");
                    OC.Logger(OC.LogType.Action, "Target HP %: " + aHealthPercent);
                }
            }

        }

        private static void CreateMenuItem(int dfv, string sdname, string name, string menuvar, SpellSlot slot, bool usemana = true)
        {
            var champslot = Me.GetSpellSlot(sdname.ToLower());
            if (champslot == SpellSlot.Unknown || champslot != SpellSlot.Unknown && champslot != slot)
            {
                return;
            }

            var menuName = new Menu(name + " | " + slot, menuvar);
            menuName.AddItem(new MenuItem("use" + menuvar, "Use " + name)).SetValue(true);

            if (!menuvar.Contains("zhonya"))
            {
                if (menuvar.Contains("slow"))
                    menuName.AddItem(new MenuItem("use" + menuvar + "Slow", "Remove slows").SetValue(true));

                if (!menuvar.Contains("slow"))
                    menuName.AddItem(new MenuItem("use" + menuvar + "Pct", "Use on HP %"))
                        .SetValue(new Slider(dfv, 1, 99));

                if (!menuvar.Contains("ult") && !menuvar.Contains("slow"))
                    menuName.AddItem(new MenuItem("use" + menuvar + "Dmg", "Use on Dmg dealt %"))
                        .SetValue(new Slider(45));

                if (menuvar.Contains("soraka"))
                    menuName.AddItem(new MenuItem("useSorakaMana", "Minimum HP % to use")).SetValue(new Slider(35));

                if (usemana)
                    menuName.AddItem(new MenuItem("use" + menuvar + "Mana", "Minimum mana % to use"))
                        .SetValue(new Slider(45));
            }

            if (menuvar.Contains("zhonya"))
            {
                if (menuvar.Contains("CC"))
                {
                    menuName.AddItem(new MenuItem("use" + menuvar + "Any", "Use on any Spell")).SetValue(false);
                    menuName.AddItem(new MenuItem("use" + menuvar + "CC", "Use on Crowd Control")).SetValue(true);
                }

                menuName.AddItem(new MenuItem("use" + menuvar + "Norm", "Use on Dangerous (Spells)")).SetValue(slot != SpellSlot.R);
                menuName.AddItem(new MenuItem("use" + menuvar + "Ults", "Use on Dangerous (Ultimates Only)")).SetValue(true);
            }

            _mainMenu.AddSubMenu(menuName);
        }
    }
}