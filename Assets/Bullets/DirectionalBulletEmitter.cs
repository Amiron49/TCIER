using System;
using System.Collections.Generic;
using InternalLogic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DirectionalBulletEmitter : BulletEmitterBase
{
	public DirectionalBullet BulletPrefab;
	private Transform _transform;

	protected override void ReInitInternal()
	{
		_transform = transform;
	}

	protected override bool ShootInternal()
    {
        var amount = Properties.GetValueOrDefault(GunProperties.Projectiles, 1);

        for (int i = 0; i < amount; i++)
        {
            EmitBullet(_transform.up);
        }

		return true;
	}
	
	public void EmitBullet(Vector3 direction)
	{
		var ownPosition = _transform.position;
		var bulletTravelDirection = direction.normalized;

		var bulletPrefab = BulletPrefab;

		var bulletInstance = Instantiate(bulletPrefab.gameObject, ownPosition, _transform.rotation);
		var bullet = bulletInstance.GetComponent<IBullet>();
		ConfigureBullet(bulletInstance, bullet, bulletTravelDirection);
	}

	private void ConfigureBullet(GameObject bulletInstance, IBullet bullet, Vector2 bulletTravelDirection)
	{
		base.ConfigureBullet(bulletInstance);

        var antiAccuracy = Properties.GetValueOrDefault(GunProperties.Accuracy, 0);
        if (antiAccuracy != 0)
        {
            var random = Random.Range(-1f, 1f);
            var throwOffVector = Vector2.Perpendicular(bulletTravelDirection) * (random * antiAccuracy);
            bulletTravelDirection = throwOffVector + bulletTravelDirection;
        }

        var homingStrength = Properties.GetValueOrDefault(GunProperties.Homing, 0);

        if (homingStrength > 0)
        {
            var homing = bulletInstance.AddComponent<Homing>();
            homing.UnlimitedHoming = true;
            homing.HomingStrength = homingStrength;
            var targetAcquirer = bulletInstance.AddComponent<AutoAquireHomingTarget>();
            targetAcquirer.HomesInOn = Damages;
        }
        
		bullet.Direction = bulletTravelDirection;
	}
}