using System.Collections.Generic;
using System.Linq;
using Helpers;
using InternalLogic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public BulletEmitter PlayerGunPrefab;
    public PhysicalBullet DefaultBulletPrefab;
    private Transform _transform;
    private Vector3 _defaultOrientation = Vector2.up;

    private ChildrenSync<BulletEmitter, Gun> _emitterSync;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        _emitterSync = new ChildrenSync<BulletEmitter, Gun>(CreateEmitter);
        
        Game.Instance.State.Inventory.Body.OnGunCountChange += (_, _) =>
        {
            RefreshGuns();
        };
        
        RefreshGuns();
    }

    private void RefreshGuns()
    {
        var expectedGuns = Game.Instance.State.Inventory.Body.Guns;
        _emitterSync.Update(expectedGuns);
    }
    
    private BulletEmitter CreateEmitter(Gun gun)
    {
        var emitter = Instantiate(PlayerGunPrefab, transform);

        var syncer = emitter.GetComponentStrict<SyncGunProperties>();

        syncer.AssociatedGun = gun;

        return emitter;
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

        foreach (var (_, emitter) in _emitterSync.ManagedChildren)
            emitter.child.Shoot();
    }
}