using System.Collections.Generic;
using InternalLogic;
using UnityEngine;

public abstract class BulletEmitterBase : MonoBehaviour, IBulletEmitter
{
	[SerializeField] private Team _damages;

	public Team Damages
	{
		get => _damages;
		set => _damages = value;
	}

	public float Cooldown { get; private set; }
	public float MaxCooldown { get; private set; }

	public virtual Dictionary<GunProperties, float> Properties { get; set; } = new()
	{
		{ GunProperties.Cooldown, 1 },
		{ GunProperties.Damage, 10 }
	};

	private void Start()
	{
		ReInit();
	}

	public void ReInit()
	{
		MaxCooldown = Properties[GunProperties.Cooldown];
		ReInitInternal();
	}

	protected virtual void ReInitInternal()
	{
	}

	// Update is called once per frame

	void Update()
	{
		if (Game.Instance.State.GameTime.Paused)
			return;
		
		if (Cooldown > 0)
			Cooldown -= Game.Instance.State.GameTime.DeltaTime;
	}


	public void Shoot()
	{
		if (Cooldown > 0)
			return;

		var success = ShootInternal();

		if (success)
		{
			Cooldown = MaxCooldown;
		}
		else
		{
			Cooldown = MaxCooldown * 0.10f;
		}
	}

	protected abstract bool ShootInternal();
	
	protected void ConfigureBullet(GameObject bulletInstance)
	{
		var damageConfig = bulletInstance.GetComponent<IDamageSource>() ?? bulletInstance.AddComponent<DamageSource>();

		bulletInstance.layer = Damages.DamageToLayer();
		damageConfig.For = Damages;
		damageConfig.Damage = Properties[GunProperties.Damage];
	}
}