using UnityEngine;

namespace Lightning
{
	public class TakeEnemyLightningDamage : MonoBehaviour, IConduitEnemyLightning
	{
		public float damage = 40;
		public string ConduitId { get; set; } 
		public GameObject GameObject { get; set; }
		public Life life;


		private void Start()
		{
			GameObject = gameObject;
			ConduitId = GameObject.GetInstanceID().ToString();
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