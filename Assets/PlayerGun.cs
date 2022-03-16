using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers;
using InternalLogic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public BulletEmitter PlayerGunPrefab;
    public PhysicalBullet DefaultBulletPrefab;
    private Transform _transform;
    private Vector3 _defaultOrientation = Vector2.up;

    private List<(Gun gun, BulletEmitter emitter)> _managedGuns = new();

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        Game.Instance.State.Inventory.Body.OnGunCountChange += (_, _) =>
        {
            RefreshGuns();
        };
    }

    private void RefreshGuns()
    {
        var expectedGuns = Game.Instance.State.Inventory.Body.Guns;

        var toBeRemoved = _managedGuns.Where(x => !expectedGuns.Contains(x.gun)).ToList();
        var toBeAdded = expectedGuns.Where(expectedGun => _managedGuns.All(existing => existing.gun != expectedGun));

        foreach (var gun in toBeRemoved)
        {
            Destroy(gun.emitter);
            _managedGuns.Remove(gun);
        }

        Add(toBeAdded);
    }

    private void Add(IEnumerable<Gun> guns)
    {
        foreach (var gun in guns)
        {
            CreateEmitter(gun);
        }
    }

    
    private void CreateEmitter(Gun gun)
    {
        var emitter = Instantiate(PlayerGunPrefab);

        var syncer = emitter.GetComponentStrict<SyncGunProperties>();

        syncer.AssociatedGun = gun;
        
        _managedGuns .Add((gun, emitter));
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.State.GameTime.Paused)
            return;
        
        //TODO controller lol
        var target = Game.Instance.LegacyControls.MouseWorldPosition;
        _transform.LookAt2d(target);

        if (!Game.Instance.Controls.Player.Fire.IsPressed()) 
            return;

        foreach (var (_, emitter) in _managedGuns)
            emitter.Shoot();
    }
}