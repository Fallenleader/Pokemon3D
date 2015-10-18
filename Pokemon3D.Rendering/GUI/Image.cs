using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI
{
    public class Image : GuiElement
    {
        private Vector2 _position;
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public Image(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        public Image(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            if(element.HasAttribute("Source"))
            {
                Texture = guiSystem.GameContext.Content.Load<Texture2D>("Textures/" + element.GetAttribute("Source"));
            }

            if(Texture != null)
            {
                SourceRectangle = element.HasAttribute("NormalRectangle") ? element.GetAttribute("NormalRectangle").ParseRectangle() : Texture.Bounds;   
            }
        }

        public override Rectangle GetMinSize()
        {
            return ApplyMarginAndHandleSize(SourceRectangle);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);
            _position = Bounds.Center.ToVector2();
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            _position.X += x;
            _position.Y += y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Texture == null) return;
            spriteBatch.Draw(Texture, _position, SourceRectangle, Color.White, 0.0f, SourceRectangle.Center.ToVector2(), 1.0f, SpriteEffects.None, 0);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
