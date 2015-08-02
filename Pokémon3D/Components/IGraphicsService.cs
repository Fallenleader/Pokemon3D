using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokémon3D.UI;

namespace Pokémon3D.Components
{
    /// <summary>
    /// An interface for a service to draw UI parts.
    /// </summary>
    interface IGraphicsService
    {
        /// <summary>
        /// Draws a shape in a solid color.
        /// </summary>
        void DrawShape(IShape shape, Color color);

        /// <summary>
        /// Draws a shape with a gradient fill.
        /// </summary>
        void DrawShape(IShape shape, Gradient gradient);

        /// <summary>
        /// Draws a shape in a solid color with an offset.
        /// </summary>
        void DrawShape(IShape shape, Vector2 offset, Color color);

        /// <summary>
        /// Draws a shape with a gradient fill with an offset.
        /// </summary>
        void DrawShape(IShape shape, Vector2 offset, Gradient gradient);

        /// <summary>
        /// Draws a shape in a solid color.
        /// </summary>
        void DrawShape(IShape shape, Rectangle destination, Color color);

        /// <summary>
        /// Draws a shape with a gradient fill.
        /// </summary>
        void DrawShape(IShape shape, Rectangle destination, Gradient gradient);

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        void DrawRectangle(Rectangle rectangle, Color color);

        /// <summary>
        /// Draws a line between two points.
        /// </summary>
        void DrawLine(Vector2 start, Vector2 end, double thickness, Color color);

        /// <summary>
        /// Draws a border around a rectangle.
        /// </summary>
        void DrawBorder(Rectangle rectangle, int thickness, Color color);

        /// <summary>
        /// Returns the currently active <see cref="SpriteBatch"/>.
        /// </summary>
        SpriteBatch ActiveSpriteBatch { get; }
    }
}