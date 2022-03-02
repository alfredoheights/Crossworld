using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI
{
    /**
     * A base class for any sort of draggable game piece.
     *
     * For use with board game pieces, cards, UI elements, etc.
     *
     * Combine with DroppableSpace for best results.
     */
    public class DraggablePiece /*<T>*/ : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
        IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform fallbackParent;
        [SerializeField] private Transform potentialParent;
        [SerializeField] private Transform movingPiecePlaceholder;

        [FormerlySerializedAs("transitionInterval")]
        public float transitionDuration = .0625f;

        private CanvasGroup _canvasGroup;

        [HideInInspector] public RectTransform rectTransform;
        [HideInInspector] public Canvas canvas;

        [SerializeField] private PiecePickupEventScriptableObject piecePickupEventScriptableObject;

        // Timeout for selection without any movement, to prevent getting stuck
        public float dragTimeoutDuration = .75f;
        private DateTime? _lastDragEvent;

        #region Unity Hooks

        public void Awake()
        {
            // There must be a game object in the canvas that adopts this piece when it's moving.
            movingPiecePlaceholder = GameObject.FindWithTag("MovingPiecePlaceholder").transform;
            rectTransform = (RectTransform) transform;
            canvas = transform.GetComponentInParent<Canvas>();

            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                throw new Exception("Canvas group required for draggable piece");
            }
        }

        public void Update()
        {
            if (_lastDragEvent is null) return;
            TimeSpan timeSinceLastDragEvent = DateTime.Now - (DateTime) _lastDragEvent;
            if (
                (!Input.GetMouseButton(0) && Input.touchCount == 0) &&
                timeSinceLastDragEvent > TimeSpan.FromSeconds(dragTimeoutDuration)
            )
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                OnPointerUp(null);
            }
        }

        #endregion

        #region Draggable Piece Utillities

        private void MoveToParent()
        {
            if (!(potentialParent is null))
            {
                transform.SetParent(potentialParent);
                potentialParent = null;
                fallbackParent = null;
            }
            else if (!(fallbackParent is null))
            {
                transform.SetParent(fallbackParent);
                fallbackParent = null;
            }

            LeanTween.moveLocal(gameObject, Vector3.zero, transitionDuration);
        }

        public void EnterTheFosterSystem(Transform placeholder)
        {
            fallbackParent = transform.parent;
            transform.SetParent(placeholder);
        }

        public void BeAdopted(Transform newParent)
        {
            transform.SetParent(newParent, true);
            MoveToParent();
            fallbackParent = null;
            potentialParent = null;
        }

        public void MeetYourFosterParents(Transform newPotentialParent)
        {
            // Used by the foster parent when dragged over
            this.potentialParent = newPotentialParent;
        }

        public void AccidentallyKillYourFosterParentsDog()
        {
            // Used by the foster parent when dragged out
            potentialParent = null;
        }

        public void CryOnTheOrphanageDoorstep()
        {
            // Beg desperately for someone to take you in by looking at what's below you
            if (potentialParent == null)
            {
                MoveToParent();
                return;
            }

            BeAdopted(potentialParent);
        }

        #endregion


        #region Pointer Interface Methods

        public virtual void OnDrag(PointerEventData eventData)
        {
            _lastDragEvent = System.DateTime.Now;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _lastDragEvent = System.DateTime.Now;
            _canvasGroup.blocksRaycasts = false;
            piecePickupEventScriptableObject.PickUpPiece(this);

            if (Camera.main is null) return;

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, 90);

            LeanTween.move(gameObject, targetPosition, transitionDuration);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            // After ending the drag, you can re-enable raycast
            _canvasGroup.blocksRaycasts = true;
            piecePickupEventScriptableObject.DropPiece();
            MoveToParent();
            _lastDragEvent = null;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _lastDragEvent = System.DateTime.Now;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _lastDragEvent = null;
        }

        #endregion
    }
}