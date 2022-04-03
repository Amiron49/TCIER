using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

public class LookAtPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    private Transform _transform;
    public float RotationSpeed = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        var o = gameObject;
        _transform = o.transform;
        _playerTransform = Game.Instance.State.Player.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.State.GameTime.Paused)
            return;
        
        _transform.LookAt2dQuad(_playerTransform.position,  Time.deltaTime * RotationSpeed);
    }
}
