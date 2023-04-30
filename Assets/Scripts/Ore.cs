using UnityEngine;

namespace Tuna
{
    public class Ore : Collectable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent<StackManager>(out StackManager stackManager))
            {
                if (stackManager.TryCollectItem(InstanceType, this as Collectable))
                {
                    _collider.isTrigger = true;
                    _rigidbody.isKinematic = true;    
                }
            }
        }
    }
}