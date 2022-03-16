using InternalLogic;
using UnityEngine;

public class SyncGunProperties : MonoBehaviour
{
    private Transform _transform;
    public Gun AssociatedGun { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        AssociatedGun.OnOffsetChange += AssociatedGunOnOnOffsetChange;
        AssociatedGun.OnRotationChange += AssociatedGunOnOnRotationChange;
        _transform = transform;
    }

    private void AssociatedGunOnOnRotationChange(object sender, Quaternion rotation)
    {
        _transform.rotation = rotation;
    }

    private void AssociatedGunOnOnOffsetChange(object sender, Vector2 offset)
    {
        _transform.position = offset;
    }

    private void OnDestroy()
    {
        AssociatedGun.OnOffsetChange -= AssociatedGunOnOnOffsetChange;
        AssociatedGun.OnRotationChange -= AssociatedGunOnOnRotationChange;
    }
}
