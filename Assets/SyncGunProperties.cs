using System;
using Helpers;
using InternalLogic;
using UnityEngine;

public class SyncGunProperties : MonoBehaviour
{
	private Transform _transform;
	public Gun AssociatedGun { get; set; }
	private BulletEmitterManager _emitter;

	// Start is called before the first frame update
	void Start()
	{
		_emitter = this.GetComponentStrict<BulletEmitterManager>();
		AssociatedGun.OnOffsetChange += AssociatedGunOnOnOffsetChange;
		AssociatedGun.OnRotationChange += AssociatedGunOnOnRotationChange;
		AssociatedGun.OnBulletChange += AssociatedGunOnOnBulletChange;
		AssociatedGun.OnPropertyChange += AssociatedGunOnOnPropertyChange;
		_transform = transform;
        ForceSync();
    }

	private void AssociatedGunOnOnPropertyChange(object sender, EventArgs e)
	{
		_emitter.Properties = AssociatedGun.PropertiesTyped;
	}

	private void AssociatedGunOnOnBulletChange(object sender, IBulletEquipConfig e)
	{
		var projectile = e.BulletPrefab.GetComponentStrict<IProjectile>();
		_emitter.ChangeBullet(projectile);
	}

	private void AssociatedGunOnOnRotationChange(object sender, Quaternion rotation)
	{
		_transform.rotation = rotation;
	}

	private void AssociatedGunOnOnOffsetChange(object sender, Vector2 offset)
	{
		_transform.localPosition = offset;
	}

    public void ForceSync()
    {
        AssociatedGunOnOnOffsetChange(this, AssociatedGun.Offset);
        AssociatedGunOnOnRotationChange(this, AssociatedGun.Rotation);
        AssociatedGunOnOnBulletChange(this, AssociatedGun.Bullet);
        AssociatedGunOnOnPropertyChange(this, EventArgs.Empty);
    }

    private void OnDestroy()
	{
		AssociatedGun.OnOffsetChange -= AssociatedGunOnOnOffsetChange;
		AssociatedGun.OnRotationChange -= AssociatedGunOnOnRotationChange;
		AssociatedGun.OnBulletChange -= AssociatedGunOnOnBulletChange;
		AssociatedGun.OnPropertyChange -= AssociatedGunOnOnPropertyChange;
	}
}