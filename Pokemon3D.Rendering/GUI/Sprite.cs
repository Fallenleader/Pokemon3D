using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.GUI
{
    public class Sprite
    {
        private Vector2 _position;
        public Texture2D Texture { get; set; }
        
        public float Rotation { get; set; }
        public float Alpha { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public Rectangle Bounds { get; protected set; }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                var difference = value - _position;
                _position = value;

                var bounds = Bounds;
                bounds.X += (int)Math.Round(difference.X, MidpointRounding.AwayFromZero);
                bounds.Y += (int)Math.Round(difference.Y, MidpointRounding.AwayFromZero);
                Bounds = bounds;
            }
        }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Origin = new Vector2(texture.Width * 0.5f, texture.Height *0.5f);
            Alpha = 1.0f;
            Scale = new Vector2(1.0f);
        }

        public virtual void SetBounds(Rectangle rectangle)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White * Alpha, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
        }

        public Vector2 Size
        {
            get { return new Vector2(Texture.Width, Texture.Height);}
        }

        public float Width
        {
            get { return Texture.Width; }
        }

        public float Height
        {
            get { return Texture.Height; }
        }
    }
}
