using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Tuna
{
    public class ProductFactory<T> : Factory where T : Collectable
    {
        //Fields
        [SerializeField] private List<StackSlot> arrivalSlots;
        [SerializeField] private T product;
        [SerializeField] private Transform jumpPos;

        //Properties
        private List<Collectable> stocks = new List<Collectable>();
        private StackManager stackManager;

        protected virtual void TakeItem(Collectable newItem, StackSlot slot)
        {
            //Remove item from player stack
            stackManager.RemoveCollectable(newItem);

            //Add item to factory inventory
            stocks.Add(newItem);
            slot.TakeCollectable(newItem);

            //Craft product
            if (stocks.Count >= craftMin)
            {
                CraftProduct();
            }
        }

        protected virtual void CraftProduct()
        {
            var ingredients = stocks.TakeLast(craftMin).ToList();

            foreach (var item in ingredients)
            {
                //Pool-able
                arrivalSlots.FirstOrDefault(x => x.item == item).FreeSlot();
                stocks.Remove(item);
                Destroy(item.gameObject);
            }

            var newProduct = Instantiate(product, jumpPos.position, Quaternion.identity);
            newProduct.transform.DOScale(1, .2f);
        }

        public override void TryTakeItem(StackManager _stackManager, List<Collectable> collectables)
        {
            var availableSlots = arrivalSlots.Where(x => !x.HasItem).ToList();
            var availability = availableSlots.Any() ? availableSlots.Count : 0;
            availability = Mathf.Min(availability, collectables.Count);

            stackManager = _stackManager;

            for (int i = 0; i < availability; i++)
            {
                var newItem = collectables[i];
                var waitSlot = availableSlots[i];
                TakeItem(newItem, waitSlot);
            }
        }
    }
}
