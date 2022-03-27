using System.Collections.Generic;
using InternalLogic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletEmitter : MonoBehaviour
{
	[FormerlySerializedAs("damages")] public Team Damages;
	private float _cooldown;
	[FormerlySerializedAs("bulletPrefab")] public GameObject BulletPrefab;
	public GameObject DefaultBulletPrefab;
	private Transform _transform;

	public Dictionary<GunProperties, float> Properties = new()
	{
		{ GunProperties.Cooldown, 1 },
		{ GunProperties.Damage, 10 }
	};

	// Start is called before the first frame update
	void Start()
	{
		_transform = transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (Game.Instance.State.GameTime.Paused)
			return;

		if (_cooldown > 0)
			_cooldown -= Time.deltaTime;
	}

	public void Shoot(Vector3 direction)
	{
		if (_cooldown > 0)
			return;

		EmitBullet(direction);
	}

	public void Shoot()
	{
		if (_cooldown > 0)
			return;

		EmitBullet(_transform.up);
	}

	public void EmitBullet(Vector3 direction)
	{
		_cooldown = Properties[GunProperties.Cooldown];

		var ownPosition = _transform.position;
		var bulletTravelDirection = direction.normalized;

		var bulletPrefab = BulletPrefab;

		if (BulletPrefab == null)
		{
			bulletPrefab = DefaultBulletPrefab;
		}

		var bulletInstance = Instantiate(bulletPrefab, ownPosition, _transform.rotation);
		var bullet = bulletInstance.GetComponent<IBullet>();
		ConfigureBullet(bulletInstance, bullet, bulletTravelDirection);
	}

	private void ConfigureBullet(GameObject bulletInstance, IBullet bullet, Vector3 bulletTravelDirection)
	{
		var damageConfig = bulletInstance.GetComponent<IDamageSource>();

		gameObject.layer = Damages.DamageToLayer();
		damageConfig.For = Damages;
		damageConfig.Damage = Properties[GunProperties.Damage];
		bullet.Direction = bulletTravelDirection;
	}
}