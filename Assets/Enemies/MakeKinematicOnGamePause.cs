using UnityEngine;

public class MakeKinematicOnGamePause : MonoBehaviour
{
    private Rigidbody2D _selfRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody2D>();

        if (_selfRigidbody == null || _selfRigidbody.isKinematic)
        {
            Destroy(this);
            return;
        }
        
        Game.Instance.State.GameTime.OnPauseChange += GameTimeOnOnPauseChange;
    }

    private Vector2 _velocityBefore;
    
    private void GameTimeOnOnPauseChange(object sender, bool e)
    {
        _selfRigidbody.isKinematic = e;
        if (e)
        {
            _velocityBefore = _selfRigidbody.velocity;
            _selfRigidbody.velocity = Vector2.zero;
        }
        else
        {
            _selfRigidbody.velocity = _velocityBefore;
        }
    }

    private void OnDestroy()
    {
        Game.Instance.State.GameTime.OnPauseChange -= GameTimeOnOnPauseChange;
    }
}
