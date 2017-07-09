using Assets.Scripts.Interfaces.Services.Combat;
using Assets.Scripts.Services.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Factories.Services.Combat
{
    public class AttackTargetServiceFactory : BaseSingletonFactory<AttackTargetService, IAttackTargetService>
    {
    }
}
