using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.AttackDefence
{
    public class DefaultDefenceStrategy : IDefenceStrategy
    {
        public int CalculateDefence(List<DefenceItem> defenceItems)
        {
            int totalDefence = 0;
            foreach (var defenceItem in defenceItems)
            {
                totalDefence += defenceItem.DefenceValue;
            }
            return totalDefence;
        }
    }
}
