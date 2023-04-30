using UnityEngine;

namespace Tuna
{
    public class Weapon : Collectable
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent<PlayerController>(out PlayerController player))
            {
                if (player)
                {
                    player.AcquireWeapon();
                    AudioManager.Instance.PlaySound(AudioManager.Instance.yesSir);
                    Destroy(gameObject);
                }
            }
        }
    }
}