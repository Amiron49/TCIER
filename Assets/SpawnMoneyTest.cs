using UnityEngine;

public class SpawnMoneyTest : MonoBehaviour
{
	public GameObject target;
	[Range(0.1f, 40f)] public float speed = 20;
	[Range(0.1f, 40f)] public float wackSpeed = 8;
	[Range(0.1f, 10f)] public float wackFollow = 1;
	[Range(0.1f, 3f)] public float delay;
	public float waited;
	public GameObject moneyPrefab;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		waited += Time.deltaTime;

		if (waited <= delay)
			return;

		if (delay < Time.smoothDeltaTime)
		{
			var howMany = (int) (Time.smoothDeltaTime / delay) + 1;
			for (var i = 0; i < howMany; i++)
				Spawn();
		}
		else
		{
			Spawn();
		}

		waited = 0;
	}

	private void Spawn()
	{
		var instance = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
		var asHoming = instance.GetComponent<WackyHoming>();
		asHoming.to = target;
		asHoming.speed = speed;
		asHoming.wackSpeed = wackSpeed;
		asHoming.initialWackVectorFollowTime = wackFollow;
	}
}