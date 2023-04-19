using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.DeathObserver
{
    public class CreatureTracker : IDeathObserver
    {
        private World World { get; set; }
        public List<Creature> DeadCreatures { get; private set; }

        public CreatureTracker(World world)
        {
            World = world;
            DeadCreatures = new List<Creature>();
        }

        public void TrackCreature(Creature creature)
        {
            creature.DeathObservers.Add(this);
        }
        public void OnDeath(Creature creature)
        {
            DeadCreatures.Add(creature);
            World.Creatures.Remove(creature);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Trace.TraceInformation($"ID: ({creature.ID})|({creature.Name}) at position: ({creature.X}, {creature.Y}) has died and been removed from the world!");
            Console.ResetColor();
        }
    }
}
