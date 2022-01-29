using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FlexibleGridLayout : LayoutGroup  
    {
        public enum FitType {
            Uniform,
            Width,
            Height
        }

        public FitType fitType;

        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;


        public override void CalculateLayoutInputHorizontal() {

            base.CalculateLayoutInputHorizontal();

            var sqrRt = Mathf.Sqrt(rectChildren.Count);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);

            rows = fitType switch
            {
                FitType.Width => Mathf.CeilToInt(rectChildren.Count / (float) columns),
                FitType.Height => Mathf.CeilToInt(rectChildren.Count / (float) rows),
                _ => rows
            };

            var rectTransformRect = rectTransform.rect;
            var parentWidth = rectTransformRect.width;
            var parentHeight = rectTransformRect.height;

            var cellWidth = parentWidth / columns - spacing.x / columns * (columns - 1) - padding.left / (float)columns - padding.right / (float)columns;
            var cellHeight = parentHeight / rows - spacing.y / rows * (rows - 1) - padding.top / (float)rows - padding.bottom / (float)rows;

            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            for (var i = 0; i < rectChildren.Count; i++) {
            
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
                var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }

        }

        public override void CalculateLayoutInputVertical() { }

        public override void SetLayoutHorizontal() { }

        public override void SetLayoutVertical() { }
    }
}

