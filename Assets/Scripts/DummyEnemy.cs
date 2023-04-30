using UnityEngine;

namespace Tuna
{
    public class DummyEnemy : MonoBehaviour
    {

        //Fields
        [SerializeField] private ParticleSystem hitParticle;
        
        //Properties
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health <= 0)
                {
                    EventManager.EnemyDeath?.Invoke();
                    Destroy(gameObject);
                }
            }
        }
        private int _health = 2;
        
        //Unity Methods
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Sword"))
            {
                var hitDir = transform.position - collision.contacts[0].point;
                hitDir = hitDir.normalized;
                TakeHit(hitDir);
            }
        }
        
        //Private Methods
        private void TakeHit(Vector3 _dir)
        {
            print("Took a hit!");
            Health--;
            var hitFeedback = Instantiate(hitParticle, transform.position + _dir * .3f, Quaternion.identity);
            
            CameraShake.Instance.ShakeCamera(1,.5f);
            AudioManager.Instance.PlaySound(AudioManager.Instance.aaaAAAA);
            Destroy(hitFeedback, 2f);
        }
    }
}
