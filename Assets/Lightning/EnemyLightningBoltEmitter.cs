using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable
namespace Lightning
{
	public class EnemyLightningBoltEmitter : MonoBehaviour
	{
		public LightingZap lightningZapPrefab = null!;
		public Queue<PropagatingProjectile> Bolts = new();
		[FormerlySerializedAs("Cooldown")] public float cooldown = 0.3f;
		public IConduitEnemyLightning[] PotentialTargets = Array.Empty<IConduitEnemyLightning>();
		private Timer _timer = null!;
		private Transform _self = null!;
		private void Start()
		{
			_self = transform;
			_timer = new Timer(cooldown);
			_timer.Start();
			_timer.OnTime += (_, _) =>
			{
				ZapNext();
			};
		}

		private void Update()
		{
			_timer.Update();
		}

		private void ZapNext()
		{
			var bolt = Bolts.Dequeue();

			ZapFor(bolt);
		
			if (Bolts.Count == 0)
			{
				Destroy(this);
			}
		}
	
		private void ZapFor(PropagatingProjectile propagatingProjectile)
		{
			var eligibleTargets = PotentialTargets.Where(x => !propagatingProjectile.AlreadyTouched.Contains(x.ConduitId)).ToList();

			if (!eligibleTargets.Any())
				return;

			foreach (var target in eligibleTargets)
			{
				propagatingProjectile.AlreadyTouched.Add(target.ConduitId);
				var bolt = Instantiate(lightningZapPrefab);
				bolt.from = _self.position;
				bolt.to = target.GameObject.transform.position;
				bolt.OnZapped += (_, _) =>
				{
					target.QueueBolt(propagatingProjectile);
				};
			}
		}
	}
}