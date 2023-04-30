using DG.Tweening;
using UnityEngine;

namespace Tuna
{
    public class StackSlot : MonoBehaviour
    {
        public bool HasItem;
        
        public Collectable item => _item ;
        private Collectable _item;
        
        public void FreeSlot()
        {
            _item = null;
            HasItem = false;
        }

        public void TakeCollectable(Collectable _item)
        {
            HasItem = true;
            this._item = _item;
            this._item.transform.SetParent(transform);
            this._item.transform.DOLocalMove(Vector3.zero, .25f).OnComplete(()=>
            {
                this._item.transform.localPosition = Vector3.zero;
                this.item.transform.localEulerAngles = Vector3.zero;
            });
        }
    }
}