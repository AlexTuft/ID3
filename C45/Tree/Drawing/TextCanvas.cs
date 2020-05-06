using System;
using System.Linq;
using System.Text;

namespace C45.Tree
{
    public class TextCanvas
    {
        private char[,] _canvas = new char[1, 1];
        private int _maxY = 0;

        public void Draw(string text, int x, int y)
        {
            var lines = text.Split('\n');
            
            GuardCoordinates(x, y);
            
            var boundingBoxWidth = lines.Max(x => x.Length);
            var boundingBoxHeight = lines.Length;
            CheckBounds(x + boundingBoxWidth, y + boundingBoxHeight);
            
            DrawToCanvas(lines, x, y);
        }

        private void DrawToCanvas(string[] textLines, int x, int y)
        {
            for (int yy = 0; yy < textLines.Length; yy++)
            {
                for (int xx = 0; xx < textLines[yy].Length; xx++)
                {
                    _canvas[y + yy, x + xx] = textLines[yy][xx];
                }
            }
            _maxY = Math.Max(_maxY, y + textLines.Length);
        }

        private void CheckBounds(int x, int y)
        {
            if (y >= _canvas.GetLength(dimension: 0) || x >= _canvas.GetLength(dimension: 1))
            {
                int newHeight = ResizeHeightEnoughFor(y);
                int newWidth = ResizeWidthEnoughFor(x);
                CopyCanvas(newHeight, newWidth);
            }
        }

        private int ResizeWidthEnoughFor(int x)
        {
            var newWidth = _canvas.GetLength(1);
            while (newWidth <= x)
            {
                newWidth *= 2;
            }

            return newWidth;
        }

        private int ResizeHeightEnoughFor(int y)
        {
            var newHeight = _canvas.GetLength(0);
            while (newHeight <= y)
            {
                newHeight *= 2;
            }

            return newHeight;
        }

        private void CopyCanvas(int newHeight, int newWidth)
        {
            var newCanvas = new char[newHeight, newWidth];

            for (int y = 0; y < Math.Min(_maxY, _canvas.GetLength(0)); y++) {
                for (int x = 0; x < _canvas.GetLength(1); x++) {
                    newCanvas[y, x] = _canvas[y, x];
                }
            }

            _canvas = newCanvas;
        }

        private void GuardCoordinates(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException("Coordinates must be positive");
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int y = 0; y < _canvas.GetLength(0); y++) {
                for (int x = 0; x < _canvas.GetLength(1); x++) {
                    builder.Append(_canvas[y, x]);
                }
                builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
