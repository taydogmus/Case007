using UnityEngine;

namespace Tuna
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioSource effectSource;
        public AudioClip yesSir;
        public AudioClip aaaAAAA;
        public AudioClip victory;
        public AudioClip pieceOfCake;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void PlaySound(AudioClip clip)
        {
            effectSource.PlayOneShot(clip);
        }
    }
}