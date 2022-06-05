using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class WackyHoming : MonoBehaviour
{
	public GameObject to;
	public float speed;
	public float wackSpeed;
	public float initialWackVectorFollowTime;
	private float _aliveTime = 0;
	private Vector2 _initialWackVector;
	private Transform _target;
	private Transform _transform;
	private static Random _random = Random.CreateFromIndex((uint) DateTime.Now.Millisecond);

	// Start is called before the first frame update
	void Start()
	{
		_transform = transform;
		_target = to.transform;
		var randomX = _random.NextFloat2(-2, 2);
		var randomVector = new Vector2(randomX.x, randomX.y).normalized;
		_initialWackVector = randomVector;
	}

	// Update is called once per frame
	void Update()
	{
		if (Game.Instance.State.GameTime.Paused)
			return;
		
		var realVectorStrengthMod = _aliveTime / initialWackVectorFollowTime;

		var currentPosition = _transform.position;
		var normalVector = CalculateNormalVector(currentPosition, _target.position) * realVectorStrengthMod;
		var wackVector = CalculateWackVector();

		var targetVector = Vector3.Lerp(wackVector, normalVector, realVectorStrengthMod);
		
		var targetPosition = currentPosition + targetVector;
		_transform.position = Vector3.Lerp(currentPosition, targetPosition, Game.Instance.State.GameTime.DeltaTime);

		_aliveTime += Game.Instance.State.GameTime.DeltaTime;
	}

	private Vector3 CalculateNormalVector(Vector3 currentPosition, Vector3 playerPosition)
	{
		var direction = (playerPosition - currentPosition).normalized;
		return direction * speed;
	}
	
	private Vector3 CalculateWackVector()
	{
		var direction = _initialWackVector;
		return (Vector3)direction * wackSpeed;
	}
}