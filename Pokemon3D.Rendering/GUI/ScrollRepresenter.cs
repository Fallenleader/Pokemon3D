using System;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.GUI
{
    internal class ScrollRepresenter
    {
        private int _maxScrollY;
        private float _scrollCalculatedPerDisplayed;

        public ScrollRepresenter()
        {
            _maxScrollY = 0;
            _scrollCalculatedPerDisplayed = 0.0f;
        }

        public int ScrollValueDisplayed { get; private set; }
        public int ScrollValueCalculated { get; private set; }
        public int ThumbSize { get; private set; }
        public bool CanContentScroll { get { return _maxScrollY > 0; } }

        public void SetRange(int scrollableArea, int contentSize, int minThumbSize)
        {
            _maxScrollY = scrollableArea - minThumbSize;
            var invisibleSize = contentSize - scrollableArea;

            if (invisibleSize < _maxScrollY)
            {
                _maxScrollY = invisibleSize;
                ThumbSize = scrollableArea - _maxScrollY;
            }

            _scrollCalculatedPerDisplayed = invisibleSize / (float)_maxScrollY;
            
        }

        public void Scroll(int value)
        {
            ScrollValueDisplayed = MathHelper.Clamp(ScrollValueDisplayed + value, 0, _maxScrollY);
            ScrollValueCalculated = (int)Math.Round(ScrollValueDisplayed * _scrollCalculatedPerDisplayed);
        }
    }
}