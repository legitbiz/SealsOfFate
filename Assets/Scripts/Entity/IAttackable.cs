using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Assets.Scripts.Entity
{
    interface IAttackable
    {
        CombatData ToCombatData();
    }

    
}
