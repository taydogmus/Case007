using System;
using UnityEngine;

namespace Tuna
{
    public class BloodVFX : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float health = 200f;
        
        private bool isInteracted = false;
        private Texture2D maskTexture;
        private Texture2D maskTextureCopy;

        private void Start()
        {
            EventManager.BloodVfxBirth?.Invoke();
            
            Texture2D texture = Instantiate(_renderer.material.GetTexture("mainTexture")) as Texture2D;
            _renderer.material.SetTexture("mainTexture", texture);
            
        }

        public void TakeHit() => TakeDamage();

        private void TakeDamage()
        {
            health -= Time.deltaTime;
            if (0 >= health)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.pieceOfCake);
                EventManager.BloodVfxDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}