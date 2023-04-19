using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.WorldObjectFactory
{
    public class DefaultWorldObjectFactory : WorldObjectFactory
    {
        public override Creature CreateCreature(int x, int y, string name, int health)
        {
            return new Creature(x, y, name, health);
        }

        public override AttackItem CreateAttackItem(int x, int y, bool canBePicked, int damage, string name, string description)
        {
            return new AttackItem(x, y, canBePicked, damage, name, description);
        }

        public override DefenceItem CreateDefenceItem(int x, int y, bool canBePicked, int defenceValue, string name, string description)
        {
            return new DefenceItem(x, y, canBePicked, defenceValue, name, description);
        }

        public override Obstacle CreateObstacle(int x, int y, string name, string description)
        {
            return new Obstacle(x, y, name, description);
        }
    }
}
