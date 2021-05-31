using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
	public float fireRate = 2f;
	public Team damages;
	private float _cooldown;
	public GameObject bulletPrefab;
	private Transform _transform;
    
	// Start is called before the first frame update
	void Start()
	{
		_transform = transform;
	}

	private float CalculateCooldown()
	{
		return 1 / fireRate;
	}

	// Update is called once per frame
	void Update()
	{   
		if (_cooldown > 0)
			_cooldown -= Time.deltaTime;
	}

	public void Shoot(Vector3 direction)
	{
		if (_cooldown > 0)
			return;
        
		EmitBullet(direction);
	}
	
	public void Shoot()
	{
		if (_cooldown > 0)
			return;

		EmitBullet(_transform.up);
	}
	
	public void EmitBullet(Vector3 direction)
	{
		_cooldown = CalculateCooldown();

		var ownPosition = _transform.position;
		var bulletTravelDirection = direction.normalized;
            
		var bulletInstance = Instantiate(bulletPrefab, ownPosition, _transform.rotation);
		var bullet = bulletInstance.GetComponent<IBullet>();
		var damageConfig = bulletInstance.GetComponent<IDamageSource>();

		damageConfig.For = damages;		
		bullet.Direction = bulletTravelDirection;
	}
}