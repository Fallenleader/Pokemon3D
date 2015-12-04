using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.GUI
{
    public class NinePatchSprite : Sprite
    {
        private readonly List<Tuple<Rectangle, Rectangle>> _patchFields = new List<Tuple<Rectangle, Rectangle>>();
        private readonly Rectangle _sourceTextureRectangle;
        private readonly int _leftBorder;
        private readonly int _rightBorder;
        private readonly int _topBorder;
        private readonly int _bottomBorder;
        private readonly int _commulativeFixedWidth;
        private readonly int _commulativeFixedHeight;
        private Rectangle _bounds;

        public NinePatchSprite(Texture2D texture, int unitBorder)
            : this(texture, null, unitBorder, unitBorder, unitBorder, unitBorder)
        { }

        public NinePatchSprite(Texture2D texture, int horizontalBorder, int verticalBorder)
            : this(texture, null, horizontalBorder, verticalBorder, horizontalBorder, verticalBorder)
        { }

        public NinePatchSprite(Texture2D texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
            : this(texture, null, leftBorder, rightBorder, topBorder, bottomBorder)
        { }

        public NinePatchSprite(Texture2D texture, Rectangle? sourceRectangle, int unitBorder)
            : this(texture, sourceRectangle, unitBorder, unitBorder, unitBorder, unitBorder)
        { }

        public NinePatchSprite(Texture2D texture, Rectangle? sourceRectangle, int horizontalBorder, int verticalBorder)
            : this(texture, sourceRectangle, horizontalBorder, verticalBorder, horizontalBorder, verticalBorder)
        { }

        public NinePatchSprite(Texture2D texture, Rectangle? sourceRectangle, Thickness border) :
            this(texture, sourceRectangle, border.Left, border.Right, border.Top, border.Bottom)
        { }

        public NinePatchSprite(Texture2D texture, Rectangle? sourceRectangle, int leftBorder, int rightBorder, int topBorder, int bottomBorder):
            base(texture)
        {
            _sourceTextureRectangle = sourceRectangle ?? new Rectangle(0, 0, texture.Width, texture.Height);
            _leftBorder = leftBorder;
            _rightBorder = rightBorder;
            _topBorder = topBorder;
            _bottomBorder = bottomBorder;
            _commulativeFixedWidth = _leftBorder + _rightBorder;
            _commulativeFixedHeight = _topBorder + _bottomBorder;
            MinSize = new Rectangle(0, 0, _commulativeFixedWidth, _commulativeFixedHeight);
            FixedBorder = new Thickness(leftBorder, topBorder, rightBorder, bottomBorder);
            Color = Color.White;
        }

        public Rectangle MinSize { get; private set; }
        public Thickness FixedBorder { get; private set; }
        public Color Color { get; set; }

        public void AddPatchField(Rectangle source, Rectangle target)
        {
            _patchFields.Add(new Tuple<Rectangle, Rectangle>(source, target));
        }

        public override void SetBounds(Rectangle bounds)
        {
            _patchFields.Clear();

            var innerTargetWidth = bounds.Width - _commulativeFixedWidth;
            var innerTargetHeight = bounds.Height - _commulativeFixedHeight;

            var innerSourceWidth = _sourceTextureRectangle.Width - _commulativeFixedWidth;
            var innerSourceHeight = _sourceTextureRectangle.Height - _commulativeFixedHeight;

            var source = new Rectangle(_sourceTextureRectangle.X, _sourceTextureRectangle.Y, _leftBorder, _topBorder);
            var target = new Rectangle(bounds.X, bounds.Y, _leftBorder, _topBorder);

            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X += _leftBorder;
            target.X += _leftBorder;
            source.Width = innerSourceWidth;
            target.Width = innerTargetWidth;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X += innerSourceWidth;
            target.X += innerTargetWidth;
            source.Width = _rightBorder;
            target.Width = _rightBorder;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X = _sourceTextureRectangle.X;
            source.Y += _topBorder;
            source.Width = _leftBorder;
            source.Height = innerSourceHeight;
            target.X = bounds.X;
            target.Y += _topBorder;
            target.Width = _leftBorder;
            target.Height = innerTargetHeight;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X += _leftBorder;
            source.Width = innerSourceWidth;
            target.X += _leftBorder;
            target.Width = innerTargetWidth;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X += innerSourceWidth;
            source.Width = _rightBorder;
            target.X += innerTargetWidth;
            target.Width = _rightBorder;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X = _sourceTextureRectangle.X;
            source.Y += innerSourceHeight;
            source.Width = _leftBorder;
            source.Height = _bottomBorder;
            target.X = bounds.X;
            target.Y += innerTargetHeight;
            target.Width = _leftBorder;
            target.Height = _bottomBorder;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X += _leftBorder;
            source.Width = innerSourceWidth;
            target.X += _leftBorder;
            target.Width = innerTargetWidth;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            source.X += innerSourceWidth;
            source.Width = _rightBorder;
            target.X += innerTargetWidth;
            target.Width = _rightBorder;
            if (target.Width > 0 && target.Height > 0) AddPatchField(source, target);

            _bounds = bounds;
        }

        public override Rectangle Bounds => _bounds;

        public override void Draw(SpriteBatch spriteBatch)
        {
            _patchFields.ForEach(n => spriteBatch.Draw(Texture, n.Item2, n.Item1, Color));
        }
    }
}
