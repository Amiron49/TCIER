using InternalLogic;
using JetBrains.Annotations;
using Lightning;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public EnemyDefinition Definition;
	[CanBeNull] public GameObject DeathAnimation;

	// Start is called before the first frame update
	void Awake()
	{
		Init();
	}

	private bool _initiated = false;

	public void Init()
	{
		if (_initiated)
			return;

		var gameObjectWtfUnity = gameObject;
		_initiated = true;

		gameObjectWtfUnity.AddComponent<PropagateFriendlyProjectiles>();
		gameObjectWtfUnity.AddComponent<MakeKinematicOnGamePause>();
		if (Definition.ConduitsLightning)
		{
			var conduitEnemyLightning = gameObjectWtfUnity.AddComponent<ConduitEnemyLightning>();
			conduitEnemyLightning.conduitFrom = gameObjectWtfUnity;
		}
		var life = gameObjectWtfUnity.AddComponent<Life>();
		life.health = Definition.Health;
		life.team = Team.Enemy;

		var spawnMoney = gameObjectWtfUnity.AddComponent<SpawnMoneyOnDeath>();
		spawnMoney.ChunkThreshold = Definition.MoneyAward / 5;
		spawnMoney.TotalMoney = Definition.MoneyAward;

		if (DeathAnimation == null)
		{
			DeathAnimation = Game.Instance.Prefabs.Enemy.DefaultDeathAnimation;
		}

		Instantiate(Game.Instance.Prefabs.Enemy.EnemyInfo, transform);
		
		life.OnDeath += (_, _) =>
		{
			Instantiate(DeathAnimation, transform.position, quaternion.identity);
		};
	}
}