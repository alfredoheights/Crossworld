using System;
using UnityEngine;
using Xyz.MomsSpaghettiCode.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class HandSpaceView : SpaceView
    {
        public float handSpaceFadeDuration = .0625f;
        
        private void OnEnable()
        {
            SubscribeToEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        private void Start()
        {
            transform.localScale = new Vector3(0, 1, 1);
            LeanTween.scaleX(gameObject, 1, handSpaceFadeDuration);
        }

        private void Remove()
        {
            LeanTween.scaleX(gameObject, 0, handSpaceFadeDuration).setDestroyOnComplete(true);
        }

        protected override void PieceRemoved(DraggablePiece piece)
        {
            base.PieceRemoved(piece);
            Remove();
        }
    }
}