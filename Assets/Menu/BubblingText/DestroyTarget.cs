using UnityEngine;

namespace Menu.BubblingText
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
