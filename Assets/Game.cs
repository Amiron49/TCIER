using System.Collections.Generic;
using InternalLogic;
using Menu;
using Menu.ItemTiles;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public GameObject bubbleTextPrefab;
	public static Game Instance { get; private set; }
	public ControlManager LegacyControls { get; private set; }
	public TCIERControls Controls { get; private set; }
	public State State { get; private set; }
	public Camera MainCamera { get; private set; }

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
			if (isPaused)
			{
				Controls.UI.Enable();
			}
			else
			{
				Controls.UI.Disable();
			}
		};
		MainCamera = Camera.main;
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
	
	public static void BubbleTextOnMe(this ItemTile itemTile, string text)
	{
		var rectTransform = itemTile.GetComponent<RectTransform>();
		itemTile.gameObject.BubbleTextOnMe(text,  new Vector3(0, rectTransform.sizeDelta.y / 2, 0) );
	}
}