using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameViews.ScriptableObjects;
using Xyz.MomsSpaghettiCode.UI;
using Xyz.MomsSpaghettiCode.UI.Model;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class PieceView : DraggablePiece, IPointerDownHandler, IPointerUpHandler,
        IEndDragHandler
    {
        public Image top;
        public Image side;
        public Image shadow;
        public TextMeshProUGUI letter;
        public TextMeshProUGUI points;

        public GamePiece gamePiece;

        [FormerlySerializedAs("pieceViewScriptableObject")]
        public PieceViewSettingsScriptableObject pieceViewSettingsScriptableObject;

        public PlayerStateScriptableObject playerStateScriptableObject;

        private bool _isScaledUp = false;

        private CanvasGroup _imagesCanvasGroup;

        #region Unity Hooks

        private new void Awake()
        {
            if (pieceViewSettingsScriptableObject == null)
            {
                throw new Exception("scriptable object not set for piece view");
            }
            
            playerStateScriptableObject.pieceMovedEvent.AddListener(PieceMovedEventResponse);

            base.Awake();
            _imagesCanvasGroup = top.gameObject.GetComponentInParent<CanvasGroup>();
        }

        #endregion

        #region Transform Functions

        private void ScaleUp()
        {
            if (_isScaledUp) return;
            // Move and scale only the side, the shadow should stay where it is.

            LeanTween.scale(top.rectTransform, pieceViewSettingsScriptableObject.dragScale, transitionDuration);
            LeanTween.scale(side.rectTransform, pieceViewSettingsScriptableObject.dragScale, transitionDuration);

            LeanTween.moveLocal(
                top.gameObject,
                Vector3.up * 3 * pieceViewSettingsScriptableObject.dragScale.y +
                pieceViewSettingsScriptableObject.dragOffset,
                transitionDuration
            );
            LeanTween.moveLocal(
                side.gameObject,
                Vector3.zero + pieceViewSettingsScriptableObject.dragOffset,
                transitionDuration
            );

            LeanTween.alphaCanvas(_imagesCanvasGroup, pieceViewSettingsScriptableObject.dragOpacity,
                transitionDuration);
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

        #region Game Piece Interactions

        public void SetPiece(GamePiece newPiece)
        {
            ChangeLetter(newPiece.Letter.ToString());
            ChangePointValue(newPiece.Points);
            SetPieceId(newPiece.Id);
            gamePiece = newPiece;
        }
        private void ChangeLetter(string newLetter)
        {
            letter.text = newLetter;
        }

        private void ChangePointValue(int newPointValue)
        {
            if (!Constants.numberDots.ContainsKey(newPointValue)) return;
            points.text = Constants.numberDots[newPointValue];
        }

        private void SetPieceId(int newPieceId)
        {
            gameStateReferenceId = newPieceId;
        }

        #endregion
        
        #region Game State Events

        public void PieceMovedEventResponse(GamePiece piece)
        {
            if (piece.Id != gameStateReferenceId) return;
            Debug.Log($"This piece moved - {piece.Id}, {piece.Letter}");
        }
        
        #endregion
    }
}