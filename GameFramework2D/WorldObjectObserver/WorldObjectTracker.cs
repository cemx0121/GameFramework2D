using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.WorldObjectObserver
{
    public class WorldObjectTracker : IWorldObjectObserver
    {
        private World _world;
        public List<WorldObject> RemovedWorldObjects { get; }

        public WorldObjectTracker(World world)
        {
            _world = world;
            RemovedWorldObjects = new List<WorldObject>();
        }

        public void OnWorldObjectPicked(WorldObject worldObject)
        {
            _world.RemoveWorldObject(worldObject);
            RemovedWorldObjects.Add(worldObject);
        }
    }
}
