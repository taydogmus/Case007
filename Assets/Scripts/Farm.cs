using DG.Tweening;
using UnityEngine;

namespace Tuna
{
    public class Farm : MonoBehaviour
    {
        [SerializeField] private Collectable product;
        [SerializeField] private Transform outputTransform;
        [SerializeField] private Transform model;

        private bool isEmpty = true;
        private int maxStock = 7;
        private int currentStock;
        private int stockCount
        {
            get => currentStock;
            set
            {
                currentStock = value;
                if (currentStock >= maxStock)
                {
                    currentStock = maxStock;
                }
                OnStockChanged();
            }
        }

        private void Awake()
        {
            stockCount = 0;
        }

        private void OnStockChanged()
        {
            if (currentStock == 0)
            {
                isEmpty = true;
                FillStocks();
            }
        }

        private void FillStocks()
        {
            model.transform.DOScale(1f, 3.5f)
                .OnComplete(() =>
                {
                    stockCount = maxStock;
                    isEmpty = false;
                });
        }

        private void GiveAProduct()
        {
            if (stockCount <= 0) return;
            
            stockCount--;
            var scale = Vector3.one;
            var height = (float)currentStock / (float)maxStock;
            scale.y = 0.1f + height;
            model.transform.localScale = scale;
            var productClone = Instantiate(product, transform.position, Quaternion.identity);
            productClone.transform.DOJump(outputTransform.position, 1f, 1, 1f).OnComplete(() =>
            {
                productClone.isReadyToCollect = true;
            });
        }

        public void TryTakeProduct() => GiveAProduct();
    }
}