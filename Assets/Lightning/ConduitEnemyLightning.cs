using UnityEngine;

#nullable enable
namespace Lightning
{
	public class ConduitEnemyLightning : MonoBehaviour, IConduitEnemyLightning
	{
		public string ConduitId { get; private set; } = null!;
		public GameObject GameObject => conduitFrom;
		public GameObject conduitFrom = null!;
		public PeriodicBouncingEnemyLightningEmit lightningEmitter = null!;
		public PeriodicBouncingEnemyLightningEmit lightningEmitterPrefab = null!;

		public void QueueBolt(PropagatingProjectile bolt)
		{
			lightningEmitter.QueueBolt(bolt);
		}

		private void Start()
		{
			ConduitId = GameObject.GetInstanceID().ToString();

			if (lightningEmitter == null)
			{
				lightningEmitter = Instantiate(lightningEmitterPrefab, GameObject.transform);
			}
		}
	}
}