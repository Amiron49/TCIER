using System;
using System.Linq;
using UnityEngine;

public class SeekPlayer : MonoBehaviour
{
    public float speed = 2;
    
    private Transform _playerTransform;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    public GameObject DeathExplosion;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        var player = gameObject.scene.GetRootGameObjects().Single(x => x.CompareTag("Player"));
        _playerTransform = player.transform;
    }

    // Update is called once per frame
    public void Update()
    {
        
		
    }

    private void FixedUpdate()
    {
        if (Game.Instance.State.GameTime.Paused)
            return;
        
        var currentPosition = _transform.position;
        var targetPosition = CalculateNextPosition(currentPosition, _playerTransform.position);
        _rigidbody.MovePosition(targetPosition);
    }

    private Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 playerPosition)
    {
        var direction = (playerPosition - currentPosition).normalized;
        return currentPosition + direction * (speed * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        Instantiate(DeathExplosion, _transform.position, _transform.rotation);
    }
}
