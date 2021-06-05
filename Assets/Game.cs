using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }
	public readonly ControlManager ControlManager = new ControlManager();
	public readonly State State = new State();

	public List<EnemyDefinition> enemies = new List<EnemyDefinition>();
	
	private void Start()
	{
		Instance = this;
	}
}