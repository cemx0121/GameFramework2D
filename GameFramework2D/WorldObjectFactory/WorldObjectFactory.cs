using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.WorldObjectFactory
{
    public abstract class WorldObjectFactory
    {
        public abstract Creature CreateCreature(int x, int y, string name, int health);
        public abstract AttackItem CreateAttackItem(int x, int y, bool canBePicked, int damage, string name, string description);
        public abstract DefenceItem CreateDefenceItem(int x, int y, bool canBePicked, int defenceValue, string name, string description);
        public abstract Obstacle CreateObstacle(int x, int y, string name, string description);
    }
}
