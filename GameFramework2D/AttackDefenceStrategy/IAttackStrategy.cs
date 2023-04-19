using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.AttackDefence
{
    public interface IAttackStrategy
    {
        int CalculateDamage(List<AttackItem> attackItems);
    }
}
