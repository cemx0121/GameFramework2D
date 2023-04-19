using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.Models
{
    public class Obstacle : WorldObject
    {
        public Obstacle(int x, int y, string name, string description)
    : base(x, y, false, name, description)
        {

        }
    }
}
