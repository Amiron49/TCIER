using System;
using Helpers;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private BulletEmitter[] _guns;
    private Transform _transform;
    private Vector3 _defaultOrientation = Vector2.up;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _guns = GetComponentsInChildren<BulletEmitter>();
        var bulletPrefab = UnityEngine.Resources.Load<GameObject>("Bullets/PhysicalBullet/PhysicalBullet") ?? throw new Exception($"Couldn't find prefab 'PhysicalBullet'");

        foreach (var gun in _guns)
            gun.bulletPrefab = bulletPrefab;
    }


    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.State.GameTime.Paused)
            return;
        
        var target = Game.Instance.LegacyControls.MouseWorldPosition;
        _transform.LookAt2d(target);

        if (!Game.Instance.Controls.Player.Fire.IsPressed()) 
            return;

        foreach (var gun in _guns)
            gun.Shoot();
    }
}