using JetBrains.Annotations;
using StateMachine;
using UnityEngine;

public class SeekTargetState : StateBase
{
    [System.Serializable]
    public class SeekPlayerConfig
    {
        public float BaseSpeed;
        public float Speed => BaseSpeed * SpeedMultiplier.Multiplier;
        public SpeedMultiplier SpeedMultiplier;
    }

    private readonly Transform _target;
    private readonly SeekPlayerConfig _configuration;
    private readonly Rigidbody2D _self;

    public SeekTargetState(Rigidbody2D self, Transform target, SeekPlayerConfig configuration, [CanBeNull] string key = null) : base(
        key ?? nameof(SeekTargetState))
    {
        _self = self;
        _target = target;
        _configuration = configuration;
    }

    public override void Update()
    {
        var difference = (Vector2)_target.position - _self.position;
        var direction = difference.normalized;
        _self.velocity = direction * _configuration.Speed;
    }

}