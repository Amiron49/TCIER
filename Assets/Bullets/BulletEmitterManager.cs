using System;
using System.Collections.Generic;
using InternalLogic;
using UnityEngine;

public class BulletEmitterManager: MonoBehaviour, IBulletEmitter 
{
	public Dictionary<GunProperties, float> Properties
	{
		get => _properties;
		set
		{
			_currentActive.Properties = value;
			_properties = value;
		}
	}

	public float Cooldown => _currentActive.Cooldown;
	public float MaxCooldown => _currentActive.MaxCooldown;

	private DirectionalBulletEmitter _directionalBulletEmitter; 
	private LightningEmitter _lightningEmitter; 
	private IBulletEmitter _currentActive;

	public Team damagesDefault;
	private Dictionary<GunProperties, float> _properties;

	public Team Damages
	{
		set => _currentActive.Damages = value;
	}

	public void Shoot()
	{
		_currentActive.Shoot();
	}

	public void ChangeBullet(IProjectile projectile)
	{
		switch (projectile)
		{
			case IZap asZap:
				_currentActive = SpawnOrConfigDirectionalLightningEmitter(asZap);
				break;
			case IBullet asBullet:
				_currentActive = SpawnOrConfigDirectionalBulletEmitter(asBullet);
				break;
			default:
				throw new Exception($"Projectile of this type is not supported ({projectile.GetType().Name})");
		}
		
		_currentActive.Damages = damagesDefault;
		_currentActive.Properties = _properties;
	}

	private IBulletEmitter SpawnOrConfigDirectionalBulletEmitter(IBullet projectile)
	{
		if (_directionalBulletEmitter == null)
		{
			_directionalBulletEmitter = gameObject.AddComponent<DirectionalBulletEmitter>();
		}

		_directionalBulletEmitter.BulletPrefab = (DirectionalBullet)projectile;

		return _directionalBulletEmitter;
	}
	
	private IBulletEmitter SpawnOrConfigDirectionalLightningEmitter(IZap projectile)
	{
		if (_lightningEmitter == null)
		{
			_lightningEmitter = gameObject.AddComponent<LightningEmitter>();
		}

		_lightningEmitter.LightningZapPrefab = projectile;
		_lightningEmitter.Damages = damagesDefault;
		return _lightningEmitter;
	}
}