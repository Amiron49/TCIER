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
        var bulletPrefab = Resources.Load<GameObject>("PhysicalBullet") ?? throw new Exception("Couldn't find prefab");

        foreach (var gun in _guns)
            gun.bulletPrefab = bulletPrefab;
    }


    // Update is called once per frame
    void Update()
    {
        var target = Game.Instance.ControlManager.MouseWorldPosition;
        _transform.LookAt2d(target);

        if (!Game.Instance.ControlManager.Shoot) 
            return;

        foreach (var gun in _guns)
            gun.Shoot();
    }
}