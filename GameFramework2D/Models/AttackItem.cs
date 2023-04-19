using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.Models
{
    public class AttackItem : WorldObject
    {
        public int Damage { get; set; }
        public AttackItem(int x, int y, bool canBePicked, int damage, string name, string description)
            : base(x, y, canBePicked, name, description)
        {
            Damage = damage;
        }
    }
}
