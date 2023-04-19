using GameFramework2D.AttackDefence;
using GameFramework2D.DeathObserver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.Models
{
    public enum MoveResult
    {
        Success,
        Obstacle,
        OutOfBounds,
        Creature
    }
    public class Creature
    {
        public int ID { get; }
        private static int nextID = 1;
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public bool IsAlive { get; set; }
        public const int MaxItems = 5;
        public List<AttackItem> AttackItems { get; set; }
        public List<DefenceItem> DefenceItems { get; set; }
        public IAttackStrategy AttackStrategy { get; set; }
        public IDefenceStrategy DefenceStrategy { get; set; }
        public List<IDeathObserver> DeathObservers { get; set; }
        private ConsoleColor ConsoleColor { get; set; }
        private static List<ConsoleColor> AvailableColors = Enum.GetValues(typeof(ConsoleColor))
                                                                            .Cast<ConsoleColor>()
                                                                            .Where(c => c != ConsoleColor.Black && c != ConsoleColor.White && c != ConsoleColor.Gray && c != ConsoleColor.DarkGray)
                                                                            .ToList();
        private static Random Random = new Random();

        public Creature(int x, int y, string name, int health)
        {
            X = x;
            Y = y;
            Name = name;
            Health = health;
            IsAlive = true;
            AttackItems = new List<AttackItem>();
            DefenceItems = new List<DefenceItem>();
            AttackStrategy = new DefaultAttackStrategy();
            DefenceStrategy = new DefaultDefenceStrategy();
            DeathObservers = new List<IDeathObserver>();
            ID = nextID++;
            ConsoleColor = GetUniqueColor();
        }

        public void Hit(Creature target)
        {
            int totalDamage = AttackStrategy.CalculateDamage(AttackItems);
            PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) is hitting ID: ({target.ID})|({target.Name}) at position: ({target.X}, {target.Y}) with {totalDamage} damagepoints.");
            target.ReceiveHit(totalDamage);
        }

        public void ReceiveHit(int damage)
        {
            int totalDefence = DefenceStrategy.CalculateDefence(DefenceItems);
            Health -= Math.Max(damage - totalDefence, 0);
            PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) received {damage} in damagepoints while having {totalDefence} defencepoints. {Name} healthpoints is now: {Health}.");
            if (Health <= 0)
            {
                Health = 0;
                IsAlive = false;
                NotifyDeathObservers();
            }
        }

        public void Pick(WorldObject worldObject)
        {
            if (worldObject.CanBePicked && worldObject is AttackItem attackItem)
            {
                if (AttackItems.Count < MaxItems)
                {
                    AttackItems.Add(attackItem);
                    worldObject.NotifyPicked();
                    PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) picked up ID: ({worldObject.ID})|({attackItem.Name}) and now has a total of: {AttackStrategy.CalculateDamage(AttackItems)} hitpoints!");
                }
                else
                {
                    PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) tried to pick up ID: ({worldObject.ID})|({attackItem.Name}) but already carries maximum number of attackitems!");
                }
            }
            if (worldObject.CanBePicked && worldObject is DefenceItem defenceItem)
            {
                if (DefenceItems.Count < MaxItems)
                {
                    DefenceItems.Add(defenceItem);
                    worldObject.NotifyPicked();
                    PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) picked up ID: ({worldObject.ID})|({defenceItem.Name}) and now has a total of {DefenceStrategy.CalculateDefence(DefenceItems)} defencepoints!");
                }
                else
                {
                    PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) tried to pick up ID: ({worldObject.ID})|({defenceItem.Name}) but already carries maximum number of defenceitems!");
                }
            }
            if (!worldObject.CanBePicked)
            {
                PrintMessageWithColor($"ID: ({ID})|({Name}) at position: ({X}, {Y}) tried to pick up ID: ({worldObject.ID})|({worldObject.Name}) but the worldobject is not lootable!");
            }
        }

        private void NotifyDeathObservers()
        {
            foreach (var observer in DeathObservers)
            {
                observer.OnDeath(this);
            }
        }

        private void PrintMessageWithColor(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor;
            Trace.TraceInformation(message);
            Console.ForegroundColor = originalColor;
        }

        private ConsoleColor GetUniqueColor()
        {
            if (AvailableColors.Count > 0)
            {
                int randomIndex = Random.Next(AvailableColors.Count);
                ConsoleColor pickedColor = AvailableColors[randomIndex];
                AvailableColors.RemoveAt(randomIndex);
                return pickedColor;
            }
            else
            {
                // If there are no more unique colors, assign a random color without checking for uniqueness
                Array consoleColors = Enum.GetValues(typeof(ConsoleColor));
                return (ConsoleColor)consoleColors.GetValue(Random.Next(consoleColors.Length));
            }
        }

        public MoveResult Move(int dx, int dy, World world)
        {
            int newX = X + dx;
            int newY = Y + dy;

            if (world.IsValidPosition(newX, newY, out MoveResult moveResult, out string obstacleName))
            {
                X = newX;
                Y = newY;
                PrintMessageWithColor($"ID: ({ID})|({Name}) moved to position: ({X}, {Y}).");
            }
            else
            {
                if (moveResult == MoveResult.Obstacle)
                {
                    PrintMessageWithColor($"ID: ({ID})|({Name}) tried to move into a non-lootable obstacle ({obstacleName}) at position: ({newX}, {newY}), turn wasted!");
                }
                else if (moveResult == MoveResult.Creature)
                {
                    PrintMessageWithColor($"ID: ({ID})|({Name}) tried to move into a position occupied by another creature! {Name} makes a hit but stays in the same position: ({X}, {Y})");
                }
                else
                {
                    PrintMessageWithColor($"ID: ({ID})|({Name}) tried to move to position: ({newX}, {newY}), but its outside of the Worlds boundaries ({world.Width}, {world.Height}), turn wasted!");
                }
            }

            return moveResult;
        }
    }
}
