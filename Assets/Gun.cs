using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public float fireRate = 2f;
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

	public void Shoot(Vector3 target)
	{
		if (_cooldown > 0)
			return;
        
		_cooldown = CalculateCooldown();

		var ownPosition = _transform.position;
		var bulletTravelDirection = (target - ownPosition).normalized;
            
		var bulletInstance = Instantiate(bulletPrefab, ownPosition, _transform.localRotation);
		var bullet = bulletInstance.GetComponent<IBullet>();

		bullet.Direction = bulletTravelDirection;
		Debug.Log(bulletTravelDirection);
	}
}