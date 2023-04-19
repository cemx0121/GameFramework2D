using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework2D.Models
{
    public class DefenceItem : WorldObject
    {
        public int DefenceValue { get; set; }

        public DefenceItem(int x, int y, bool canBePicked, int defenceValue, string name, string description)
            : base(x, y, canBePicked, name, description)
        {
            DefenceValue = defenceValue;
        }
    }
}
