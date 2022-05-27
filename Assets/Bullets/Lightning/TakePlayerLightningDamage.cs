using System;
using UnityEngine;

namespace Lightning
{
	public class TakePlayerLightningDamage : MonoBehaviour, IConduitEnemyLightning
	{
		public float damage = 40;
		public string ConduitId => Guid.NewGuid().ToString();
		public GameObject GameObject { get; set; }
		public Life life;


		private void Start()
		{
			GameObject = gameObject;
		}

		public void QueueBolt(PropagatingProjectile bolt)
		{
			life.TakeDamage(new EnemyLightningDamage(damage));
		}

		private class EnemyLightningDamage: IDamageSource
		{
			public float Damage { get; set; }
			public Team For { get; set; } = Team.Player;

			public EnemyLightningDamage(float damage)
			{
				Damage = damage;
			}
		}
	}
}