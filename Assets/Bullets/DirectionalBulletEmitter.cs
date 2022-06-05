using UnityEngine;

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
		EmitBullet(_transform.up);

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

	private void ConfigureBullet(GameObject bulletInstance, IBullet bullet, Vector3 bulletTravelDirection)
	{
		base.ConfigureBullet(bulletInstance);
		bullet.Direction = bulletTravelDirection;
	}
}