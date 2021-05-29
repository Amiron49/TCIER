using System;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private Gun _gun;
    private Transform _transform;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _gun = GetComponent<Gun>();
        var bulletPrefab = Resources.Load<GameObject>("PhysicalBullet") ?? throw new Exception("Couldn't find prefab");

        _gun.bulletPrefab = bulletPrefab;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Game.ControlManager.Shoot) 
            return;
        
        var aimedAtDirection = Game.ControlManager.MouseWorldPosition;
        _gun.Shoot(aimedAtDirection);
    }
}