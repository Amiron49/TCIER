using System.Collections.Generic;
using InternalLogic;
using StateMachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject bubbleTextPrefab;
	public static Game Instance { get; private set; }
	public ControlManager LegacyControls { get; private set; }
	public TCIERControls Controls { get; private set; }
	public State State { get; private set; }

	public List<EnemyDefinition> enemies = new List<EnemyDefinition>();

	private void Awake()
	{
		Controls = new TCIERControls();
		Controls.Enable();
		Controls.Player.TogglePause.performed += _ => State.TogglePause();
		Instance = this;
		State = new State(enemies);
		LegacyControls = new ControlManager();
		State.GameTime.OnPauseChange += (_, isPaused) =>
		{
			// Time.timeScale = isPaused ? 0 : 1;
		};
		State.AddMoney(20000);
	}

	private void Update()
	{
		LegacyControls.Update();
	}

	public void TogglePause()
	{
		State.TogglePause();
	}
}

public static class GameObjectExtensions
{
	public static void BubbleText(Vector3 position, string text)
	{
		var bubbleText = Object.Instantiate(Game.Instance.bubbleTextPrefab, position, Quaternion.identity);
		var textObject = bubbleText.GetComponentInChildren<TMP_Text>();
		textObject.text = text;
	}

	public static void BubbleTextOnMe(this GameObject gameObject, string text, Vector3? offset = null)
	{
		var position = gameObject.transform.position + offset ?? Vector3.zero;
		var bubbleText = Object.Instantiate(Game.Instance.bubbleTextPrefab, position, Quaternion.identity, gameObject.transform);
		var textObject = bubbleText.GetComponentInChildren<TMP_Text>();
		textObject.text = text;
	}
}