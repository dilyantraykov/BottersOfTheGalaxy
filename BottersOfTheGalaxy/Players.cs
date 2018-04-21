using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottersOfTheGalaxy
{
    public static class Constants
    {
        public const int MapWidth = 1920;
        public const int MapHeight = 750;
        public const int UnitSpawnPeriod = 15;

        public const string UnitString = "UNIT";
        public const string HeroString = "HERO";
        public const string TowerString = "TOWER";
        public const string GrootString = "GROOT";

        public const string DeadpoolString = "DEADPOOL";
        public const string DoctorStrangeString = "DOCTOR_STRANGE";
        public const string HulkString = "HULK";
        public const string IronmanString = "IRONMAN";
        public const string ValkyrieString = "VALKYRIE";

        public const string WaitCommand = "WAIT";
        public const string MoveCommand = "MOVE";
        public const string AttackCommand = "ATTACK";
        public const string AttackNearestCommand = "ATTACK_NEAREST";
        public const string MoveAttackCommand = "MOVE_ATTACK";
        public const string BuyCommand = "BUY";
        public const string SellCommand = "SELL";

        public const int Hero0PositionX = 200;
        public const int Hero0PositionY = 590;
        public const int Hero1PositionX = 1720;
        public const int Hero1PositionY = 590;

        public const double HeroAttackTime = 0.1;
        public const double UnitAttackTime = 0.2;
        public const double AttackRangeLimit = 150;
        public const int AggroDistance = 300;
        public const int AggroRounds = 3;

        public const int MeleeUnitGoldReward = 30;
        public const int RangedUnitGoldReward = 50;
        public const int HeroGoldReward = 300;
        public const int DeniedHeroGoldReward = 300;
        public const int GrootGoldReward = 300;

        public const int IsVisibleValue = 1;
        public const int IsPotionValue = 1;

        public const int MaxItemCountPerHero = 4;

        public const int AoeHealManaCost = 50;
        public const string AoeHealCommand = "AOEHEAL";
        public const int AoeHealRange = 250;

        public const int BurningManaCost = 50;
        public const int BurningRange = 250;
        public const string BurningCommand = "BURNING";

        public const int FireBallManaCost = 60;
        public const string FireBallCommand = "FIREBALL";
        public const int FireBallRange = 900;

        public const int PullManaCost = 40;
        public const int PullRange = 300;
        public const string PullCommand = "PULL";

        public const int ShieldManaCost = 40;
        public const int ShieldRange = 500;
        public const string ShieldCommand = "SHIELD";
    }

    public static class Utils
    {
        public static double GetTimeToMove(double distance, int moveSpeed)
        {
            var time = distance / moveSpeed;
            return time;
        }

        internal static UnitType GetUnitTypeFromString(string unitType)
        {
            switch (unitType)
            {
                case Constants.UnitString:
                    return UnitType.Unit;
                case Constants.HeroString:
                    return UnitType.Hero;
                case Constants.TowerString:
                    return UnitType.Tower;
                default:
                    return UnitType.Groot;
            }
        }

        internal static HeroType GetHeroTypeFromString(string heroType)
        {
            switch (heroType)
            {
                case Constants.DeadpoolString:
                    return HeroType.Deadpool;
                case Constants.DoctorStrangeString:
                    return HeroType.DoctorStrange;
                case Constants.HulkString:
                    return HeroType.Hulk;
                case Constants.ValkyrieString:
                    return HeroType.Valkyrie;
                default:
                    return HeroType.Ironman;
            }
        }

        internal static double CalculateFireBallDamage(int mana, double distance)
        {
            var damage = mana * 0.2 + 55 * distance / 1000;
            return damage;
        }
    }

    public static class MathUtils
    {
        public static double GetDistance(Entity a, Entity b)
        {
            return GetDistance(a.Position, b.Position);
        }

        public static double GetDistance(Position a, Position b)
        {
            var distance = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            return distance;
        }

        public static Position GetPointAlongLine(Position p2, Position p1, double distance)
        {
            Position vector = new Position(p2.X - p1.X, p2.Y - p1.Y);
            double c = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            double a = distance / c;

            var newX = (int) (p1.X + vector.X * a);
            var newY = (int) (p1.Y + vector.Y * a);

            return new Position(newX, newY);
        }
    }

    public static class DeadpoolConstants
    {
        public const int Health = 1380;
        public const int Mana = 100;
        public const int Damage = 80;
        public const int MoveSpeed = 200;
        public const int ManaRegen = 1;
        public const int AttackRange = 110;
    }

    class Player
    {
        static void Main(string[] args)
        {
            string[] inputs;
            int myTeam = int.Parse(Console.ReadLine());
            int bushAndSpawnPointCount = int.Parse(Console.ReadLine()); // usefrul from wood1, represents the number of bushes and the number of places where neutral units can spawn
            for (int i = 0; i < bushAndSpawnPointCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string entityType = inputs[0]; // BUSH, from wood1 it can also be SPAWN
                int x = int.Parse(inputs[1]);
                int y = int.Parse(inputs[2]);
                int radius = int.Parse(inputs[3]);
            }
            int itemCount = int.Parse(Console.ReadLine()); // useful from wood2

            var items = new List<Item>();
            var myItems = new Dictionary<int, List<Item>>();
            for (int i = 0; i < itemCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string itemName = inputs[0]; // contains keywords such as BRONZE, SILVER and BLADE, BOOTS connected by "_" to help you sort easier
                int itemCost = int.Parse(inputs[1]); // BRONZE items have lowest cost, the most expensive items are LEGENDARY
                int damage = int.Parse(inputs[2]); // keyword BLADE is present if the most important item stat is damage
                int health = int.Parse(inputs[3]);
                int maxHealth = int.Parse(inputs[4]);
                int mana = int.Parse(inputs[5]);
                int maxMana = int.Parse(inputs[6]);
                int moveSpeed = int.Parse(inputs[7]); // keyword BOOTS is present if the most important item stat is moveSpeed
                int manaRegeneration = int.Parse(inputs[8]);
                int isPotion = int.Parse(inputs[9]); // 0 if it's not instantly consumed

                var item = new Item()
                {
                    Name = itemName,
                    Cost = itemCost,
                    Damage = damage,
                    Health = health,
                    MaxHealth = maxHealth,
                    Mana = mana,
                    MaxMana = maxMana,
                    MoveSpeed = moveSpeed,
                    ManaRegeneration = manaRegeneration,
                    IsPotion = isPotion == Constants.IsPotionValue
                };

                items.Add(item);
            }

            // game loop
            while (true)
            {
                var gameContext = new GameContext(myItems);
                int gold = int.Parse(Console.ReadLine());
                int enemyGold = int.Parse(Console.ReadLine());
                int roundType = int.Parse(Console.ReadLine()); // a positive value will show the number of heroes that await a command
                int entityCount = int.Parse(Console.ReadLine());

                gameContext.MyGold = gold;
                gameContext.EnemyGold = enemyGold;
                gameContext.RoundType = roundType;
                gameContext.EntityCount = enemyGold;
                gameContext.Items = items;

                for (int i = 0; i < entityCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int unitId = int.Parse(inputs[0]);
                    int team = int.Parse(inputs[1]);
                    string unitType = inputs[2]; // UNIT, HERO, TOWER, can also be GROOT from wood1
                    int x = int.Parse(inputs[3]);
                    int y = int.Parse(inputs[4]);
                    int attackRange = int.Parse(inputs[5]);
                    int health = int.Parse(inputs[6]);
                    int maxHealth = int.Parse(inputs[7]);
                    int shield = int.Parse(inputs[8]); // useful in bronze
                    int attackDamage = int.Parse(inputs[9]);
                    int movementSpeed = int.Parse(inputs[10]);
                    int stunDuration = int.Parse(inputs[11]); // useful in bronze
                    int goldValue = int.Parse(inputs[12]);
                    int countDown1 = int.Parse(inputs[13]); // all countDown and mana variables are useful starting in bronze
                    int countDown2 = int.Parse(inputs[14]);
                    int countDown3 = int.Parse(inputs[15]);
                    int mana = int.Parse(inputs[16]);
                    int maxMana = int.Parse(inputs[17]);
                    int manaRegeneration = int.Parse(inputs[18]);
                    string heroType = inputs[19]; // DEADPOOL, VALKYRIE, DOCTOR_STRANGE, HULK, IRONMAN
                    int isVisible = int.Parse(inputs[20]); // 0 if it isn't
                    int itemsOwned = int.Parse(inputs[21]); // useful from wood1

                    var unitDetails = new EntityDetails()
                    {
                        UnitId = unitId,
                        Team = team,
                        UnitType = unitType,
                        Position = new Position(x, y),
                        AttackRange = attackRange,
                        Health = health,
                        MaxHealth = maxHealth,
                        Shield = shield,
                        AttackDamage = attackDamage,
                        MovementSpeed = movementSpeed,
                        StunDuration = stunDuration,
                        GoldValue = goldValue,
                        CountDowns = new List<int>() { countDown1, countDown2, countDown3 },
                        Mana = mana,
                        MaxMana = maxMana,
                        ManaRegeneration = manaRegeneration,
                        HeroType = heroType,
                        IsVisible = isVisible,
                        ItemsOwned = itemsOwned
                    };

                    Entity entity = gameContext.InitializeEntity(unitDetails);

                    if (entity.UnitType == UnitType.Groot)
                    {
                        gameContext.Groots.Add(entity);
                    }
                    else
                    {
                        if (myTeam == entity.Team)
                        {
                            if (entity.UnitType == UnitType.Hero)
                            {
                                gameContext.MyHeroes.Add(entity as Hero);
                            }
                            else
                            {
                                gameContext.MyUnits.Add(entity);
                            }
                        }
                        else
                        {
                            if (entity.UnitType == UnitType.Hero)
                            {
                                gameContext.EnemyHeroes.Add(entity as Hero);
                            }
                            else
                            {
                                gameContext.EnemyUnits.Add(entity);
                            }
                        }
                    }
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // If roundType has a negative value then you need to output a Hero name, such as "DEADPOOL" or "VALKYRIE".
                // Else you need to output roundType number of any valid action, such as "WAIT" or "ATTACK unitId"
                gameContext.ProcessTurn();
            }
        }
    }

    public class EntityDetails
    {
        public int UnitId { get; internal set; }
        public int Team { get; internal set; }
        public string UnitType { get; internal set; }
        public Position Position { get; internal set; }
        public int AttackRange { get; internal set; }
        public int Health { get; internal set; }
        public int MaxHealth { get; internal set; }
        public int Shield { get; internal set; }
        public int AttackDamage { get; internal set; }
        public int MovementSpeed { get; internal set; }
        public int StunDuration { get; internal set; }
        public int GoldValue { get; internal set; }
        public List<int> CountDowns { get; internal set; }
        public int Mana { get; internal set; }
        public int MaxMana { get; internal set; }
        public int ManaRegeneration { get; internal set; }
        public string HeroType { get; internal set; }
        public int IsVisible { get; internal set; }
        public int ItemsOwned { get; internal set; }
    }

    public class GameContext
    {
        public GameContext(Dictionary<int, List<Item>> myItems)
        {
            this.MyUnits = new List<Entity>();
            this.MyHeroes = new List<Hero>();
            this.EnemyUnits = new List<Entity>();
            this.EnemyHeroes = new List<Hero>();
            this.Groots = new List<Entity>();
            this.Items = new List<Item>();
            this.MyItems = myItems;
        }

        public int MyGold { get; set; }

        public int EnemyGold { get; set; }

        public int RoundType { get; set; }

        public int EntityCount { get; set; }

        public List<Entity> MyUnits { get; set; }

        public List<Entity> EnemyUnits { get; set; }

        public List<Hero> MyHeroes { get; set; }

        public List<Hero> EnemyHeroes { get; set; }

        public List<Entity> Groots { get; set; }

        public List<Item> Items { get; set; }

        public Dictionary<int, List<Item>> MyItems { get; set; }

        internal Entity InitializeEntity(EntityDetails details)
        {
            UnitType unitType = Utils.GetUnitTypeFromString(details.UnitType);
            switch (unitType)
            {
                case UnitType.Unit:
                    {
                        var unit = new Entity(details);
                        return unit;
                    }
                case UnitType.Hero:
                    {
                        var hero = new Hero(details);
                        return hero;
                    }
                case UnitType.Tower:
                    {
                        var tower = new Entity(details);
                        return tower;
                    }
                default:
                    {
                        var groot = new Entity(details);
                        return groot;
                    }
            }
        }

        internal void ProcessTurn()
        {
            if (this.RoundType < 0)
            {
                var hero = this.PickHero(this.RoundType);
                Console.WriteLine(hero);
            }
            else
            {
                foreach (var hero in this.MyHeroes)
                {
                    this.PickCommandForHero(hero);
                }
            }
        }

        private void PickCommandForHero(Hero hero)
        {
            var handlers = new List<ActionsHandler>()
            {
                new BuyPotionHandler(),
                new UseSkillHandler(),
                new FinishHeroHandler(),
                new MoveBackToTowerHandler(),
                new BuyItemsHandler(),
                new AttackHeroHandler(),
                new AttackClosestUnitHandler(),
                new DenyUnitHandler(),
                new MoveAttackClosestUnitHandler(),
                new FollowClosestAllyUnitHandler(),
                new DefaultWaitHandler()
            };

            this.ProcessHandlers(handlers, hero);
        }

        public void ProcessHandlers(List<ActionsHandler> handlers, Hero hero)
        {
            for (int i = 1; i < handlers.Count; i++)
            {
                handlers[i - 1].SetSuccessor(handlers[i]);
            }

            handlers[0].ProcessTurn(this, hero);
        }

        public void Move(Position position)
        {
            Console.WriteLine($"{Constants.MoveCommand} {position.ToString()}");
        }

        private string PickHero(int round)
        {
            if (round == -1)
            {
                return Constants.IronmanString;
            }
            else
            {
                return Constants.DoctorStrangeString;
            }
        }

        internal void Attack(int unitId, string hero)
        {
            Console.WriteLine($"{Constants.AttackCommand} {unitId};" + hero);
        }

        internal void MoveAttack(Position position, int unitId)
        {
            Console.WriteLine($"{Constants.MoveAttackCommand} {position.ToString()} {unitId}");
        }

        internal void Wait()
        {
            Console.WriteLine(Constants.WaitCommand);
        }

        internal void Buy(string name)
        {
            Console.WriteLine($"{Constants.BuyCommand} {name}");
        }

        internal void Sell(string name)
        {
            Console.WriteLine($"{Constants.SellCommand} {name}");
        }

        internal void AoeHeal(Position position)
        {
            Console.WriteLine($"{Constants.AoeHealCommand} {position.ToString()}");
        }

        internal void Burning(Position position)
        {
            Console.WriteLine($"{Constants.BurningCommand} {position.ToString()}");
        }

        internal void Fireball(Position position)
        {
            Console.WriteLine($"{Constants.FireBallCommand} {position.ToString()}");
        }

        internal void Pull(int unitId)
        {
            Console.WriteLine($"{Constants.PullCommand} {unitId.ToString()}");
        }

        internal void Shield(int unitId)
        {
            Console.WriteLine($"{Constants.ShieldCommand} {unitId.ToString()}");
        }
    }

    internal class DenyUnitHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetUnit = context.MyUnits.Where(u => MathUtils.GetDistance(u, hero) <= hero.AttackRange).FirstOrDefault(u => u.Health <= hero.AttackDamage && u.Health < u.MaxHealth * 0.4);
            if (targetUnit != null)
            {
                context.Attack(targetUnit.UnitId, hero.HeroType.ToString());
                context.MyUnits.Remove(targetUnit);
                return true;
            }

            return false;
        }
    }

    internal class FinishHeroHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetHero = context.EnemyHeroes.FirstOrDefault(h => h.Health < hero.AttackDamage * 2);
            if (targetHero != null)
            {
                if (MathUtils.GetDistance(hero, targetHero) <= hero.AttackRange)
                {
                    context.Attack(targetHero.UnitId, hero.HeroType.ToString());
                    return true;
                }
                else if (MathUtils.GetDistance(targetHero, hero) <= hero.MovementSpeed + hero.AttackRange)
                {
                    var position = MathUtils.GetPointAlongLine(hero.Position, targetHero.Position, hero.AttackRange);
                    context.MoveAttack(position, targetHero.UnitId);
                    return true;
                }
            }

            return false;
        }
    }

    internal class AttackHeroHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            if (context.EnemyUnits.Where(u => MathUtils.GetDistance(hero, u) < Constants.AggroDistance).Count() <= 2 &&
                hero.Health > hero.MaxHealth * 0.6)
            {
                var targetUnit = context.EnemyHeroes.Where(u => MathUtils.GetDistance(u, hero) <= hero.AttackRange).OrderBy(u => u.Health).FirstOrDefault();
                if (targetUnit != null)
                {
                    context.Attack(targetUnit.UnitId, hero.HeroType.ToString());
                    return true;
                }
            }

            return false;
        }
    }

    public class FinishEnemyWithFireballHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            Entity target = context.EnemyHeroes.FirstOrDefault(h => h.Health <= Utils.CalculateFireBallDamage(hero.Mana, MathUtils.GetDistance(hero, h)));
            if (target != null &&
                hero.Mana >= Constants.FireBallManaCost &&
                hero.SkillCountdowns[1] == 0 &&
                MathUtils.GetDistance(hero, target) < Constants.FireBallRange)
            {
                context.Fireball(target.Position);
                return true;
            }

            return false;
        }
    }

    public class FireballHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            Entity target = context.EnemyHeroes.FirstOrDefault(h => Utils.CalculateFireBallDamage(hero.Mana, MathUtils.GetDistance(hero, h)) > 50);
            Console.Error.WriteLine(string.Join(", ", hero.SkillCountdowns));
            if (target != null &&
                hero.Mana >= Constants.FireBallManaCost &&
                hero.SkillCountdowns[1] == 0 &&
                MathUtils.GetDistance(hero, target) <= Constants.FireBallRange)
            {
                context.Fireball(target.Position);
                return true;
            }

            return false;
        }
    }

    public class BurnCloseEnemiesHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var allEnemyUnits = context.EnemyUnits.Concat(context.EnemyHeroes);
            var target = allEnemyUnits.FirstOrDefault(e => MathUtils.GetDistance(hero, e) <= Constants.BurningRange);
            if (target != null &&
                hero.Mana >= Constants.BurningManaCost * 2 &&
                hero.SkillCountdowns[2] == 0)
            {
                context.Burning(target.Position);
                return true;
            }

            return false;
        }
    }

    public class HealInjuredHeroHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetHero = context.MyHeroes.FirstOrDefault(h => h.Health < h.MaxHealth * 0.5);
            if (targetHero != null &&
                               hero.Mana >= Constants.AoeHealManaCost &&
                               hero.SkillCountdowns[0] == 0 &&
                               MathUtils.GetDistance(hero, targetHero) <= Constants.AoeHealRange)
            {
                context.AoeHeal(hero.Position);
                return true;
            }

            return false;
        }
    }

    public class PullEnemyHeroHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetHero = context.EnemyHeroes.FirstOrDefault(h => MathUtils.GetDistance(hero, h) < Constants.PullRange);
            if (targetHero != null &&
                               hero.Mana >= Constants.PullManaCost &&
                               hero.SkillCountdowns[2] == 0)
            {
                context.Pull(targetHero.UnitId);
                return true;
            }

            return false;
        }
    }

    public class UseSkillHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            switch (hero.HeroType)
            {
                case HeroType.Ironman:
                    {
                        var ironManHandlers = new List<ActionsHandler>()
                        {
                            new FinishEnemyWithFireballHandler(),
                            new FireballHandler(),
                            new BurnCloseEnemiesHandler(),
                        };

                        if (ironManHandlers.Any(h => h.CanProcessTurn(context, hero)))
                        {
                            return true;
                        }

                        return false;
                    }
                case HeroType.DoctorStrange:
                    {
                        var doctorStrangeHandlers = new List<ActionsHandler>()
                        {
                            new ShieldUnitHandler(),
                            new HealInjuredHeroHandler(),
                            new PullEnemyHeroHandler(),
                        };

                        if (doctorStrangeHandlers.Any(h => h.CanProcessTurn(context, hero)))
                        {
                            return true;
                        }

                        return false;
                    }
                default:
                    return false;
            }
        }
    }

    internal class ShieldUnitHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetHero = context.MyHeroes.FirstOrDefault(h => h.Health < h.MaxHealth * 0.2 && MathUtils.GetDistance(hero, h) <= Constants.ShieldRange);
            if (targetHero != null &&
                context.EnemyHeroes.Any(h => h.AttackRange < MathUtils.GetDistance(h, hero)) &&
                               hero.Mana >= Constants.ShieldManaCost &&
                               hero.SkillCountdowns[1] == 0)
            {
                context.Shield(targetHero.UnitId);
                return true;
            }

            return false;
        }
    }

    public class BuyPotionHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetItem = context.Items.Where(i => i.IsPotion && i.Health > 0).OrderByDescending(i => i.Health).FirstOrDefault(i => context.MyGold >= i.Cost);

            if (targetItem != null &&
                hero.ItemsOwned < Constants.MaxItemCountPerHero &&
                hero.Health <= hero.MaxHealth * 0.4)
            {
                context.Buy(targetItem.Name);
                context.MyGold -= targetItem.Cost;
                return true;
            }

            return false;
        }
    }

    public class BuyItemsHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetItem = context.Items.Where(i => !i.IsPotion).OrderByDescending(i => i.Damage).FirstOrDefault(i => context.MyGold >= i.Cost);
            if (targetItem != null)
            {
                if (hero.ItemsOwned < Constants.MaxItemCountPerHero - 1)
                {
                    if (context.MyItems.ContainsKey(hero.UnitId))
                    {
                        context.MyItems[hero.UnitId].Add(targetItem);
                    }
                    else
                    {
                        context.MyItems.Add(hero.UnitId, new List<Item>() { targetItem });
                    }
                    context.MyGold -= targetItem.Cost;
                    context.Buy(targetItem.Name);
                    return true;
                }
                else if (context.MyItems.Count > 0 &&
                    !context.MyItems[hero.UnitId].Any(x => x.Name == targetItem.Name || x.Cost > targetItem.Cost))
                {
                    var sellItem = context.MyItems[hero.UnitId].OrderBy(i => i.Cost).FirstOrDefault();
                    context.Sell(sellItem.Name);
                    context.MyItems[hero.UnitId].Remove(sellItem);
                    context.MyGold += sellItem.Cost / 2;
                    return true;
                }
            }

            return false;
        }
    }

    public class DefaultWaitHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            context.Wait();
            return true;
        }
    }

    public class FollowClosestAllyUnitHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetUnit = context.MyUnits.Where(u => u.UnitType != UnitType.Tower).OrderBy(u => MathUtils.GetDistance(u, hero)).FirstOrDefault();

            if (targetUnit != null)
            {
                var targetPosition = new Position(targetUnit.Position.X + (hero.Team == 0 ? -1 : 1), targetUnit.Position.Y);
                context.Move(targetPosition);
                return true;
            }

            return false;
        }
    }

    public class MoveAttackClosestUnitHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetUnit = context.EnemyUnits.Where(u => MathUtils.GetDistance(u, hero) <= hero.MovementSpeed + hero.AttackRange).OrderBy(u => u.Health).FirstOrDefault(u => u.Health <= hero.AttackDamage);

            if (targetUnit != null &&
                hero.Health > hero.MaxHealth * 0.4)
            {
                var position = MathUtils.GetPointAlongLine(hero.Position, targetUnit.Position, hero.AttackRange);
                context.MoveAttack(position, targetUnit.UnitId);
                return true;
            }

            return false;
        }
    }

    public class AttackClosestUnitHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var targetUnit = context.EnemyUnits.Where(u => MathUtils.GetDistance(u, hero) <= hero.AttackRange).OrderBy(u => u.Health).FirstOrDefault();
            if (targetUnit != null)
            {
                context.Attack(targetUnit.UnitId, hero.HeroType.ToString());
                return true;
            }

            return false;
        }
    }

    public class MoveBackToTowerHandler : ActionsHandler
    {
        public override bool CanProcessTurn(GameContext context, Hero hero)
        {
            var towerPosition = context.MyUnits.FirstOrDefault(u => u.UnitType == UnitType.Tower).Position;
            var myTowerPosition = new Position(towerPosition.X + (hero.Team == 0 ? -1 : 1), towerPosition.Y);
            var targetUnit = context.MyUnits.Where(u => u.UnitType != UnitType.Tower).OrderBy(u => MathUtils.GetDistance(myTowerPosition, u.Position)).FirstOrDefault();
            var enemyTower = context.EnemyUnits.FirstOrDefault(u => u.UnitType == UnitType.Tower);
            if (targetUnit != null)
            {
                if (hero.Health < hero.MaxHealth * 0.30 ||
                    context.MyUnits.Where(u => MathUtils.GetDistance(u, enemyTower) < MathUtils.GetDistance(hero, enemyTower)).Count() < 2)
                {
                    context.Move(myTowerPosition);
                    return true;
                }

                return false;
            }
            else
            {
                if (MathUtils.GetDistance(hero.Position, myTowerPosition) == 0)
                {
                    return false;
                }

                context.Move(myTowerPosition);
                return true;
            }
        }
    }

    public abstract class ActionsHandler
    {
        protected ActionsHandler successor;

        public void SetSuccessor(ActionsHandler successor)
        {
            this.successor = successor;
        }

        public void ProcessTurn(GameContext context, Hero hero)
        {
            var result = this.CanProcessTurn(context, hero);
            if (result)
            {
                return;
            }
            else if (successor != null)
            {
                successor.ProcessTurn(context, hero);
            }

        }

        public abstract bool CanProcessTurn(GameContext context, Hero hero);
    }

    public class Entity
    {
        public Entity(EntityDetails details)
        {
            this.UnitId = details.UnitId;
            this.Team = details.Team;
            this.UnitType = Utils.GetUnitTypeFromString(details.UnitType);
            this.Position = details.Position;
            this.AttackRange = details.AttackRange;
            this.Health = details.Health;
            this.MaxHealth = details.MaxHealth;
            this.AttackDamage = details.AttackDamage;
            this.MovementSpeed = details.MovementSpeed;
        }

        public int UnitId { get; set; }

        public int Team { get; set; }

        public UnitType UnitType { get; set; }

        public Position Position { get; set; }

        public int AttackRange { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int AttackDamage { get; set; }

        public int MovementSpeed { get; set; }

    }

    public class Hero : Entity
    {
        public Hero(EntityDetails details)
            : base(details)
        {
            this.SkillCountdowns = details.CountDowns;
            this.Mana = details.Mana;
            this.MaxMana = details.MaxMana;
            this.Shield = details.Shield;
            this.ManaRegeneration = details.ManaRegeneration;
            this.HeroType = Utils.GetHeroTypeFromString(details.HeroType);
            this.IsVisible = details.IsVisible == Constants.IsVisibleValue;
            this.ItemsOwned = details.ItemsOwned;
        }

        public List<int> SkillCountdowns { get; set; }

        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int Shield { get; set; }

        public int ManaRegeneration { get; set; }

        public HeroType HeroType { get; set; }

        public bool IsVisible { get; set; }

        public int ItemsOwned { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public int Damage { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int MoveSpeed { get; set; }

        public int ManaRegeneration { get; set; }

        public bool IsPotion { get; set; }
    }

    public enum UnitType
    {
        Unit = 0,
        Hero = 1,
        Groot = 2,
        Tower = 3
    }

    public enum HeroType
    {
        Deadpool,
        Valkyrie,
        DoctorStrange,
        Hulk,
        Ironman
    }

    public struct Position
    {
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(Object other)
        {
            return other is Position && Equals((Position) other);
        }

        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
        }
    }
}
