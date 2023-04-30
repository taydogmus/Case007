using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tuna
{
    public class GuiManager : MonoBehaviour
    {
        public static GuiManager Instance;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject tutorialPanel;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            EventManager.GameOver += ShowGameOverPanel;
        }

        private void ShowGameOverPanel()
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.victory);
            gameOverUI.SetActive(true);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(0);
        }

        public void ShowTutorial()
        {
            tutorialPanel.SetActive(true);
            tutorialPanel.transform.DOScale(1, .3f);
        }

        public void HideTutorial()
        {
            tutorialPanel.SetActive(false);
            TutorialManager.IsTutorialPlayed = true;
            PlayerPrefs.Save();
        }
    }
}
