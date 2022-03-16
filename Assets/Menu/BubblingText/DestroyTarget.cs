using UnityEngine;

namespace Menu.UI.BubblingText
{
	public class DestroyTarget : MonoBehaviour
	{
		public GameObject destroyTarget;

		public void DestroyGameObject()
		{
			Destroy(destroyTarget);
		}
	}
}
