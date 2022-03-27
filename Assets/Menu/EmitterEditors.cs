using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
	public class EmitterEditors : MonoBehaviour
	{
		public Menu MainMenuManager;
		public MenuPage EmitterEditorPrefab;
		private List<GameObject> _editors;
		private ChildrenSync<MenuPage> _childrenSync;

		// Start is called before the first frame update
		void Start()
		{
			_childrenSync = new ChildrenSync<MenuPage>(gameObject, CreateEditor);
			_childrenSync.RememberAllCurrentChildren();
			_childrenSync.OnDestroy += (_, args) =>
			{
				Debug.Log($"event {args.GameObject.InternalIdentifier}");
				var (toBeRemoved, _) = args;
				var asMenuPage = toBeRemoved.GetComponent<MenuPage>();
				MainMenuManager.Remove(asMenuPage.InternalIdentifier);
			};
			
			Game.Instance.State.Inventory.Body.OnGunCountChange += (_, difference) =>
			{
				_childrenSync.Change(difference);
			};
		}

		private MenuPage CreateEditor(int index)
		{
			var newEmitterMenuPage = Instantiate(EmitterEditorPrefab, transform);
			newEmitterMenuPage.gameObject.SetActive(false);
			newEmitterMenuPage.InternalIdentifier = $"Gun#{index}";
			newEmitterMenuPage.UserFriendlyName = $"Gun#{index + 1}";
			newEmitterMenuPage.GetComponent<GunIndexProvider>().gunIndex = index;
			MainMenuManager.AddPage(newEmitterMenuPage);
			return newEmitterMenuPage;
		}
	}
}