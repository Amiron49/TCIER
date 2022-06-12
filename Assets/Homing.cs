using Helpers;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public Transform Target;
    private Transform _ownTransform;
    public float HomingStrength = 0.1f;
    public float HomingJuice = 1f;
    public bool UnlimitedHoming;
    private PhysicalDirectionalBullet _physicalDirectionalBullet;

    // Start is called before the first frame update
    void Start()
    {
        _physicalDirectionalBullet = this.GetComponentStrict<PhysicalDirectionalBullet>();
        _ownTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.State.GameTime.Paused || Target == null)
            return;

        var targetDirection = (Target.position - _ownTransform.position).normalized;

        _physicalDirectionalBullet.direction = Vector3.RotateTowards(_physicalDirectionalBullet.direction, targetDirection,
            HomingStrength * Game.Instance.State.GameTime.DeltaTime, 0);
    }
}