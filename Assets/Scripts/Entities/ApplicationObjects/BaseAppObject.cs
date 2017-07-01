using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Entities.Itens.Equippable;
using Assets.Scripts.Entities.Skills;
using Assets.Scripts.Entities.Skills.Combat;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Interfaces.Managers.Components;

namespace Assets.Scripts.Entities.ApplicationObjects
{
    public abstract class BaseAppObject
    {
        public BaseAppObject() { }

        public BaseAppObject(
            GameObject gameObject,
            BaseCreature baseCreature)
        {
            GameObject = gameObject;
            Transform = gameObject.transform;
            BaseCreature = baseCreature;
        }



        private float defaultSpeedValue = 1.5f;
        private float defaultRunningValue = 1.5f;
        private float defaultDiagonalSpeedDivisor = 2.4f;

        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BaseCreature BaseCreature { get; set; }
        public GameObject GameObject { get; set; }
        public Transform Transform { get; set; }

        //Determines whether object can make any action or not
        public bool HasActionsPrevented { get; set; }

        public AttackTypeEnum GetAttackTypeForEquippedWeapon()
        {
            //DEBUG
            //Remove (TESTS ONLY)
            return AttackTypeEnum.Stock;

            var equippedItensManager = GetMonoBehaviourObject<IEquippedItensManager>();

            if (equippedItensManager.GetEquippedWeapon() == null)
            {
                return AttackTypeEnum.Stock;
            }
            return equippedItensManager.GetEquippedWeapon().CurrentAttackType;
        }

        public float IncreaseCombatSkillPoint()
        {
            string skillName = "HandToHandSkill";
            var equippedItensManager = GetMonoBehaviourObject<IEquippedItensManager>();
            if (equippedItensManager.GetEquippedWeapon() != null)
            {
                skillName = equippedItensManager.GetEquippedWeapon().SkillUsed;
            }

            BaseSkill skill = BaseCreature.CombatSkills.GetSkillByName(skillName);
            skill.EarnSkillPoint();

            return skill.SkillValue;
        }

        public float GetStraightLineSpeed(bool isRunning)
        {
            if (isRunning)
            {
                return defaultSpeedValue * defaultRunningValue;
            }

            return defaultSpeedValue;
        }

        public float GetDiagionalSpeed(bool isRunning)
        {
            float diagonalSpeedValue = defaultSpeedValue - (defaultSpeedValue / defaultDiagonalSpeedDivisor);

            if (isRunning)
            {
                float runningSpeedValue = diagonalSpeedValue * defaultRunningValue;

                return runningSpeedValue;
            }
            return diagonalSpeedValue;
        }

        //Shall receive Weapon Weight (float) in the future to calculate
        //Less strength = minor sequence
        public int GetMaximumAttacks()
        {
            return 3 - 1; //Zero based
        }

        public float GetTimeToRecoverFromLastAttack()
        {
            return GetDelayActionTime(GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon());
        }

        public float GetTimeToRecoverFromAction()
        {
            float delayTime = GetDelayActionTime(GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon());
            if (delayTime > 4.5f)
                return 4.5f;
            if (delayTime < 0.75f)
                return 0.75f;

            return delayTime;
        }

        float GetDelayActionTime(BaseEquippableItem equippedWeapon)
        {
            return (float)equippedWeapon.Weight * 100 / (BaseCreature.Strength + BaseCreature.Dexterity);
        }

        public double GetDamageDealt(BaseEquippableItem equippedWeapon)
        {
            double skillValue = BaseCreature.CombatSkills.GetSkillValueByName(equippedWeapon.SkillUsed);
            System.Random rnd = new System.Random();

            if (equippedWeapon.AttacksType.All(x => x != AttackTypeEnum.QuickRanged && x != AttackTypeEnum.LongRanged))
            {
                return (BaseCreature.Strength + BaseCreature.Intelligence + equippedWeapon.Damage + skillValue + rnd.Next(0, 100)) / 5;
            }

            return (BaseCreature.Dexterity + BaseCreature.Intelligence + equippedWeapon.Damage + skillValue + rnd.Next(0, 100)) / 5;
        }

        public void ReceiveDamage(double damage)
        {
            Debug.Log(string.Format("{0} - Damage received: {1}", GameObject.name, damage));
            //Damage received = (Resistance + Strength)/4 - damage
            BaseCreature.Health -= damage;
        }

        public BaseSkill GetCombatSkillByWeapon()
        {
            string skillName = GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon().SkillUsed;

            return BaseCreature.CombatSkills.GetSkillByName(skillName);
        }

        public float GetAttackValue()
        {
            float skillValue = GetCombatSkillByWeapon().SkillValue;
            int abilityBuffValue = GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon().AttacksType.Any(x => x == AttackTypeEnum.QuickRanged || x == AttackTypeEnum.LongRanged) ? BaseCreature.Dexterity : BaseCreature.Strength;

            return (skillValue * BaseCreature.Intelligence) / 2;
        }

        public bool DefendAttack(BaseAppObject attacker, DefenseTypeEnum defenseType)
        {
            BaseCombatSkill defenderCombatSkill = null;

            //Is blocking with shield
            if (defenseType == DefenseTypeEnum.Block)
            {
                EquippableItemTypeEnum defenderWeaponType = GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon().ItemType;
                if (defenderWeaponType == EquippableItemTypeEnum.Shield)
                {
                    defenderCombatSkill = BaseCreature.CombatSkills.ShieldsSkill;
                }
                else if (defenderWeaponType == EquippableItemTypeEnum.Weapon)
                {
                    defenderCombatSkill = BaseCreature.CombatSkills.ParrySkill;
                }
            }

            float defendingPoints = (defenderCombatSkill.SkillValue * BaseCreature.Intelligence) / 2;
            float attackerPoints = attacker.GetAttackValue();

            return defendingPoints > attackerPoints;
        }

        public bool CanAttackTarget(BaseAppObject target)
        {
            //if distance of target is greater than distance of attacker's weapon each, then the attack will fail
            float distanceFromAttacker = Vector3.Distance(target.Transform.position, Transform.position);
            float weaponReach = 1;
            Debug.Log(distanceFromAttacker);
            if (distanceFromAttacker > weaponReach)
            {
                return false;
            }

            var targetCombatManager = target.GetMonoBehaviourObject<ICombatManager>();
            //Target has already defended the attack from this attacker
            if (targetCombatManager.GetIsAttemptingToParry() && targetCombatManager.GetParryingTarget() == this)
            {
                //Target has defended, so it should be set to false
                targetCombatManager.SetIsAttemptingToParryAttack(false);

                Debug.Log("Blocked attack with parry");

                return false;
            }
            //If is attacking a target that is parrying a different attacker, then this attacker will have
            //an instant success.
            else if (targetCombatManager.GetIsAttemptingToParry() && targetCombatManager.GetParryingTarget() != this)
            {
                //Target has received an attack and is not defending anymore
                targetCombatManager.SetIsAttemptingToParryAttack(false);

                Debug.Log(target.GameObject.name + " foi atacado por outro objeto e perdeu a defesa");
                return true;
            }

            //Target will receive a mini-stun caused by attack either on his shield or on itself
            var attackService = IoCContainer.GetImplementation<IAttackService>();
            attackService.MiniStunTarget(target);

            //Target is trying to block with a shield
            if (targetCombatManager.GetIsBlockingWithShield())
            {
                if (attackService.TargetIsBlockingAttackerDirectionsWithShield(this, target))
                {
                    Debug.Log(target.GameObject.name + " is trying to block with shield");
                    return target.DefendAttack(this, DefenseTypeEnum.Block);
                }
                else //Target is not blocking attacker direction resulting in Instant hit
                {
                    return true;
                }
            }

            //return: Int + Dex + Skill + Rand 100 X enemie's
            return true;
        }

        public void DecreaseSteamina()
        {
            var staminaManager = GetMonoBehaviourObject<IStaminaManager>();
            if (!staminaManager.IsEnabled())
            {
                staminaManager.Enable();
            }

            staminaManager.SetDecreasingStamina(true);
        }

        public void Die()
        {

            var objectPoolingService = IoCContainer.GetImplementation<IObjectPoolingService>();
            objectPoolingService.KillAndPoolNpc(this);
        }

        public bool IsAlive()
        {
            return BaseCreature.Health > 0;
        }

        public DirectionEnum GetFacingDirection()
        {
            return GameObject.GetComponent<IFacingDirection>().GetFacingDirection();
        }

        public T GetMonoBehaviourObject<T>() where T : class
        {
            return this.GameObject.GetComponent<T>();
        }
    }
}
