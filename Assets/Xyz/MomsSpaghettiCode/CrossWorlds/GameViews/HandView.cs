using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects;
using Xyz.MomsSpaghettiCode.UI;
using Xyz.MomsSpaghettiCode.UI.Model;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class HandView : MonoBehaviour
    {
        public HandSide leftSide;
        public HandSide rightSide;

        public Transform handSpaceContainer;
        public GameObject handSpacePrefab;
        public GameObject pieceViewPrefab;

        public MatchStateScriptableObject matchState;
        public PlayerStateScriptableObject playerState;

        private void Start()
        {
            leftSide.pieceDroppedOnHandSideEvent.AddListener(AddPieceToLeftOfHand);
            rightSide.pieceDroppedOnHandSideEvent.AddListener(AddPieceToRightOfHand);
            playerState.newHitEvent.AddListener(NewHit);
        }

        private void OnDestroy()
        {
            playerState.newHitEvent.RemoveListener(NewHit);
        }

        private void NewHit(List<GamePiece> pieces)
        {
            foreach (GamePiece gamePiece in pieces)
            {
                GameObject handSpace = Instantiate(handSpacePrefab, handSpaceContainer);
                GameObject pieceGO = Instantiate(pieceViewPrefab, handSpace.transform);
                PieceView pieceView = pieceGO.GetComponent<PieceView>();
                pieceView.SetPieceId(gamePiece.Id);
                pieceView.ChangeLetter(gamePiece.Letter.ToString());
                pieceView.ChangePointValue(gamePiece.Points);
            }
        }

        private void AddPieceToHand(DraggablePiece piece, int index)
        {
            GameObject newSpace = Instantiate(handSpacePrefab, handSpaceContainer, true);
            newSpace.transform.localScale = Vector3.one;
            newSpace.transform.SetSiblingIndex(index);
        }

        private void AddPieceToLeftOfHand(DraggablePiece piece)
        {
            AddPieceToHand(piece, 0);
        }

        private void AddPieceToRightOfHand(DraggablePiece piece)
        {
            AddPieceToHand(piece, handSpaceContainer.childCount);
        }
    }
}