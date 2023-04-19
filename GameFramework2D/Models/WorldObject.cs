using GameFramework2D.WorldObjectObserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.Models
{
    public abstract class WorldObject
    {
        public int ID { get; }
        private static int nextID = 1;
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanBePicked { get; set; }

        private List<IWorldObjectObserver> _observers = new List<IWorldObjectObserver>();

        public WorldObject(int x, int y, bool canBePicked, string name, string description)
        {
            X = x;
            Y = y;
            CanBePicked = canBePicked;
            Name = name;
            Description = description;
            ID = nextID++;
        }

        public void RegisterObserver(IWorldObjectObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnregisterObserver(IWorldObjectObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyPicked()
        {
            var observersToNotify = new List<IWorldObjectObserver>(_observers);

            foreach (var observer in observersToNotify)
            {
                observer.OnWorldObjectPicked(this);
            }
        }
    }
}
