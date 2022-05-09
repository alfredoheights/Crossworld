using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class BoardView : MonoBehaviour
    {
        public BoardSpaceView spaceTemplate;
        // Create 15x15 board for now

        [SerializeField] private int minimumRow = 0;
        [SerializeField] private int minimumColumn = 0;
        [SerializeField] private int maximumRow = 0;
        [SerializeField] private int maximumColumn = 0;

        [SerializeField] private RectTransform contentRef;
        [SerializeField] private RectTransform gameContainerRef;

        private GridLayoutGroup _gridLayoutGroup;
        private RectTransform _rectTransform;

        private void Awake()
        {
            // Get reference to the grid so you can change the values there
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            _rectTransform = (RectTransform) transform;
            GeneratePieces();
        }

        private void GeneratePieces()
        {
            for (int row = minimumRow; row <= maximumRow; row++)
            {
                for (int column = minimumColumn; column <= maximumColumn; column++)
                {
                    AddNewSpace(column, row);
                }
            }

            UpdateSize();
        }

        private void UpdateSize()
        {
            int rowCount = maximumRow - minimumRow + 1;
            int columnCount = maximumColumn - minimumColumn + 1;
            float newWidth = columnCount * 32f - Screen.width;
            newWidth = Math.Max(newWidth, gameContainerRef.rect.size.x);
            float newHeight = rowCount * 32f - 32f;
            newHeight = Math.Max(newHeight, gameContainerRef.rect.size.y);

            _gridLayoutGroup.constraintCount = columnCount;
            
            contentRef.sizeDelta = new Vector2(
            newWidth,
            newHeight
            );
            contentRef.anchoredPosition = new Vector2(
                newWidth / 2,
                newHeight / 2
            );
        }
        
        public void AddRow(int direction)
        {
            int newRowIndex;
            if (direction == 1)
            {
                newRowIndex = maximumRow + direction;
                maximumRow = newRowIndex;
            }
            else if (direction == -1)
            {
                newRowIndex = minimumRow + direction;
                minimumRow = newRowIndex;
            }
            else
            {
                return;
            }

            for (int i = minimumColumn; i <= maximumColumn; i++)
            {
                AddNewSpace(i, newRowIndex);
            }
        }

        public void AddColumn(int direction)
        {
            int newColumnIndex;
            if (direction == 1)
            {
                newColumnIndex = maximumColumn + direction;
                maximumColumn = newColumnIndex;
            }
            else if (direction == -1)
            {
                newColumnIndex = minimumColumn + direction;
                minimumColumn = newColumnIndex;
            }
            else
            {
                return;
            }

            for (int i = minimumRow; i <= maximumRow; i++)
            {
                AddNewSpace(newColumnIndex, i);
            }
        }

        private void AddNewSpace(int x, int y)
        {
            GameObject newSpace = Instantiate(spaceTemplate.gameObject, transform);

            // Set the sibling index so it goes to the right spot when added
            int width = maximumColumn - minimumColumn + 1;
            int heightOffset = width * (y - minimumColumn);
            newSpace.transform.SetSiblingIndex(heightOffset + x - minimumColumn);

            BoardSpaceView spaceView = newSpace.GetComponent<BoardSpaceView>();
            spaceView.X = x;
            spaceView.Y = y;
        }
    }
}