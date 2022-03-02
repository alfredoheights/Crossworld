using UnityEngine;
using UnityEngine.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class BoardView : MonoBehaviour
    {
        public GameObject spaceTemplate;
        // Create 15x15 board for now

        [SerializeField] private int minimumRow = -7;
        [SerializeField] private int minimumColumn = -7;
        [SerializeField] private int maximumRow = 7;
        [SerializeField] private int maximumColumn = 7;

        [SerializeField] private RectTransform contentRef;

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
            int rowCount = maximumRow - minimumRow + 1;
            int columnCount = maximumColumn - minimumColumn + 1;
            _gridLayoutGroup.constraintCount = columnCount;
            for (int row = minimumRow; row <= maximumRow; row++)
            {
                for (int column = minimumColumn; column <= maximumColumn; column++)
                {
                    GameObject newSpace = Instantiate(spaceTemplate, transform);
                }
            }

            float newWidth = columnCount * 32f - Screen.width;
            float newHeight = rowCount * 32f - 32f;

            contentRef.sizeDelta = new Vector2(
                newWidth,
                newHeight
            );
            contentRef.anchoredPosition = new Vector2(
                newWidth / 2,
                newHeight / 2
            );
        }
    }
}