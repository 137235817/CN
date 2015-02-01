using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace KurisuMorgana
{
    internal class Program
    {
        private static Menu _menu;
        private static Spell _q, _w, _r;
        private static Orbwalking.Orbwalker _orbwalker;
        private static readonly Obj_AI_Hero Me = ObjectManager.Player;

        static void Main(string[] args)
        {
            Console.WriteLine("Morgana injected...");
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Me.ChampionName != "Morgana")
                return;

            // set spells
            _q = new Spell(SpellSlot.Q, 1175f);
            _q.SetSkillshot(0.25f, 72f, 1400f, true, SkillshotType.SkillshotLine);

            _w = new Spell(SpellSlot.W, 900f);
            _w.SetSkillshot(0.25f, 175f, 1200f, false, SkillshotType.SkillshotCircle);

            _r = new Spell(SpellSlot.R, 600f);

            _menu = new Menu("KurisuMorgana", "morgana", true);

            var orbmenu = new Menu("Orbwalker", "orbwalker");
            _orbwalker = new Orbwalking.Orbwalker(orbmenu);
            _menu.AddSubMenu(orbmenu);

            var tsmenu = new Menu("Selector", "selector");
            TargetSelector.AddToMenu(tsmenu);
            _menu.AddSubMenu(tsmenu);

            var drmenu = new Menu("Drawings", "drawings");
            drmenu.AddItem(new MenuItem("drawq", "Draw Q")).SetValue(true);
            drmenu.AddItem(new MenuItem("draww", "Draw W")).SetValue(true);
            drmenu.AddItem(new MenuItem("drawe", "Draw E")).SetValue(true);
            drmenu.AddItem(new MenuItem("drawr", "Draw R")).SetValue(true);
            _menu.AddSubMenu(drmenu);

            var spellmenu = new Menu("Spells", "spells");

            var menuQ = new Menu("Q Menu", "qmenu");
            menuQ.AddItem(new MenuItem("hitchanceq", "Binding Hitchance ")).SetValue(new Slider(3, 1, 4));
            menuQ.AddItem(new MenuItem("useqcombo", "Use in combo")).SetValue(true);
            menuQ.AddItem(new MenuItem("useharassq", "Use in harass")).SetValue(true);
            menuQ.AddItem(new MenuItem("useqanti", "Use on gapcloser")).SetValue(true);
            menuQ.AddItem(new MenuItem("useqauto", "Use on immobile")).SetValue(true);
            menuQ.AddItem(new MenuItem("useqdash", "Use on dashing")).SetValue(true);
            spellmenu.AddSubMenu(menuQ);

            var menuW = new Menu("W Menu", "wmenu");
            menuW.AddItem(new MenuItem("hitchancew", "Tormentsoil Hitchance ")).SetValue(new Slider(2, 1, 4));
            menuW.AddItem(new MenuItem("usewcombo", "Use in combo")).SetValue(true);
            menuW.AddItem(new MenuItem("useharassw", "Use in harass")).SetValue(true);       
            menuW.AddItem(new MenuItem("usewauto", "Use on immobile")).SetValue(true);
            menuW.AddItem(new MenuItem("waitfor", "Wait for bind or immobile")).SetValue(true);
            spellmenu.AddSubMenu(menuW);

            var menuR = new Menu("R Menu", "rmenu");
            menuR.AddItem(new MenuItem("usercombo", "Enable")).SetValue(true);
            //menuR.AddItem(new MenuItem("autor", "Use automatic if enemies >= ")).SetValue(new Slider(4, 2, 5));
            menuR.AddItem(new MenuItem("rcount", "Use in combo if enemies >= ")).SetValue(new Slider(2, 1, 5));
            menuR.AddItem(new MenuItem("ronlyif", "Use only if main target is immobile")).SetValue(true);
            spellmenu.AddSubMenu(menuR);

            spellmenu.AddItem(new MenuItem("harassmana", "Harass Mana %")).SetValue(new Slider(55, 0, 99));
            _menu.AddSubMenu(spellmenu);
            _menu.AddToMainMenu();

            Game.PrintChat("<font color=\"#AF7AFF\">KurisuMorgana</font> - Loaded");

            // events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!Me.IsValidTarget(300, false))
            {
                return;
            }

            Dashing(_menu.Item("useqdash").GetValue<bool>());

            Immobile(_menu.Item("useqauto").GetValue<bool>(),
                     _menu.Item("usewauto").GetValue<bool>());

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                Combo(_menu.Item("useqcombo").GetValue<bool>(),
                      _menu.Item("usewcombo").GetValue<bool>(), 
                      _menu.Item("usercombo").GetValue<bool>());
            }

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                Harass(_menu.Item("useqcombo").GetValue<bool>(),
                       _menu.Item("usewcombo").GetValue<bool>());
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Me.IsValidTarget(300, false))
            {
                Render.Circle.DrawCircle(Me.Position, Me.BoundingRadius - 50, System.Drawing.Color.White, 3);

                if (_menu.Item("drawq").GetValue<bool>())
                    Render.Circle.DrawCircle(Me.Position, _q.Range + 10, System.Drawing.Color.White, 3);
                if (_menu.Item("draww").GetValue<bool>())
                    Render.Circle.DrawCircle(Me.Position, _w.Range, System.Drawing.Color.White, 3);
                if (_menu.Item("drawe").GetValue<bool>())
                    Render.Circle.DrawCircle(Me.Position, 750f, System.Drawing.Color.White, 3);
                if (_menu.Item("drawr").GetValue<bool>())
                    Render.Circle.DrawCircle(Me.Position, _r.Range + 10, System.Drawing.Color.White, 3);

                var target = TargetSelector.GetTarget(_q.Range + 10, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(_q.Range + 10))
                    Render.Circle.DrawCircle(target.Position, target.BoundingRadius - 50, System.Drawing.Color.Yellow, 3);
            }
        }

        private static void Combo(bool useq, bool usew, bool user)
        {
            if (useq && _q.IsReady())
            {
                var qtarget = TargetSelector.GetTarget(_q.Range + 10, TargetSelector.DamageType.Magical);
                if (qtarget.IsValidTarget(_q.Range + 10))
                {
                    var poutput = _q.GetPrediction(qtarget);
                    if (poutput.Hitchance >= (HitChance) _menu.Item("hitchanceq").GetValue<Slider>().Value + 2)
                    {
                        _q.Cast(poutput.CastPosition);
                    }
                }
            }

            if (usew && _w.IsReady())
            {
                var wtarget = TargetSelector.GetTarget(_w.Range + 10, TargetSelector.DamageType.Magical);
                if (wtarget.IsValidTarget(_w.Range))
                {
                    var poutput = _w.GetPrediction(wtarget);
                    if (poutput.Hitchance >= (HitChance)_menu.Item("hitchancew").GetValue<Slider>().Value + 2)
                    {
                        if (!_menu.Item("waitfor").GetValue<bool>())
                            _w.Cast(poutput.CastPosition);
                    }                  
                }
            }

            if (user && _r.IsReady())
            {
                var rtarget = TargetSelector.GetTarget(_r.Range, TargetSelector.DamageType.Magical);
                if (rtarget.IsValidTarget(_r.Range))
                {
                    if (Me.CountEnemiesInRange(_r.Range) >= _menu.Item("rcount").GetValue<Slider>().Value)
                    {
                        if (_menu.Item("ronlyif").GetValue<bool>() && !rtarget.IsImmovable)
                        {
                            return;
                        }

                        _r.Cast();
                    }
                }
            }
        }

        private static void Harass(bool useq, bool usew)
        {
            if (useq && _q.IsReady())
            {
                var qtarget = TargetSelector.GetTarget(_q.Range - 300, TargetSelector.DamageType.Magical);
                if (qtarget.IsValidTarget(_q.Range - 300))
                {
                    var poutput = _q.GetPrediction(qtarget);
                    if (poutput.Hitchance >= (HitChance)_menu.Item("hitchanceq").GetValue<Slider>().Value + 2)
                    {
                        if ((int)(Me.Mana / Me.MaxMana * 100) >= _menu.Item("harassmana").GetValue<Slider>().Value)
                            _q.Cast(poutput.CastPosition);
                    }
                }
            }

            if (usew && _w.IsReady())
            {
                var wtarget = TargetSelector.GetTarget(_w.Range + 10, TargetSelector.DamageType.Magical);
                if (wtarget.IsValidTarget(_w.Range))
                {
                    var poutput = _w.GetPrediction(wtarget);
                    if (poutput.Hitchance >= (HitChance)_menu.Item("hitchancew").GetValue<Slider>().Value + 2)
                    {
                        if ((int)(Me.Mana / Me.MaxMana * 100) >= _menu.Item("harassmana").GetValue<Slider>().Value)
                            _w.Cast(poutput.CastPosition);
                    }
                }           
            }
        }

        private static void Dashing(bool useq)
        {
            if (useq && _q.IsReady())
            {
                var itarget = ObjectManager.Get<Obj_AI_Hero>()
                        .FirstOrDefault(h => h.Distance(Me.ServerPosition, true) <= _q.RangeSqr && h.IsEnemy);

                if (itarget.IsValidTarget())
                {
                    var poutput = _q.GetPrediction(itarget);
                    if (poutput.Hitchance == HitChance.Dashing)
                        _q.Cast(poutput.CastPosition);

                }
            }
        }

        private static void Immobile(bool useq, bool usew)
        {
            if (usew && _w.IsReady())
            {
                var itarget = ObjectManager.Get<Obj_AI_Hero>()
                    .FirstOrDefault(h => h.Distance(Me.ServerPosition, true) <= _w.RangeSqr && h.IsEnemy);

                if (itarget.IsValidTarget())
                {
                    var poutput = _w.GetPrediction(itarget);                  
                    if (poutput.Hitchance == HitChance.Immobile)
                        _w.Cast(poutput.CastPosition);
                }
            }

            if (useq && _q.IsReady())
            {
                var itarget = ObjectManager.Get<Obj_AI_Hero>()
                        .FirstOrDefault(h => h.Distance(Me.ServerPosition, true) <= _q.RangeSqr && h.IsEnemy);

                if (itarget.IsValidTarget())
                {
                    var poutput = _q.GetPrediction(itarget);
                    if (poutput.Hitchance == HitChance.Immobile)
                        _q.Cast(poutput.CastPosition);

                }
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsValidTarget(_q.Range + 10))
            {
                if (_menu.Item("useqanti").GetValue<bool>())
                {
                    var poutput = _q.GetPrediction(gapcloser.Sender);
                    if (poutput.Hitchance >= HitChance.Low)
                    {
                        _q.Cast(poutput.CastPosition);
                    }
                }
            }
        }
    }
}
