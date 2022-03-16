using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Popup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private RectTransform _popupBackground;
	private CanvasGroup _rootCanvasGroup;
	private CanvasGroup _ownGroup;

	private bool _mouseIsOver = false;

	// Start is called before the first frame update
	void Start()
	{
		Init();
		gameObject.SetActive(false);
	}

	private void Init()
	{
		if (_ownGroup != null)
			return;

		_rootCanvasGroup = this.GetComponentStrict<RectTransform>().root.GetComponentStrict<CanvasGroup>();
		_ownGroup = gameObject.AddComponent<CanvasGroup>();
		_ownGroup.ignoreParentGroups = true;
		_ownGroup.interactable = true;
	}

	public void Show()
	{
		Init();

		gameObject.SetActive(true);
		_ownGroup.interactable = true;
		_rootCanvasGroup.interactable = false;
	}

	private void Update()
	{
		if (Game.Instance.Controls.UI.Cancel.IsPressed() || !_mouseIsOver && Game.Instance.Controls.UI.Click.IsPressed())
		{
			Hide();
		}
	}

	public void Hide()
	{
		_rootCanvasGroup.interactable = true;
		_ownGroup.interactable = false;
		gameObject.SetActive(false);
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		_mouseIsOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_mouseIsOver = false;
	}
}