using System;
using System.Linq;
using UnityEngine;

public class SeekPlayer : MonoBehaviour
{
    public float speed = 2;
    
    private Transform _playerTransform;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    
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
        var memeExplosion = Resources.Load<GameObject>("Effects/MemeExplosion") ?? throw new Exception("Couldn't find prefab");
        Instantiate(memeExplosion, _transform.position, _transform.rotation);
    }
}
