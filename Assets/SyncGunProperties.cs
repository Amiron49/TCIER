using System;
using Helpers;
using InternalLogic;
using UnityEngine;

public class SyncGunProperties : MonoBehaviour
{
    private Transform _transform;
    public Gun AssociatedGun { get; set; }
    private BulletEmitter _emitter;

    // Start is called before the first frame update
    void Start()
    {
        _emitter = this.GetComponentStrict<BulletEmitter>();
        AssociatedGun.OnOffsetChange += AssociatedGunOnOnOffsetChange;
        AssociatedGun.OnRotationChange += AssociatedGunOnOnRotationChange;
        AssociatedGun.OnBulletChange += AssociatedGunOnOnBulletChange;
        AssociatedGun.OnPropertyChange += AssociatedGunOnOnPropertyChange;
        _transform = transform;
    }

    private void AssociatedGunOnOnPropertyChange(object sender, EventArgs e)
    {
        _emitter.Properties = AssociatedGun.PropertiesTyped;
    }

    private void AssociatedGunOnOnBulletChange(object sender, IBulletEquipConfig e)
    {
        _emitter.BulletPrefab = e.BulletPrefab;
    }

    private void AssociatedGunOnOnRotationChange(object sender, Quaternion rotation)
    {
        _transform.rotation = rotation;
    }

    private void AssociatedGunOnOnOffsetChange(object sender, Vector2 offset)
    {
        _transform.localPosition = offset;
    }

    private void OnDestroy()
    {
        AssociatedGun.OnOffsetChange -= AssociatedGunOnOnOffsetChange;
        AssociatedGun.OnRotationChange -= AssociatedGunOnOnRotationChange;
        AssociatedGun.OnBulletChange -= AssociatedGunOnOnBulletChange;
        AssociatedGun.OnPropertyChange -= AssociatedGunOnOnPropertyChange;
    }
}