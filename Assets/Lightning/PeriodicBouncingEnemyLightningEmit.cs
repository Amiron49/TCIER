using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#nullable enable
namespace Lightning
{
	public class PeriodicBouncingEnemyLightningEmit : MonoBehaviour
	{
		[FormerlySerializedAs("RadialTelegraph")] public RadialTelegraph radialTelegraph;
		[FormerlySerializedAs("Cooldown")] public float cooldown = 2;
		[FormerlySerializedAs("Radius")] [FormerlySerializedAs("Range")] public float radius = 6;
		[FormerlySerializedAs("Unlimited")] public bool unlimited;
		private readonly Queue<PropagatingProjectile> _queuedBolts = new();
		public LightingZap lightingZapPrefab = null!;
		private ComplexTimer<PeriodicBouncingEnemyLightningEmit> _timer = null!;
		public GameObject buzzingEffect = null!;

		// Start is called before the first frame update
		void Start()
		{
			radialTelegraph.MaxRadius = radius;
			_timer = new ComplexTimer<PeriodicBouncingEnemyLightningEmit>(this, 2, 3);

			_timer.OnTime += (_, _) =>
			{
				SpawnLightningEmitter();

				if (unlimited)
				{
					QueueBolt();			
					return;
				}

				Stop();
			};
			
			if (unlimited)
			{
				QueueBolt();
				ReStart();
			}
			else
			{
				Stop();
			}
		}

		// Update is called once per frame
		void Update()
		{
			if (!_queuedBolts.Any())
				return;
		
			_timer.Update();
			radialTelegraph.SetProgress(_timer.Timer.Progress);
		}

		public void QueueBolt(PropagatingProjectile? bolt = null)
		{
			_queuedBolts.Enqueue(bolt ?? new PropagatingProjectile());
			if (!_timer.Running)
			{
				ReStart();
			}
		}

		public void Stop()
		{
			radialTelegraph.SetMax(0);
			_timer.Stop();
			_timer.Reset();
			buzzingEffect.SetActive(false);
		}
	
		public void ReStart()
		{
			radialTelegraph.SetMax(radius);
			_timer.Reset();
			_timer.Start();
			buzzingEffect.SetActive(true);
		}

		public void SpawnLightningEmitter()
		{
			var targets = FindPotentialTargets();
			var emitter = gameObject.AddComponent<EnemyLightningBoltEmitter>();
			emitter.lightningZapPrefab = lightingZapPrefab;
			emitter.cooldown = Mathf.Min(cooldown / _queuedBolts.Count, emitter.cooldown);
			emitter.Bolts = new Queue<PropagatingProjectile>(_queuedBolts);
			_queuedBolts.Clear();
			emitter.PotentialTargets = targets.ToArray();
		}

		private IEnumerable<IConduitEnemyLightning> FindPotentialTargets()
		{
			var results = new Collider2D[30];
			Physics2D.OverlapCircleNonAlloc(transform.position, radius / 2, results);
			var filtered = results.Where(x => x != null).Select(x => x.gameObject.GetComponent<IConduitEnemyLightning>()).Where(x => x != null).ToArray();
			return filtered.ToArray();
		}
	}
}