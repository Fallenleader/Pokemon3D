using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Common
{
    public class ShapeRenderer
    {
        private readonly Texture2D _blank;
        private readonly SpriteBatch _spriteBatch;

        public ShapeRenderer(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            var texture = new Texture2D(device, 1, 1);
            texture.SetData(new[] { Color.White });
            _blank = texture;
            _spriteBatch = spriteBatch;
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            var dX = x2 - x1;
            var dY = y2 - y1;
            var length = (float)Math.Sqrt(dX * dX + dY * dY);

            _spriteBatch.Draw(_blank, new Vector2(x1, y1), null, color, (float)Math.Atan2(dY, dX), Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        public void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            _spriteBatch.Draw(_blank, new Rectangle(x, y, width, 1), color);
            _spriteBatch.Draw(_blank, new Rectangle(x, y + height - 1, width, 1), color);
            _spriteBatch.Draw(_blank, new Rectangle(x, y, 1, height), color);
            _spriteBatch.Draw(_blank, new Rectangle(x + width - 1, y, 1, height), color);
        }

        public void DrawFilledRectangle(int x, int y, int width, int height, Color color)
        {
            _spriteBatch.Draw(_blank, new Rectangle(x, y, width, height), color);
        }

        public void DrawRectangle(Rectangle target, Color color)
        {
            DrawRectangle(target.X, target.Y, target.Width, target.Height, color);
        }

        public void DrawCircle(int x, int y, int radius, int slices, Color color)
        {
            var deltaAngle = 1 / (float)slices * MathHelper.TwoPi;
            var segmentWidth = 2f * (float)Math.Sin(deltaAngle * 0.5f) * radius;

            for (var i = 0; i <= slices; i++)
            {
                _spriteBatch.Draw(_blank,
                    new Vector2(x + (float)(Math.Cos(i * deltaAngle) * radius),
                                y + (float)(Math.Sin(i * deltaAngle) * radius)).SnapToPixels(),
                    null, color, i * deltaAngle + deltaAngle * 0.5f + MathHelper.PiOver2,
                    Vector2.Zero, new Vector2(segmentWidth, 1), SpriteEffects.None, 0);
            }
        }
    }
}
