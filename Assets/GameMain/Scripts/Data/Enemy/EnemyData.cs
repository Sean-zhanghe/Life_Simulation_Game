using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class EnemyData
    {
        private DREnemy dREnemy;

        public int Id { get { return dREnemy.Id; } }

        public string Name { get { return dREnemy.Name; } }

        public int EntityId { get { return dREnemy.EntityId; } }

        public string EnemyType { get { return dREnemy.EnemyType; } }

        public float MaxHP { get { return dREnemy.MaxHP; } }

        public float Damage { get { return dREnemy.Damage; } }

        public float WalkSpeed { get { return dREnemy.Speed; } }

        public float ChaseSpeed { get { return dREnemy.Chase; } }

        public float IdleTime { get { return dREnemy.IdleTime; } }

        public float PatrolRadius { get { return dREnemy.PatrolRadius; } }

        public float ChaseRadius { get { return dREnemy.ChaseRadius; } }

        public float AttackRate { get { return dREnemy.AttackRate; } }

        public string Drop { get { return dREnemy.Drop; } }

        public EnemyData(DREnemy dREnemy)
        {
            this.dREnemy = dREnemy;
        }
    }
}
