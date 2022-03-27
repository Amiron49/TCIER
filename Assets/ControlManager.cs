using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager
{
	public Vector3 MousePosition { get; private set; }

	public Vector3 MouseWorldPosition { get; private set; }

	public void Update()
	{
		//Todo abstract away as cursor object
		var vector2Control = InputSystem.GetDevice<Mouse>().position;
		MousePosition = new Vector3(vector2Control.x.ReadValue(), vector2Control.y.ReadValue());
		MouseWorldPosition = Game.Instance.MainCamera.ScreenToWorldPoint(MousePosition).NoZ();
	}
}