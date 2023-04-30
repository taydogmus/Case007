using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Tuna
{
    public class OreFactory : Factory
    {
        //Fields
        [SerializeField] private List<StackSlot> arrivalSlots;
        [SerializeField] private Metal metal;
        [SerializeField] private Transform JumpPos;

        //Properties
        private List<Ore> Stocks = new List<Ore>();
        private StackManager stackManager;
        
        private void TakeOre(Ore newOre, StackSlot slot)
        {
            //RemoveOre From PlayerStack
            stackManager.RemoveCollectable(newOre as Collectable);
            
            //Add Ore To Factory Inventory
            Stocks.Add(newOre);
            slot.TakeCollectable(newOre);
            
            //Craft Product
            if (Stocks.Count >= craftMin)
            {
                CraftProduct();
            }
        }

        private void CraftProduct()
        {
            var ingredients = Stocks.TakeLast(craftMin).ToList();

            foreach (var oreInstance in ingredients)
            {
                //Pool-able
                arrivalSlots.FirstOrDefault(x => x.item == oreInstance).FreeSlot();
                Stocks.Remove(oreInstance);
                Destroy(oreInstance.gameObject);
            }

            var product = Instantiate(metal, JumpPos.position, Quaternion.identity);
            product.transform.DOScale(1, .2f);

        }
        
        public override void TryTakeItem(StackManager _stackManager,List<Collectable> collectables)
        {
            var availableSlots = arrivalSlots.Where(x => !x.HasItem).ToList();
            var availability = availableSlots.Any() ? availableSlots.Count : 0;
            availability = Mathf.Min(availability, collectables.Count);

            stackManager = _stackManager;
            
            for (int i = 0; i < availability; i++)
            {
                Ore oreInstance = collectables[i] as Ore;
                var waitSlot = availableSlots[i];
                TakeOre(oreInstance, waitSlot);
                //print(i);
            }
        }
    }
}