using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuna
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private BloodVFX particleHit;

        private ParticleSystem part;
        private List<ParticleCollisionEvent> collisionEvents;

        void Start()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }


        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            int i = 0;

            while (i < numCollisionEvents)
            {
                var targetPos = collisionEvents[i].intersection;
                targetPos.y = 0.025f;
                var bloodVFX = Instantiate(particleHit, targetPos, Quaternion.identity);
                i++;
            }
        }
    }
}
