using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameViews.ScriptableObjects;
using Xyz.MomsSpaghettiCode.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class PieceView : DraggablePiece, IPointerDownHandler, IPointerUpHandler,
        IEndDragHandler
    {
        public Image top;
        public Image side;
        public Image shadow;
        public TextMeshProUGUI letter;

        public PieceViewScriptableObject pieceViewScriptableObject;

        private bool _isScaledUp = false;

        private CanvasGroup _imagesCanvasGroup;

        #region Unity Hooks

        private new void Awake()
        {
            if (pieceViewScriptableObject == null)
            {
                throw new Exception("scriptable object not set for piece view");
            }

            base.Awake();
            _imagesCanvasGroup = top.gameObject.GetComponentInParent<CanvasGroup>();
        }


        #endregion

        #region Transform Functions

        private void ScaleUp()
        {
            if (_isScaledUp) return;
            // Move and scale only the side, the shadow should stay where it is.

            LeanTween.scale(top.rectTransform, pieceViewScriptableObject.dragScale, transitionDuration);
            LeanTween.scale(side.rectTransform, pieceViewScriptableObject.dragScale, transitionDuration);

            LeanTween.moveLocal(
                top.gameObject,
                Vector3.up * 3 * pieceViewScriptableObject.dragScale.y + pieceViewScriptableObject.dragOffset,
                transitionDuration
            );
            LeanTween.moveLocal(
                side.gameObject,
                Vector3.zero + pieceViewScriptableObject.dragOffset,
                transitionDuration
            );

            LeanTween.alphaCanvas(_imagesCanvasGroup, pieceViewScriptableObject.dragOpacity, transitionDuration);
            _isScaledUp = true;
        }

        private void ScaleDown()
        {
            if (!_isScaledUp) return;
            LeanTween.scale(top.rectTransform, Vector3.one, transitionDuration);
            LeanTween.scale(side.rectTransform, Vector3.one, transitionDuration);

            LeanTween.moveLocal(
                top.gameObject,
                Vector3.up * 3,
                transitionDuration
            );
            LeanTween.moveLocal(
                side.gameObject,
                Vector3.zero,
                transitionDuration
            );

            LeanTween.alphaCanvas(_imagesCanvasGroup, 1f, transitionDuration);
            _isScaledUp = false;
        }

        #endregion

        #region Drag Events

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            ScaleUp();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            ScaleDown();
        }

        /*
.
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
        }

        */

        #endregion

        #region Hover Events

        #endregion
    }
}