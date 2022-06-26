using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public GameObject bubbleTextPrefab;
	public Prefabs Prefabs;
	public static Game Instance { get; private set; }
	public ControlManager LegacyControls { get; private set; }
	public TCIERControls Controls { get; private set; }
	public State State { get; private set; }
	public Camera MainCamera { get; private set; }
	public BulletDefinition StartingBulletEquipConfig;

	public List<EnemyDefinition> enemies = new List<EnemyDefinition>();

	private void Awake()
	{
        //Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 1;
		Controls = new TCIERControls();
		Controls.Enable();
		Controls.Player.TogglePause.performed += _ => State.TogglePause();
		Instance = this;
		State = new State(gameObject, enemies, StartingBulletEquipConfig);
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
		
		//State.AddMoney(10000);

		StartCoroutine(Debug());
	}

	private IEnumerator Debug()
	{
		yield return new WaitForSeconds(0.1f);
		State.Inventory.Body.Guns.First().Equip(StartingBulletEquipConfig);
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