using UnityEngine;

#nullable enable
namespace Lightning
{
	public class ConduitEnemyLightning : MonoBehaviour, IConduitEnemyLightning
	{
		public string ConduitId { get; private set; } = null!;
		public GameObject GameObject => conduitFrom;
		public GameObject conduitFrom = null!;
		private PeriodicBouncingEnemyLightningEmit _lightningEmitter = null!;

		public void QueueBolt(PropagatingProjectile bolt)
		{
			if (gameObject == null)
				return;
			_lightningEmitter.QueueBolt(bolt);
		}

		private void Start()
		{
			if (conduitFrom == null)
				conduitFrom = gameObject;
			
			ConduitId = GameObject.GetInstanceID().ToString();
			_lightningEmitter = Instantiate(Game.Instance.Prefabs.Enemy.PeriodicBouncingEnemyLightningEmit, GameObject.transform);
		}
	}
}