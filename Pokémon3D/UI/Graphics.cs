using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokémon3D.GameCore;

namespace Pokémon3D.UI
{
    /// <summary>
    /// A component to draw basic shapes.
    /// </summary>
    class Graphics : IDisposable
    {
        private Texture2D _pixel;
        private SpriteBatch _batch;

        public Graphics(SpriteBatch batch)
        {
            _pixel = new Texture2D(GameController.Instance.GraphicsDevice, 1, 1);
            _pixel.SetData(new Color[] { Color.White });

            _batch = batch;
        }
        
        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            _batch.Draw(_pixel, rectangle, color);
        }

        /// <summary>
        /// Draws a border around a rectangle.
        /// </summary>
        public void DrawBorder(Rectangle rectangle, int thickness, Color color)
        {
            DrawRectangle(new Rectangle(rectangle.X + thickness, rectangle.Y, rectangle.Width - thickness, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y + thickness, thickness, rectangle.Height - thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width - thickness, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height - thickness), color);
        }

        /// <summary>
        /// Draws a line between two points.
        /// </summary>
        public void DrawLine(Vector2 start, Vector2 end, double thickness, Color color)
        {
            double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
            double length = Vector2.Distance(start, end);

            _batch.Draw(_pixel, start, null, color, (float)angle, Vector2.Zero, new Vector2((float)length, (float)thickness), SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws a shape in a solid color.
        /// </summary>
        public void DrawShape(IShape shape, Color color)
        {
            DrawShape(shape, shape.Bounds, color);
        }

        /// <summary>
        /// Draws a shape with a gradient fill.
        /// </summary>
        public void DrawShape(IShape shape, Gradient gradient)
        {
            DrawShape(shape, shape.Bounds, gradient);
        }

        /// <summary>
        /// Draws a shape in a solid color with an offset.
        /// </summary>
        public void DrawShape(IShape shape, Vector2 offset, Color color)
        {
            Rectangle destination = shape.Bounds;
            destination.Offset(offset);

            DrawShape(shape, destination, color);
        }

        /// <summary>
        /// Draws a shape with a gradient fill with an offset.
        /// </summary>
        public void DrawShape(IShape shape, Vector2 offset, Gradient gradient)
        {
            Rectangle destination = shape.Bounds;
            destination.Offset(offset);

            DrawShape(shape, destination, gradient);
        }

        /// <summary>
        /// Draws a shape in a solid color to a destination rectangle.
        /// </summary>
        public void DrawShape(IShape shape, Rectangle destination, Color color)
        {
            Texture2D texture = ShapeTextureProvider.GetTexture(GameController.Instance.GraphicsDevice, shape);
            _batch.Draw(texture, destination, color);
        }

        /// <summary>
        /// Draws a shape with a gradient fill to a destination rectangle.
        /// </summary>
        public void DrawShape(IShape shape, Rectangle destination, Gradient gradient)
        {
            Texture2D texture = GradientTextureProvider.GetTexture(GameController.Instance.GraphicsDevice, gradient, shape);
            _batch.Draw(texture, destination, Color.White);
        }

        public void Dispose()
        {
            _pixel.Dispose();
            _batch.Dispose();
        }
    }
}