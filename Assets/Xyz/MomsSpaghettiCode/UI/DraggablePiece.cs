using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Xyz.MomsSpaghettiCode.UI.Model;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;
using Object = System.Object;

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
        [SerializeField] private DroppableSpace fallbackParent;
        [SerializeField] private Transform movingPiecePlaceholder;
        private Stack<DroppableSpace> _potentialParents;

        [FormerlySerializedAs("transitionInterval")]
        public float transitionDuration = .0625f;

        private CanvasGroup _canvasGroup;

        [HideInInspector] public RectTransform rectTransform;
        [HideInInspector] public Canvas canvas;

        [FormerlySerializedAs("piecePickupEventScriptableObject")] [SerializeField]
        private PieceDragEventScriptableObject pieceDragEventScriptableObject;

        // Timeout for selection without any movement, to prevent getting stuck
        public float dragTimeoutDuration = .75f;
        private DateTime? _lastDragEvent;

        // Use this id to tell the game state about changes within events.
        public int gameStateReferenceId;

        #region Unity Hooks

        public void OnEnable()
        {
            // subscribe to events
            pieceDragEventScriptableObject.parentEnqueueEvent.AddListener(EnqueueParent);
            pieceDragEventScriptableObject.parentDequeueEvent.AddListener(DequeueParent);

            pieceDragEventScriptableObject.pieceMoveEvent.AddListener(MovePiece);
        }

        public void OnDisable()
        {
            // unsubscribe from events
            pieceDragEventScriptableObject.parentEnqueueEvent.RemoveListener(EnqueueParent);
            pieceDragEventScriptableObject.parentDequeueEvent.RemoveListener(DequeueParent);
        }

        public void Awake()
        {
            // There must be a game object in the canvas that adopts this piece when it's moving.
            movingPiecePlaceholder = GameObject.FindWithTag("MovingPiecePlaceholder").transform;
            rectTransform = (RectTransform) transform;
            canvas = transform.GetComponentInParent<Canvas>();

            _potentialParents = new Stack<DroppableSpace>();

            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                throw new Exception("Canvas group required for draggable piece");
            }
        }

        // public void Update()
        // {
        // }

        #endregion

        #region Draggable Piece Utillities

        private void MoveToParent()
        {
            if (_potentialParents.Count > 0)
            {
                transform.SetParent(_potentialParents.Pop().transform);
                _potentialParents.Clear();
                fallbackParent = null;
            }
            else if (!(fallbackParent is null))
            {
                transform.SetParent(fallbackParent.transform);
                fallbackParent = null;
            }

            LeanTween.moveLocal(gameObject, Vector3.zero, transitionDuration);
        }

        public void UsePlaceholderParent(Transform placeholder)
        {
            fallbackParent = transform.parent.GetComponent<DroppableSpace>();
            transform.SetParent(placeholder);
        }

        #region Piece Event listeners

        public void EnqueueParent(DraggablePiece piece, DroppableSpace newPotentialParent)
        {
            if (!ReferenceEquals(piece, this)) return;
            // Used by the foster parent when dragged over
            _potentialParents.Push(newPotentialParent);
        }

        public void DequeueParent(DraggablePiece piece, DroppableSpace parent)
        {
            if (!ReferenceEquals(piece, this) ||
                !ReferenceEquals(_potentialParents.Peek(), parent.GetComponent<DroppableSpace>()))
                return;

            // Used by the foster parent when dragged out
            _potentialParents.Pop();
            if (_potentialParents.Count == 0)
            {
                _potentialParents.Push(fallbackParent);
            }
        }

        public void MovePiece(DraggablePiece piece, DroppableSpace space)
        {
            if (!ReferenceEquals(piece, this)) return;
            MoveToParent();
        }

        #endregion

        public void CryOnTheOrphanageDoorstep()
        {
            // Currently just a public wrapper for MoveToParent, but we will want to have
            // it here so that we can have extra logic there.
            MoveToParent();
        }

        #endregion


        #region Pointer Interface Methods

        public virtual void OnDrag(PointerEventData eventData)
        {
            _lastDragEvent = System.DateTime.Now;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        private IEnumerator RevertIfHoldFailed()
        {
            yield return new WaitForSeconds(dragTimeoutDuration);
            if (
                Input.GetMouseButton(0) ||
                Input.touchCount != 0 ||
                _canvasGroup.blocksRaycasts
            ) yield break;
            OnPointerUp(null);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            StartCoroutine(RevertIfHoldFailed());

            _canvasGroup.blocksRaycasts = false;
            pieceDragEventScriptableObject.PickUpPiece(this);

            for (int i = eventData.hovered.Count - 1; i >= 0; i--)
            {
                GameObject hovered = eventData.hovered[i];
                // How tf is hovered not found here?
                if (!hovered)
                {
                    Debug.Log($"HEY IT SKIPPED ONE at index {i}");
                    continue;
                }
                DroppableSpace droppableSpace = hovered.GetComponent<DroppableSpace>();
                // Hovered doesn't do what I thought it did!
                _potentialParents.Push(droppableSpace);
            }

            if (Camera.main is null) return;

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, 90);

            LeanTween.move(gameObject, targetPosition, transitionDuration);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            // After ending the drag, you can re-enable raycast
            _canvasGroup.blocksRaycasts = true;
            pieceDragEventScriptableObject.DropPiece();
            if (_potentialParents.Count > 0)
            {
                DroppableSpace targetParent = _potentialParents.Peek();
                if (targetParent is { } && targetParent.IsValidTarget(this))
                {
                    pieceDragEventScriptableObject.pieceRemovedEvent.Invoke(this, fallbackParent);
                    pieceDragEventScriptableObject.pieceMoveEvent.Invoke(this, targetParent);
                }
                else
                {
                    _potentialParents.Clear();
                    _potentialParents.Push(fallbackParent);
                }
            }

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