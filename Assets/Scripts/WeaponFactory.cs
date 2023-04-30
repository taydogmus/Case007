using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Tuna
{
    public class WeaponFactory : Factory
    {
        [SerializeField] private List<StackSlot> arrivalSlots;
        [SerializeField] private Weapon weapon;
        [SerializeField] private Transform JumpPos;
        
        private List<Metal> Stocks = new List<Metal>();
        private StackManager stackManager;
        
        private void TakeMetal(Metal newMetal, StackSlot slot)
        {
            //RemoveOre From PlayerStack
            stackManager.RemoveCollectable(newMetal);
            
            //Add Ore To Factory Inventory
            Stocks.Add(newMetal);
            slot.TakeCollectable(newMetal);
            
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

            var product = Instantiate(weapon, JumpPos.position, Quaternion.identity);
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
                Metal oreInstance = collectables[i] as Metal;
                var waitSlot = availableSlots[i];
                TakeMetal(oreInstance, waitSlot);
                print(i);
            }
        }
    }
}