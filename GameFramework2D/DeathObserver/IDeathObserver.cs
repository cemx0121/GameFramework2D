using GameFramework2D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.DeathObserver
{
    public interface IDeathObserver
    {
        void OnDeath(Creature creature);
    }
}
