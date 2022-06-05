using UnityEngine;

public class PhysicalDirectionalBullet : DirectionalBullet
{
    public float velocity = 10f;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.Instance.State.GameTime.Paused)
            return;
        
        var currentPosition = _transform.position;
        var targetPosition = CalculateNextPosition(currentPosition);
        _transform.position = Vector3.Lerp(currentPosition, targetPosition, Game.Instance.State.GameTime.DeltaTime);
    }
    
    private Vector3 CalculateNextPosition(Vector3 start)
    {
        return start + direction * velocity;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}

public interface IBullet: IProjectile
{
    public Vector3 Direction { get; set; }
}

public abstract class DirectionalBullet: MonoBehaviour, IBullet
{
    public Vector3 direction = Vector3.up;
    public Vector3 Direction
    {
        get => direction;
        set => direction = value;
    }
}

public interface IProjectile
{
    // ReSharper disable once InconsistentNaming
    public GameObject gameObject { get ; } 
}