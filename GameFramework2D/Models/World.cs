using GameFramework2D.DeathObserver;
using GameFramework2D.WorldObjectObserver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.Models
{
    public class World
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Creature> Creatures { get; set; }
        public List<WorldObject> WorldObjects { get; set; }
        public CreatureTracker CreatureTracker { get; private set; }
        private WorldObjectTracker _worldObjectTracker;


        public World(int width, int height)
        {
            Width = width;
            Height = height;
            Creatures = new List<Creature>();
            WorldObjects = new List<WorldObject>();
            CreatureTracker = new CreatureTracker(this);
            _worldObjectTracker = new WorldObjectTracker(this);
        }

        public void AddCreature(Creature creature)
        {
            if (IsValidPosition(creature.X, creature.Y, out _, out _))
            {
                Creatures.Add(creature);
                CreatureTracker.TrackCreature(creature);
            }
            else
            {
                throw new ArgumentException($"Invalid position for {creature.Name}: ({creature.X}, {creature.Y})");
            }
        }

        public void AddWorldObject(WorldObject worldObject)
        {
            if (IsValidPosition(worldObject.X, worldObject.Y, out _, out _))
            {
                worldObject.RegisterObserver(_worldObjectTracker);
                WorldObjects.Add(worldObject);
            }
            else
            {
                throw new ArgumentException($"Invalid position for world object: ({worldObject.X}, {worldObject.Y})");
            }
        }

        public void RemoveWorldObject(WorldObject worldObject)
        {
            worldObject.UnregisterObserver(_worldObjectTracker);
            WorldObjects.Remove(worldObject);
        }

        public bool IsValidPosition(int x, int y, out MoveResult moveResult, out string obstacleName)
        {
            obstacleName = null;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                moveResult = MoveResult.OutOfBounds;
                return false;
            }

            Creature existingCreature = Creatures.FirstOrDefault(c => c.X == x && c.Y == y);
            if (existingCreature != null)
            {
                moveResult = MoveResult.Creature;
                return false;
            }

            WorldObject obstacle = WorldObjects.FirstOrDefault(wo => wo.X == x && wo.Y == y && !wo.CanBePicked);
            if (obstacle != null)
            {
                moveResult = MoveResult.Obstacle;
                obstacleName = obstacle.Name;
                return false;
            }

            moveResult = MoveResult.Success;
            return true;
        }

        public bool IsObstacleAt(int x, int y)
        {
            return WorldObjects.Any(wo => wo is Obstacle && wo.X == x && wo.Y == y);
        }

        public void GameLoop(int numberOfRounds)
        {
            for (int i = 0; i < numberOfRounds; i++)
            {
                Console.WriteLine($"Round: {i + 1}");
                List<Creature> creaturesCopy = new List<Creature>(Creatures);
                foreach (var creature in creaturesCopy)
                {
                    // Check if the creature is still alive
                    if (!Creatures.Contains(creature))
                    {
                        continue;
                    }

                    int moveX = 0;
                    int moveY = 0;
                    bool validInput = false;

                    while (!validInput)
                    {
                        // Get user input for movement
                        Console.WriteLine($"ID: ({creature.ID})|({creature.Name}) at position: ({creature.X}, {creature.Y}) turn to make a move - (w/a/s/d):");
                        char move = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        switch (move)
                        {
                            case 'w':
                                moveY = -1;
                                validInput = true;
                                break;
                            case 'a':
                                moveX = -1;
                                validInput = true;
                                break;
                            case 's':
                                moveY = 1;
                                validInput = true;
                                break;
                            case 'd':
                                moveX = 1;
                                validInput = true;
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please enter w/a/s/d.");
                                break;
                        }
                    }

                    MoveResult moveResult = creature.Move(moveX, moveY, this);

                    if (moveResult == MoveResult.Creature)
                    {
                        Creature target = Creatures.FirstOrDefault(c => c.ID != creature.ID && c.X == creature.X + moveX && c.Y == creature.Y + moveY);
                        if (target != null)
                        {
                            creature.Hit(target);
                        }
                    }
                    else
                    {
                        // Check if there's a world object to pick
                        WorldObject worldObject = WorldObjects.FirstOrDefault(wo => wo.X == creature.X && wo.Y == creature.Y);
                        if (worldObject != null)
                        {
                            creature.Pick(worldObject);
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        public void GetWorldStatus()
        {
            Trace.TraceInformation($"========================================= WORLD STATUS =========================================");
            Trace.TraceInformation($">>>>>>>>>> Living creatures: {Creatures.Count} <<<<<<<<<<");
            foreach (Creature creature in Creatures)
            {
                Trace.TraceInformation($"ID: {creature.ID}|{creature.Name} at position: ({creature.X}, {creature.Y}) with {creature.Health} HP");
            }

            Trace.TraceInformation($">>>>>>>>>> Dead creatures: {CreatureTracker.DeadCreatures.Count} <<<<<<<<<<");
            foreach (Creature creature in CreatureTracker.DeadCreatures)
            {
                Trace.TraceInformation($"ID: {creature.ID}|{creature.Name} died at position: ({creature.X}, {creature.Y})");
            }

            Trace.TraceInformation($">>>>>>>>>> Available worldobjects: {WorldObjects.Count} <<<<<<<<<<");
            foreach (WorldObject worldObject in WorldObjects)
            {
                Trace.TraceInformation($"ID: {worldObject.ID}|{worldObject.Name} at position: ({worldObject.X}, {worldObject.Y}) | Lootable: {worldObject.CanBePicked}");
            }

            Trace.TraceInformation($">>>>>>>>>> Non-available/picked up worldobjects: {_worldObjectTracker.RemovedWorldObjects.Count} <<<<<<<<<<");
            foreach (WorldObject worldObject in _worldObjectTracker.RemovedWorldObjects)
            {
                Trace.TraceInformation($"ID: {worldObject.ID}|{worldObject.Name} picked up at position: ({worldObject.X}, {worldObject.Y}) | Lootable: {worldObject.CanBePicked}");
            }
            Trace.TraceInformation($"================================================================================================");
        }
    }
}
