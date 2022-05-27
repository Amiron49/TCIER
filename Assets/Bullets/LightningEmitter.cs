using System.Collections.Generic;
using InternalLogic;
using UnityEngine;

public class LightningEmitter : BulletEmitterBase
{
	public IZap LightningZapPrefab;
	private PropagateFriendlyProjectiles _friendlyProjectilePropagator;

	protected override void ReInitInternal()
	{
		EnsurePropagator();
	}

	private void EnsurePropagator()
	{
		var friendlyProjectilePropagator = gameObject.GetComponent<PropagateFriendlyProjectiles>();
		if (friendlyProjectilePropagator == null)
		{
			friendlyProjectilePropagator = gameObject.AddComponent<PropagateFriendlyProjectiles>();
		}

		_friendlyProjectilePropagator = friendlyProjectilePropagator;
	}

	protected override bool ShootInternal()
	{
		return UnlimitedPower();
	}

	public bool UnlimitedPower()
	{
		var zap = Instantiate(LightningZapPrefab.gameObject);
		ConfigureZap(zap);
		var targets = _friendlyProjectilePropagator.PropagateMaybe(zap);

		return targets > 0;
	}

	private void ConfigureZap(GameObject bulletInstance)
	{
		var propagation = bulletInstance.AddComponent<PropagatedFriendlyProjectile>();
		propagation.MaxTargets = Mathf.RoundToInt(Properties.GetValueOrDefault(GunProperties.Projectiles, 1));
		propagation.PropagationCountRemaining = Mathf.RoundToInt(Properties.GetValueOrDefault(GunProperties.Arcs, 1));
		var zap = bulletInstance.GetComponent<IZap>();
		zap.Duration = 0.2f;
		ConfigureBullet(bulletInstance);
	}
}