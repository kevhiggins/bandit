using System;
using MapEditor;
using UnityEngine;

namespace Bandit
{
    class Route
    {
        private Merchant merchant;
        private Town destination;
        private Vector3 direction;

        public Route(Merchant merchant, Town destination)
        {
            this.merchant = merchant;
            this.destination = destination;

            var destinationPosition = destination.transform.position;
            var merchantPosition = merchant.transform.position;

            direction = (destinationPosition - merchantPosition).normalized;
        }

        public void Update()
        {
//            merchant.transform.position += direction*merchant.speed;
        }

        public bool HasReachedDestination()
        {
            var spriteRenderer = merchant.GetComponent<SpriteRenderer>();
            return spriteRenderer.bounds.Intersects(destination.GetComponent<SpriteRenderer>().bounds);
        }

        public Merchant GetMerchant()
        {
            return merchant;
        }
    }
}
