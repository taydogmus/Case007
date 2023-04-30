using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tuna
{
    [RequireComponent(typeof(PlayerController))]
    public class StackManager : MonoBehaviour
    {
        public List<StackSlot> stackSlots;
        public List<Collectable> inventory;

        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }
        
        private void AppendCollectable(Collectable c)
        {
            inventory.Add(c);
            _playerController.IsHandsEmpty(true);
        }

        public void RemoveCollectable(Collectable c)
        {
            var slot = stackSlots.Where(x => x.item == c).ToList();
            slot.FirstOrDefault().FreeSlot();
            inventory.Remove(c);

            var isHandEmpty = inventory.Count > 0;
            _playerController.IsHandsEmpty(isHandEmpty);
        }

        public bool TryCollectItem(CollectableType InstanceType, Collectable ore)
        {
            var availableSlots = stackSlots.Where(x => !x.HasItem).ToList();
            
            if (availableSlots.Any())
            {
                availableSlots.FirstOrDefault().TakeCollectable(ore);
                AppendCollectable(ore);
                return true;
            }
            return false;
        }

        public bool HasType(CollectableType type, out List<Collectable> collectables)
        {
            var _hasType = inventory.Any(x => x.InstanceType == type);
            
            if (_hasType)
            {
                collectables = inventory.Where(x=> x.InstanceType == type).ToList();
                return _hasType;
            }

            collectables = new List<Collectable>();
            return _hasType;
        }

        public void ReOrderInventory()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                stackSlots[i].TakeCollectable(inventory[i]);
            }
        }
    }
}