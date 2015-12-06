using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI
{
    public class Image : GuiElement
    {
        private Vector2 _position;
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float Scale { get; set; }

        public Image(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        public Image(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            if(element.HasAttribute("Source"))
            {
                Texture = guiSystem.GameContext.Content.Load<Texture2D>(element.GetAttribute("Source"));
            }

            Scale = element.HasAttribute("Scale") ? float.Parse(element.GetAttribute("Scale"), CultureInfo.InvariantCulture) : 1.0f;

            if(Texture != null)
            {
                SourceRectangle = element.HasAttribute("NormalRectangle") ? StringParser.ParseRectangle(element.GetAttribute("NormalRectangle")) : Texture.Bounds;   
            }

            Bounds = SourceRectangle.Scale(Scale);
        }

        public override Rectangle GetMinSize()
        {
            return ApplyMargin(SourceRectangle.Scale(Scale));
        }

        public override void Arrange(Rectangle target)
        {
            var availableSpace = RemoveMargin(target);
            var scaledBounds = SourceRectangle.Scale(Scale);

            Bounds = ArrangeToAlignments(availableSpace, scaledBounds);
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
            spriteBatch.Draw(Texture, _position, SourceRectangle, Color.White, 0.0f, Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
