using System.Collections.Generic;
using UnityEngine;

namespace Tuna
{
    public class Factory : MonoBehaviour
    {
        public CollectableType factoryType;
        public int craftMin = 5;
        public virtual void TryTakeItem(StackManager stackManager,List<Collectable> collectables)
        {
        }
    }
}
