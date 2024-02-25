using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.CurrencySystem
{
    public class Stacker
    {
        private readonly Vector3 _basePosition;

        private readonly Vector3 _rawOffset;
        private readonly Vector3 _columnOffset;
        private readonly Vector3 _levelOffset;

        private readonly int _numberOfItemsInColumn;
        private readonly int _numberOfColumnsInLayer;

        public Stacker(Vector3 rawOffset, Vector3 columnOffset, int numberOfItemsInColumn,
            Vector3 basePosition = new Vector3(), Vector3 levelOffset = new Vector3(), int numberOfColumns = 0)
        {
            _basePosition = basePosition;

            _rawOffset = rawOffset;
            _columnOffset = columnOffset;
            _levelOffset = levelOffset;

            _numberOfItemsInColumn = numberOfItemsInColumn;
            _numberOfColumnsInLayer = numberOfColumns;
        }

        public Vector3 GetNextItemPosition(int itemsCount)
        {
            var numberOfLevels = itemsCount / _numberOfItemsInColumn / _numberOfColumnsInLayer;
            var numberOfColumns = itemsCount % _numberOfItemsInColumn;
            var numberOfRaws = (itemsCount - _numberOfItemsInColumn * _numberOfColumnsInLayer * numberOfLevels) / _numberOfItemsInColumn;

            var rawOffset = _rawOffset * numberOfRaws;
            var columnOffset = _columnOffset * numberOfColumns;
            var levelOffset = _levelOffset * numberOfLevels;

            return _basePosition + rawOffset + columnOffset + levelOffset;
        }
    }
}
