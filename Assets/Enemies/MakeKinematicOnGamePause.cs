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

    private void GameTimeOnOnPauseChange(object sender, bool e)
    {
        _selfRigidbody.isKinematic = e;
    }

    private void OnDestroy()
    {
        Game.Instance.State.GameTime.OnPauseChange -= GameTimeOnOnPauseChange;
    }
}
