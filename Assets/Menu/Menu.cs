using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
	public class Menu : MonoBehaviour
	{
		public MenuPage Current;
		public MenuPage DefaultPage;
		public GameObject PagesContainer;
		public string Name;
		private List<MenuPage> _pages;
		public Button NextButton;
		public TMP_Text NextText;
		public Button PreviousButton;
		public TMP_Text PreviousText;
		public TMP_Text CurrentPageText;
		[CanBeNull] private GameObject _lastSelected = null;

		private void Awake()
		{
			Game.Instance.State.GameTime.OnPauseChange += (_, paused) =>
			{
				if (paused)
					Open();
				else
					Close();
			};

			gameObject.SetActive(false);
		}

		void Open()
		{
			gameObject.SetActive(true);
			if (_lastSelected != null)
			{
				EventSystem.current.SetSelectedGameObject(_lastSelected);
			}
		}

		void Close()
		{
			_lastSelected = EventSystem.current.currentSelectedGameObject;
			gameObject.SetActive(false);
		}

		// Start is called before the first frame update
		void Start()
		{
			var foundPages = GetComponentsInChildren<MenuPage>();

			if (foundPages.Length == 0)
				throw new Exception($"The '{Name}' Menu has no MenuPages defined");

			_pages = foundPages.ToList();

			if (DefaultPage == null)
				DefaultPage = foundPages[0];
			else if (!_pages.Contains(DefaultPage))
				throw new Exception("Default page was misconfigured");

			foreach (var page in _pages)
			{
				page.gameObject.SetActive(false);
			}

			NextButton.onClick.AddListener(Next);
			PreviousButton.onClick.AddListener(Previous);

			Current = DefaultPage;
			RefreshState();
		}
		
		public void Next()
		{
			var next = GetNextPage();

			if (next == null)
				return;
			
			Current!.gameObject.SetActive(false);
			Current = next;
			EventSystem.current.SetSelectedGameObject(NextButton.gameObject);
			RefreshState();
		}

		public void Previous()
		{
			var previous = GetPrevious();

			if (previous == null)
				return;
			
			Current!.gameObject.SetActive(false);
			Current = previous;
			EventSystem.current.SetSelectedGameObject(PreviousButton.gameObject);
			RefreshState();
		}

		public void Hide()
		{
		}

		public void Show()
		{
		}

		private void RefreshState()
		{
			Current!.gameObject.SetActive(true);
			CurrentPageText.text = Current!.UserFriendlyName;
			UpdatePageButtonAndText(GetNextPage(), NextText, NextButton.gameObject);
			UpdatePageButtonAndText(GetPrevious(), PreviousText, PreviousButton.gameObject);
		}

		private static void UpdatePageButtonAndText([CanBeNull] MenuPage page, TMP_Text text, GameObject button)
		{
			if (page == null)
			{
				text.gameObject.SetActive(false);
				button.gameObject.SetActive(false);
				return;
			}

			text.gameObject.SetActive(true);
			button.gameObject.SetActive(true);
			text.text = page.UserFriendlyName ?? page.InternalIdentifier;
		}

		[CanBeNull]
		private MenuPage GetNextPage()
		{
			var currentIndex = _pages.IndexOf(Current!);
			var isEnd = currentIndex >= _pages.Count - 1;
			return isEnd ? null : _pages[currentIndex + 1];
		}

		[CanBeNull]
		private MenuPage GetPrevious()
		{
			var currentIndex = _pages.IndexOf(Current!);
			var isEnd = currentIndex <= 0;
			return isEnd ? null : _pages[currentIndex - 1];
		}

		public void AddPage(MenuPage menuPage)
		{
			if (_pages.Any(x => x.InternalIdentifier == menuPage.InternalIdentifier))
				throw new Exception($"Cannot add MenuPage {menuPage.InternalIdentifier} identifier: Duplicate identifier");
			
			_pages.Add(menuPage);
			RefreshState();
		}
		
		public void Remove(string internalIdentifier)
		{
			_pages.RemoveAll(x => x.InternalIdentifier == internalIdentifier);
			RefreshState();
		}

		// Update is called once per frame
		void Update()
		{
			if (Game.Instance.Controls.UI.TabNext.WasPressedThisFrame())
			{
				Next();
			}
			if (Game.Instance.Controls.UI.TabPrevious.WasPressedThisFrame())
			{
				Previous();
			}
		}
	}
}