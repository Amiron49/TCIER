#nullable enable
using Lightning;
using UnityEngine;

public interface IConduitEnemyLightning
{
	public string ConduitId { get; }
	public GameObject GameObject { get; }
	public void QueueBolt(PropagatingProjectile bolt);
}