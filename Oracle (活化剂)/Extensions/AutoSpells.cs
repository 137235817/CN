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
            CreateMenuItem(95, "braume", "Unbreakable", "braumshield", SpellSlot.E);
            CreateMenuItem(95, "dianaorbs", "Pale Cascade", "dianashield", SpellSlot.W);
            CreateMenuItem(95, "galiobulwark", "Bulwark", "galioshield", SpellSlot.W);
            CreateMenuItem(95, "garenw", "Courage", "garenshield", SpellSlot.W, false);
            CreateMenuItem(95, "eyeofthestorm", "Eye of the Storm", "jannashield", SpellSlot.E);
            CreateMenuItem(95, "karmasolkimshield", "Inspire", "karmashield", SpellSlot.E);
            CreateMenuItem(95, "lulue", "Help Pix!", "lulushield", SpellSlot.E);
            CreateMenuItem(95, "luxprismaticwave", "Prismatic Barrier", "luxshield", SpellSlot.W);
            CreateMenuItem(95, "nautiluspiercinggaze", "Titans Wraith", "nautshield", SpellSlot.W);
            CreateMenuItem(95, "orianaredactcommand", "Command Protect", "oriannashield", SpellSlot.E);
            CreateMenuItem(95, "shenfeint", "Feint", "shenshield", SpellSlot.W, false);
            CreateMenuItem(95, "moltenshield", "Molten Shield", "annieshield", SpellSlot.E);
            CreateMenuItem(95, "jarvanivgoldenaegis", "Golden Aegis", "j4shield", SpellSlot.W);
            CreateMenuItem(95, "blindmonkwone", "Safegaurd", "leeshield", SpellSlot.W, false);
            CreateMenuItem(95, "rivenfeint", "Valor", "rivenshield", SpellSlot.E, false);
            CreateMenuItem(95, "fiorariposte", "Riposte", "fiorashield", SpellSlot.W, false);
            CreateMenuItem(95, "rumbleshield", "Scrap Shield", "rumbleshield", SpellSlot.W, false);
            CreateMenuItem(95, "sionw", "Soul Furnace", "sionshield", SpellSlot.W);
            CreateMenuItem(95, "skarnerexoskeleton", "Exoskeleton", "skarnershield", SpellSlot.W);
            CreateMenuItem(95, "urgotterrorcapacitoractive2", "Terror Capacitor", "urgotshield", SpellSlot.W);
            CreateMenuItem(95, "obduracy", "Brutal Strikes", "malphshield", SpellSlot.W);
            CreateMenuItem(95, "defensiveballcurl", "Defensive Ball Curl", "rammusshield", SpellSlot.W);

            // auto heals
            CreateMenuItem(80, "triumphantroar", "Triumphant Roar", "alistarheal", SpellSlot.E);
            CreateMenuItem(80, "primalsurge", "Primal Surge", "nidaleeheal", SpellSlot.E);
            CreateMenuItem(80, "removescurvy", "Remove Scurvy", "gangplankheal", SpellSlot.W);
            CreateMenuItem(80, "judicatordivineblessing", "Divine Blessing", "kayleheal", SpellSlot.W);
            CreateMenuItem(80, "namie", "Ebb and Flow", "namiheal", SpellSlot.W);
            CreateMenuItem(80, "sonaw", "Aria of Perseverance", "sonaheal", SpellSlot.W);
            CreateMenuItem(80, "sorakaw", "Astral Infusion", "sorakaheal", SpellSlot.W, false);
            CreateMenuItem(80, "Imbue", "Imbue", "taricheal", SpellSlot.Q);

            // auto ultimates
            CreateMenuItem(25, "lulur", "Wild Growth", "luluult", SpellSlot.R, false);
            CreateMenuItem(15, "undyingrage", "Undying Rage", "tryndult", SpellSlot.R, false);
            CreateMenuItem(15, "chronoshift", "Chorno Shift", "zilult", SpellSlot.R, false);
            CreateMenuItem(15, "yorickreviveally", "Omen of Death", "yorickult", SpellSlot.R, false);
            CreateMenuItem(15, "kalistarx", "Fate's Call", "kalistault", SpellSlot.R, false);

            // slow removers
            CreateMenuItem(0, "evelynnw", "Draw Frenzy", "eveslow", SpellSlot.W, false);
            CreateMenuItem(0, "garenq", "Decisive Strike", "garenslow", SpellSlot.Q, false);

            // untargetable/evade spells           
            CreateMenuItem(0, "judicatorintervention", "Intervention", "teamkaylezhonya", SpellSlot.R, false);
            CreateMenuItem(0, "fioradance", "Blade Waltz", "herofiorazhonya", SpellSlot.R, false);
            CreateMenuItem(0, "elisespidereinitial", "Rappel", "teamelisezhonya", SpellSlot.E, false);
            CreateMenuItem(0, "fizzjump", "Playful Trickster", "teamfizzzhonyaCC", SpellSlot.E);
            CreateMenuItem(0, "lissandrar", "Frozen Tomb", "teamlissandrazhonya", SpellSlot.R, false);
            CreateMenuItem(0, "maokaiunstablegrowth", "Unstabe Growth", "heromaokaizhonya", SpellSlot.W);
            CreateMenuItem(0, "alphastrike", "Alpha Strike", "heromasteryizhonyaCC", SpellSlot.Q);
            CreateMenuItem(0, "blackshield", "Black Shield", "teammorganazhonyaCC", SpellSlot.E);
            CreateMenuItem(0, "hallucinatefull", "Hallucinate", "teamshacozhonya", SpellSlot.R, false);
            CreateMenuItem(0, "sivire", "Spell Shield", "teamsivirzhonyaCC", SpellSlot.E, false);
            CreateMenuItem(0, "vladimirsanguinepool", "Sanguine Pool", "teamvladimirzhonya", SpellSlot.W, false);
            CreateMenuItem(0, "zedult", "Death Mark", "herozedzhonya", SpellSlot.R, false);
            CreateMenuItem(0, "nocturneshroudofdarkness", "Shroud of Darkness", "teamnocturnezhonyaCC", SpellSlot.W, false);

            root.AddSubMenu(_mainMenu);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!Me.IsValidTarget(300, false))
            {
                return;
            }

            // slow removals
            UseSpell("garenq", "garenslow", float.MaxValue, false);
            UseSpell("evelynnw", "eveslow", float.MaxValue, false);

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

            // untargable/evade spells            
            UseEvade("judicatorintervention", "teamkaylezhonya", 900f, false);
            UseEvade("fioradance", "herofiorazhonya", 300f, false);
            UseEvade("elisespidereinitial", "teamelisezhonya", float.MaxValue, false);
            UseEvade("fizzjump", "teamfizzzhonyaCC");
            UseEvade("lissandrar", "teamlissandrazhonya");
            UseEvade("maokaiunstablegrowth", "heromaokaizhonya", 525f);
            UseEvade("alphastrike", "heromasteryizhonyaCC", 600f);
            UseEvade("hallucinatefull", "teamshacozhonya", float.MaxValue, false);
            UseEvade("vladimirsanguinepool", "teamvladimirzhonya", float.MaxValue, false);
            UseEvade("zedult", "herozedzhonya", 625f, false);

            if (!(OC.IncomeDamage >= 1))
            {
                return;
            }

            // auto shields
            UseSpell("braume", "braumshield");
            UseSpell("dianaorbs", "dianashield");
            UseSpell("galiobulwark", "galioshield", 800f);
            UseSpell("garenw", "garenshield", float.MaxValue, false);
            UseSpell("eyeofthestorm", "jannashield", 800f);
            UseSpell("karmasolkimshield", "karmashield", 800f);
            UseSpell("luxprismaticwave", "luxshield", 1075f);
            UseSpell("nautiluspiercinggaze", "nautshield");
            UseSpell("orianaredactcommand", "oriannashield", 1100f);
            UseSpell("shenfeint", "shenshield", float.MaxValue, false);
            UseSpell("jarvanivgoldenaegis", "j4shield");
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

            // kalista save soulbound
            if (Me.ChampionName == OC.ChampionName)
            {
                var slot = Me.GetSpellSlot("kalistarx");
                if (slot != SpellSlot.Unknown && !_mainMenu.Item("usekalistault").GetValue<bool>())
                {
                    return;
                }

                var target =
                    ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(hero => hero.HasBuff("kalistacoopstrikeally", true));

                var kalistar = new Spell(slot, 1200f);
                if (target.IsValidTarget(kalistar.Range, false) && target.IsAlly)
                {
                    if (kalistar.IsReady() && _menuConfig.Item("ason" + target.SkinName).GetValue<bool>())
                    {
                        var aHealthPercent = (int)((target.Health / target.MaxHealth) * 100);
                        if (aHealthPercent <= _mainMenu.Item("usekalistaultpct").GetValue<Slider>().Value)
                        {
                            if (OC.AggroTarget.NetworkId == target.NetworkId)
                                kalistar.Cast();
                        }
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

            if (!spell.IsReady() || !_menuConfig.Item("ason" + target.SkinName).GetValue<bool>())
            {
                return;
            }


            if (_mainMenu.Item("use" + menuvar + "Ults").GetValue<bool>())
            {
                if (OC.DangerUlt && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                        spell.CastOnUnit(target);
                }
            }

            if (_mainMenu.Item("use" + menuvar + "CC").GetValue<bool>())
            {
                if (OC.DangerCC && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                        spell.CastOnUnit(target);
                }
            }

            if (_mainMenu.Item("use" + menuvar + "Norm").GetValue<bool>())
            {
                 if (OC.Danger && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                        spell.CastOnUnit(target);
                }               
            }

            if (_mainMenu.Item("use" + menuvar + "Any").GetValue<bool>())
            {
                if (OC.Spell && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                        spell.CastOnUnit(target);
                }
            }
        }


        private static void UseEvade(string sdataname, string menuvar, float range = float.MaxValue, bool usemana = true)
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

            if (!spell.IsReady() || !_menuConfig.Item("ason" + target.SkinName).GetValue<bool>())
                return;

            if (_mainMenu.Item("use" + menuvar + "Norm").GetValue<bool>())
            {
                if (OC.Danger && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                        spell.CastOnUnit(target);
                }

                if (OC.Danger && menuvar.Contains("hero"))
                {
                    if (Me.CountHerosInRange("hostile") > Me.CountHerosInRange("allies"))
                    {
                        if (OC.AggroTarget.NetworkId != Me.NetworkId)
                            return;

                        foreach (
                            var ene in
                                ObjectManager.Get<Obj_AI_Hero>()
                                    .Where(x => x.IsValidTarget(range))
                                    .OrderByDescending(ene => ene.Health / ene.MaxHealth * 100))
                        {
                            spell.CastOnUnit(ene);
                        }
                    }
                    else
                    {
                        // if its 1v1 cast on the sender
                        if (OC.AggroTarget.NetworkId == Me.NetworkId)
                            spell.CastOnUnit(OC.Attacker);
                    }
                }
            }

            if (_mainMenu.Item("use" + menuvar + "Ults").GetValue<bool>())
            {
                if (OC.DangerUlt && menuvar.Contains("team"))
                {
                    if (OC.AggroTarget.NetworkId == target.NetworkId)
                        spell.CastOnUnit(target);
                }

                if (OC.DangerUlt && menuvar.Contains("hero"))
                {
                    if (Me.CountHerosInRange("hostile") > Me.CountHerosInRange("allies"))
                    {
                        if (OC.AggroTarget.NetworkId != Me.NetworkId)
                            return;

                        foreach (
                            var ene in
                                ObjectManager.Get<Obj_AI_Hero>()
                                    .Where(x => x.IsValidTarget(range))
                                    .OrderByDescending(ene => ene.Health / ene.MaxHealth * 100))
                        {
                            spell.CastOnUnit(ene);
                        }
                    }
                    else
                    {
                        // if its 1v1 cast on the sender
                        if (OC.AggroTarget.NetworkId == Me.NetworkId)
                            spell.CastOnUnit(OC.Attacker);
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

            if (!spell.IsReady() || !_menuConfig.Item("ason" + target.SkinName).GetValue<bool>())
                return;
         
            var manaPercent = (int) (Me.Mana/Me.MaxMana*100);
            var mHealthPercent = (int)(Me.Health / Me.MaxHealth * 100);
            var aHealthPercent = (int)((target.Health / target.MaxHealth) * 100);
            var iDamagePercent = (int)((OC.IncomeDamage / target.MaxHealth) * 100);

            if (menuvar.Contains("slow") && Me.HasBuffOfType(BuffType.Slow))
                spell.Cast();

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
                    if (OC.AggroTarget.NetworkId != target.NetworkId)
                        return;

                    switch (menuvar)
                    {
                        case "rivenshield":
                            spell.Cast(Game.CursorPos);
                            break;
                        case "luxshield":
                            spell.Cast(target.IsMe ? Game.CursorPos : target.ServerPosition);
                            break;
                        default:
                            spell.CastOnUnit(target);
                            break;
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

                if (Me.ChampionName == "Sona") 
                    spell.Cast(); 
                else 
                    spell.Cast(target);
            }

            if (!menuvar.Contains("zhonya"))
            {
                if (iDamagePercent >= _mainMenu.Item("use" + menuvar + "Dmg").GetValue<Slider>().Value)
                    spell.Cast(target);
            }

        }

        private static void CreateMenuItem(int dfv, string sdname, string name, string menuvar, SpellSlot slot, bool usemana = true)
        {
            var champslot = Me.GetSpellSlot(sdname.ToLower());
            if (champslot == SpellSlot.Unknown || champslot != SpellSlot.Unknown && champslot != slot)
                return;

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