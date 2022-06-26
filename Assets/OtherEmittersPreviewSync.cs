using System;
using System.Linq;
using Menu;
using UnityEngine;

public class OtherEmittersPreviewSync : MonoBehaviour
{
    public GunIndexProvider GunIndexProvider;
    public EmitterPositionPreviewSync PreviewPrefab;
    public EmitterPositionEditor RealEditor;

    private ChildrenSync<EmitterPositionPreviewSync> _childrenSync;

    // Start is called before the first frame update
    void Start()
    {
        _childrenSync = new ChildrenSync<EmitterPositionPreviewSync>(gameObject, ChildFactory);
        Game.Instance.State.Inventory.Body.OnGunCountChange += BodyOnOnGunCountChange;
        _childrenSync.Change(Game.Instance.State.Inventory.Body.Guns.Count);
    }

    private void BodyOnOnGunCountChange(object sender, int difference)
    {
        Debug.Log("gun count change " + difference);
        _childrenSync.Change(difference);
    }

    private EmitterPositionPreviewSync ChildFactory(int arg)
    {
        var emitterPositionPreviewSync = Instantiate(PreviewPrefab, transform);

        emitterPositionPreviewSync.AssociatedGun = Game.Instance.State.Inventory.Body.Guns[arg];
        emitterPositionPreviewSync.RealEditor = RealEditor;

        var gunIndex = GunIndexProvider.gunIndex;
        if (arg == gunIndex)
            emitterPositionPreviewSync.gameObject.SetActive(false);
        
        return emitterPositionPreviewSync;
    }

    private void OnDestroy()
    {
        Game.Instance.State.Inventory.Body.OnGunCountChange -= BodyOnOnGunCountChange;
    }
}
