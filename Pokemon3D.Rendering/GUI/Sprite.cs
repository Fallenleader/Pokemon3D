using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI
{
    public class Sprite
    {
        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Alpha = 1.0f;
            Scale = new Vector2(1.0f);
        }

        public Texture2D Texture { get; set; }
        
        public float Alpha { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        public virtual Rectangle Bounds
        {
            get
            {
                var srcRect = GetSourceRectangle();
                var size = new Vector2(srcRect.Width * Scale.X, srcRect.Height* Scale.Y).SnapToPixels();
                return new Rectangle((int) Position.X, (int)Position.Y, (int) size.X, (int) size.Y);
            }
        }

        public Vector2 Position { get; set; }
        public Rectangle? SourceRectangle { get; set; }

        public virtual void SetBounds(Rectangle rectangle)
        {
            var bounds = GetSourceRectangle();
            var scaleX = bounds.Width/(float)rectangle.Width;
            var scaleY = bounds.Height/(float)rectangle.Height;
            Scale = new Vector2(scaleX, scaleY);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = GetSourceRectangle();
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White * Alpha, Rotation, new Vector2(sourceRectangle.Width*0.5f, sourceRectangle.Height * 0.5f), Scale, SpriteEffects.None, 0.0f);
        }

        public Vector2 Size => new Vector2(Bounds.Width, Bounds.Height);

        public float Width => GetSourceRectangle().Width * Scale.X;

        public float Height => GetSourceRectangle().Height * Scale.Y;

        private Rectangle GetSourceRectangle()
        {
            return (SourceRectangle ?? Texture?.Bounds).GetValueOrDefault(Rectangle.Empty);
        }
    }
}
