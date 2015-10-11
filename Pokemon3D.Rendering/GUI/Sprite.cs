using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.GUI
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Alpha { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Origin = new Vector2(texture.Width * 0.5f, texture.Height *0.5f);
            Alpha = 1.0f;
            Scale = new Vector2(1.0f);
        }

        public void Draw(SpriteBatch spriteBatch)
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
