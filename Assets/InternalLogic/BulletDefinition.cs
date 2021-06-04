using InternalLogic;
using UnityEngine;

[CreateAssetMenu]
public class BulletDefinition : ScriptableObject, IBulletEquipConfig
{
	public string description;
	public GameObject bulletPrefab;
	public float cooldown;
	public bool channeled;
	public float windUp;
	public float windDown;
	public string bulletName;

	public string Name => bulletName;
	public string Description => description;
	public GameObject BulletPrefab => bulletPrefab;
	public float Cooldown => cooldown;
	public bool Channeled => channeled;
	public float WindUp => windUp;
	public float WindDown => windDown;
}