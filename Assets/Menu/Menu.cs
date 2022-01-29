using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [CanBeNull] public MenuPage Current;
        public MenuPage DefaultPage;
        public GameObject PagesContainer;
        public string Name;
        private List<MenuPage> Pages;
        public Button NextButton;
        public TMP_Text NextText;
        public Button PreviousButton;
        public TMP_Text PreviousText;
        public TMP_Text CurrentPageText;

        private void Awake()
        {
            Game.Instance.State.GameTime.OnPauseChange += (_, paused) =>
            {
                gameObject.SetActive(paused);
            };
        
            gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            var foundPages = GetComponentsInChildren<MenuPage>();

            if (foundPages.Length == 0)
                throw new Exception($"The '{Name}' Menu has no MenuPages defined");

            Pages = foundPages.ToList();

            if (DefaultPage == null)
                DefaultPage = foundPages[0];
            else if(!Pages.Contains(DefaultPage))
                throw new Exception("Default page was misconfigured");

            foreach (var page in Pages)
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
            Current!.gameObject.SetActive(false);
            Current = GetNextPage();
            RefreshState();
        }
    
        public void Previous()
        {
            Current!.gameObject.SetActive(false);
            Current = GetPrevious();
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
            text.text = page.UserFriendlyName;
        }

        [CanBeNull]
        private MenuPage GetNextPage()
        {
            var currentIndex = Pages.IndexOf(Current);
            var isEnd = currentIndex >= Pages.Count - 1;
            return isEnd ? null : Pages[currentIndex + 1];
        }

        [CanBeNull]
        private MenuPage GetPrevious()
        {
            var currentIndex = Pages.IndexOf(Current);
            var isEnd = currentIndex <= 0;
            return isEnd ? null : Pages[currentIndex - 1];
        }
    
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
