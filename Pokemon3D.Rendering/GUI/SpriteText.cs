using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI
{
    public class SpriteText
    {
        public SpriteFont Font { get; }

        private string _text;

        public SpriteText(SpriteFont font, string text = null)
        {
            Font = font;
            Text = text;
            Color = Color.White;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Middle;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (Equals(_text, value)) return;
                _text = value;
                TextSize = string.IsNullOrEmpty(_text) ? Vector2.Zero : Font.MeasureString(_text);

                if (TargetRectangle != Rectangle.Empty)
                {
                    SetTargetRectangle(TargetRectangle);
                }
            }
        }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 TextSize { get; private set; }
        public Color Color { get; set; }
        public int LineSpacing { get { return Font.LineSpacing; } }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        public Rectangle TargetRectangle { get; private set; }

        public void SetTargetRectangle(Rectangle rectangle)
        {
            TargetRectangle = rectangle;
            var position = Vector2.Zero;
            var origin = Vector2.Zero;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    position.X = TargetRectangle.X;
                    origin.X = 0;
                    break;
                case HorizontalAlignment.Center:
                    position.X = TargetRectangle.X + TargetRectangle.Width / 2;
                    origin.X = TextSize.X/2;
                    break;
                case HorizontalAlignment.Right:
                    position.X = TargetRectangle.X + TargetRectangle.Width;
                    origin.X = TextSize.X;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    position.Y = TargetRectangle.Y;
                    origin.Y = 0;
                    break;
                case VerticalAlignment.Middle:
                    position.Y = TargetRectangle.Y + TargetRectangle.Height / 2;
                    origin.Y = TextSize.Y/2;
                    break;
                case VerticalAlignment.Bottom:
                    position.Y = TargetRectangle.Y + TargetRectangle.Height;
                    origin.Y = TextSize.Y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Position = position.SnapToPixels();
            Origin = origin.SnapToPixels();
        }

        public Rectangle GetBounds(bool lineSpacingHeightForEmpty)
        {
            var bounds = string.IsNullOrEmpty(_text) ? new Rectangle(0, 0, 0,lineSpacingHeightForEmpty ? LineSpacing : 0) : new Rectangle(0,0, (int) TextSize.X, (int) TextSize.Y);

            bounds.X = (int) (Position.X - Origin.X);
            bounds.Y =  (int) (Position.Y - Origin.Y);

            return bounds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Text)) return;
            spriteBatch.DrawString(Font, Text, Position,Color, 0.0f, Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
