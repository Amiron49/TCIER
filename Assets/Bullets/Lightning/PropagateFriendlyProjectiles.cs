using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

public class PropagateFriendlyProjectiles: MonoBehaviour
{

	public float MaxRadius = 20;

	private Transform _transform;
	
	private void Start()
	{
		_transform = transform;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		PropagateMaybe(other.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		PropagateMaybe(other.gameObject);
	}

	public int PropagateMaybe(GameObject collidingObject)
	{
		var projectile = collidingObject.GetComponents<IProjectile>().FirstOrDefault() ?? throw new Exception("Missing IProjectile on a propagating projectile. Lol?");

		return Propagate(projectile);
	}

	public int Propagate(IProjectile projectile)
	{
		var originalPropagationState = projectile.gameObject.GetComponentStrict<PropagatedFriendlyProjectile>();

		if (originalPropagationState.PropagationCountRemaining <= 0)
			return 0;
		
		var potentialTargets = FindPotentialTargets(originalPropagationState.MaxTargets);
		var ownPosition = _transform.position;
		
		foreach (var potentialTarget in potentialTargets)
		{
			var targetPosition = potentialTarget.gameObject.transform.position;
			var differenceToTarget = targetPosition - ownPosition;

			var spawnPosition = ownPosition + differenceToTarget.normalized * 0.1f;
			var projectileClone = Clone(projectile,  spawnPosition);

			switch (projectileClone)
			{
				case IBullet bullet:
					ApplyTypeSpecifics(bullet, differenceToTarget.normalized);
					break;
				case IZap zap:
					ApplyTypeSpecifics(zap, spawnPosition, targetPosition, potentialTarget.gameObject);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(projectileClone), "Projectile type needs to added here.");
			}
		}

		return potentialTargets.Length;
	}

	private void ApplyTypeSpecifics(IZap zap, Vector2 from, Vector2 to, GameObject potentialTargetGameObject)
	{
		var zapDamageComponent = zap.gameObject.GetComponentStrict<IDamageSource>(); 
		zap.From = from;
		zap.To = to;	
		zap.OnZapStart += (_, _) =>
		{
			var gameObjectIsDestroyed = potentialTargetGameObject == null;
			if (gameObjectIsDestroyed)
				return;
			
			var lifeOfTarget = potentialTargetGameObject.GetComponentStrict<Life>();
			var propagatorOfTarget = potentialTargetGameObject.GetComponentStrict<PropagateFriendlyProjectiles>();
			propagatorOfTarget.PropagateMaybe(zap.gameObject);
			lifeOfTarget.TakeDamage(zapDamageComponent);
		};
	}

	private static void ApplyTypeSpecifics(IBullet bullet, Vector3 targetDirection)
	{
		bullet.Direction = targetDirection;
	}

	private static IProjectile Clone(IProjectile projectile, Vector2 position)
	{
		var clone = Instantiate(projectile.gameObject, position, Quaternion.identity);
		var propagation = clone.GetComponentStrict<PropagatedFriendlyProjectile>();
		propagation.PropagationCountRemaining--;
		return clone.GetComponent<IProjectile>();
	}

	private GameObject[] FindPotentialTargets(int maxResults)
	{
		var selfResultTolerance = 1;
		var results = new Collider2D[maxResults + selfResultTolerance];
		Physics2D.OverlapCircleNonAlloc(transform.position, MaxRadius / 2, results, LayerMask.GetMask("Enemy Swarmers", "Enemy Others"));
		var foundTargets = results.Where(x => x != null).ToArray();
		var filtered = foundTargets.Select(x => x.gameObject).Where(x => x != gameObject);
		return filtered.Take(maxResults).ToArray();
	}
}

public class PropagatedFriendlyProjectile: MonoBehaviour
{
	[FormerlySerializedAs("PropagationCount")] public int PropagationCountRemaining = 0;
	public int MaxTargets = 1;

}
