using Helpers;
using UnityEngine;

public class ControlManager
{
	public Vector3 MoveDirection { get; private set; }
	public Vector3 MousePosition { get; private set; }
	public Vector3 MouseWorldPosition { get; private set; }
	public bool Dodge { get; private set; }
	public bool Shoot { get; private set; }

	public void Update()
	{
		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		MoveDirection = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1);
		MoveDirection.Normalize();
		Dodge = Input.GetButtonDown("Jump");
		Shoot = Input.GetButton("Fire1");
		MousePosition = Input.mousePosition;
		MouseWorldPosition = Camera.main.ScreenToWorldPoint(MousePosition).NoZ();
	}
}