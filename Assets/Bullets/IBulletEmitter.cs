using System.Collections.Generic;
using InternalLogic;

public interface IBulletEmitter
{
	public Team Damages { set; }
	void Shoot();
	Dictionary<GunProperties, float> Properties { get; set; }
	float Cooldown { get; }
	float MaxCooldown { get; }
}