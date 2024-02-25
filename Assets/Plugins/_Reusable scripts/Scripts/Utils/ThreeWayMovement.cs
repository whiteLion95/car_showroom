using DG.Tweening;
using System;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Class with methods useful for runner with three-way movement
    /// </summary>
    [Serializable]
    public class ThreeWayMovement
    {
        [SerializeField] protected ThreeWayMovementData _data;
        [SerializeField] protected Transform _movingTransform;
        protected bool moveRight;
        protected int currentLine;
        protected bool stopped;
        protected Coordinates type;

        /// <summary>
        /// Move transform forward with certain speed. Call it in Update method
        /// </summary>
        /// <param name="speed">Move speed</param>
        public void MoveForward()
        {
            if (!stopped)
            {
                _movingTransform.Translate(Vector3.forward * _data.MovementSpeed * Time.deltaTime);
            }
        }

        public void StopMovingForward(bool value)
        {
            stopped = value;
        }

        /// <summary>
        /// Determine line to move to, direction of swipe and turns IsChangingLine to true. Call it in your SwipeHandler method
        /// </summary>
        /// <param name="deltaX">Positive if you move right and vice versa</param>
        public virtual void SwipeHandler(float deltaX)
        {
            if (!IsChangingLine)
            {
                if (deltaX > 0 && currentLine <= 0)
                {
                    currentLine++;
                    moveRight = true;
                    IsChangingLine = true;
                }

                if (deltaX < 0 && currentLine >= 0)
                {
                    currentLine--;
                    moveRight = false;
                    IsChangingLine = true;
                }
            }
        }

        /// <summary>
        /// Changes the transform's line. Call it in the Update method if IsChangingLine is true
        /// </summary>
        /// <param name="transform">Moving transform</param>
        /// <param name="speed">Changing line speed</param>
        public void ChangeLine()
        {
            CheckIfOnTheLine(_movingTransform, _data.DistanceBetweenLines);

            if (moveRight)
            {
                _movingTransform.Translate(Vector3.right * _data.ChangeLineSpeed * Time.deltaTime);
            }
            else
            {
                _movingTransform.Translate(Vector3.left * _data.ChangeLineSpeed * Time.deltaTime);
            }
        }

        public virtual void ChangeLine(Line line)
        {
            if (type == Coordinates.Global)
            {
                switch (line)
                {
                    case Line.Left:
                        if (_movingTransform.position.x == -_data.DistanceBetweenLines)
                        {
                            return;
                        }
                        break;
                    case Line.Middle:
                        if (_movingTransform.position.x == 0)
                        {
                            return;
                        }
                        break;
                    case Line.Right:
                        if (_movingTransform.position.x == _data.DistanceBetweenLines)
                        {
                            return;
                        }
                        break;
                }
            }

            currentLine = (int)line;
            float xPos = (type == Coordinates.Global) ? _movingTransform.position.x : _movingTransform.localPosition.x;

            switch (line)
            {
                case Line.Left:
                    moveRight = (xPos < -_data.DistanceBetweenLines) ? true : false;
                    break;
                case Line.Middle:
                    moveRight = (xPos < 0) ? true : false;
                    break;
                case Line.Right:
                    moveRight = (xPos > _data.DistanceBetweenLines) ? false : true;
                    break;
            }

            IsChangingLine = true;
        }

        /// <summary>
        /// Checks if transform is on the line and if so stops it from changing the line
        /// </summary>
        protected virtual void CheckIfOnTheLine(Transform transform, float distanceBetweenLines)
        {
            float xPos = (type == Coordinates.Global) ? transform.position.x : transform.localPosition.x;

            switch (currentLine)
            {
                case 0:
                    if (moveRight && xPos >= 0)
                    {
                        StopChangingLine(0);
                    }
                    else if (!moveRight && xPos <= 0)
                    {
                        StopChangingLine(0);
                    }
                    break;
                case 1:
                    if (xPos >= distanceBetweenLines)
                    {
                        StopChangingLine(distanceBetweenLines);
                    }
                    break;
                case -1:
                    if (xPos <= -distanceBetweenLines)
                    {
                        StopChangingLine(-distanceBetweenLines);
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets transform's X value to lineXValue and IsChangingLine to false
        /// </summary>
        protected virtual void StopChangingLine(float lineXValue)
        {
            if (type == Coordinates.Global)
            {
                float stopDuration = Mathf.Abs(_movingTransform.position.x - lineXValue) / _data.StopChangingLineSpeed;
                _movingTransform.DOMoveX(lineXValue, stopDuration);
            }
            else
            {
                float stopDuration = Mathf.Abs(_movingTransform.localPosition.x - lineXValue) / _data.StopChangingLineSpeed;
                _movingTransform.DOLocalMoveX(lineXValue, stopDuration);
            }

            IsChangingLine = false;
        }

        public void ChangeCoordinateSystem(Coordinates type)
        {
            this.type = type;
        }

        public bool IsChangingLine { get; private set; }
        public Line CurrentLine { get { return (Line)currentLine; } }
        public float DistanceBetweenLines { get { return _data.DistanceBetweenLines; } }
    }
}

public enum Line
{
    Left = -1,
    Middle,
    Right
}

public enum Coordinates
{
    Global,
    Local
}

