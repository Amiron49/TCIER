using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoShoot : MonoBehaviour
{
    [FormerlySerializedAs("test")] public Color ColorWhenShooting;
    private IBulletEmitter _emitter;
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _emitter = this.GetComponentStrict<IBulletEmitter>();
        _spriteRenderer = this.GetComponentStrict<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.State.GameTime.Paused)
            return;

        if (_emitter.Cooldown <= 0)
            _emitter.Shoot();

        var toCompletion = _emitter.Cooldown / _emitter.MaxCooldown;

        _spriteRenderer.color = Color.Lerp(ColorWhenShooting, _originalColor, toCompletion);
    }
}
