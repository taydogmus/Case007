using UnityEngine;

namespace Tuna
{
    public class TutorialManager : MonoBehaviour
    {
        private const string IsTutorialPlayedKey = "IsTutorialPlayedKey";
        
        public static bool IsTutorialPlayed
        {
            get
            {
                return PlayerPrefs.GetInt(IsTutorialPlayedKey) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(IsTutorialPlayedKey, value ? 1 : 0);
            }
        }

        private void Start()
        {
            if (!IsTutorialPlayed)
            {
                Invoke(nameof(ShowTutorial), 4f);
            }
        }

        private void ShowTutorial() => GuiManager.Instance.ShowTutorial();
    }
}