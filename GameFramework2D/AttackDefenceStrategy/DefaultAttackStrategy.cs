using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.AttackDefence
{
    public class DefaultAttackStrategy : IAttackStrategy
    {
        public int CalculateDamage(List<AttackItem> attackItems)
        {
            int totalDamage = 0;
            foreach (var attackItem in attackItems)
            {
                totalDamage += attackItem.Damage;
            }
            return totalDamage;
        }
    }
}
