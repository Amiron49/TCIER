using System;
using System.Collections;
using UnityEngine;

public class ReactToHealthChanges : MonoBehaviour
{
	[SerializeField] private HudBar _healthBar;
	[SerializeField] private WackyPoppingUpNumber _wackyPoppingUpNumberPrefab;
	private Life _life;

	// Start is called before the first frame update
	void Start()
	{
		var root = gameObject.transform.parent.gameObject;

		_life = root.GetComponent<Life>();
		_life.OnHealthChange += LifeOnOnHealthChange;
		_healthBar.SetFill(1);
		_healthBar.gameObject.SetActive(false);
	}

	private bool _firstTimeChange = true;

	private void LifeOnOnHealthChange(object sender, float from, float to)
	{
		ApplyHealthBarChange(to);
		SpawnDamageNumberVisualization(from, to);
	}

	private void ApplyHealthBarChange(float to)
	{
		var maxHealth = _life.health;
		var percentage = to / maxHealth;

		if (_firstTimeChange)
		{
			_healthBar.gameObject.SetActive(true);
			_healthBar.SetFill(percentage);
			_firstTimeChange = false;
		}
		else
			_healthBar.GradualChangeFill(percentage);
	}

	private void SpawnDamageNumberVisualization(float from, float to)
	{
		var healthChangeNumber = Instantiate(_wackyPoppingUpNumberPrefab, transform);
		healthChangeNumber.Number = (int)Mathf.Abs(from - to);
		var healthGain = to > from;
		healthChangeNumber.Color = healthGain ? Color.green : Color.red;

	}
}