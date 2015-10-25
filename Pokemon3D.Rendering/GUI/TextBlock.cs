using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering.GUI.ItemDescriptors;

namespace Pokemon3D.Rendering.GUI
{
    public class TextBlock : GuiElement
    {
        private SpriteText _spriteText;

        public string Text
        {
            get { return _spriteText.Text; }
            set
            {
                _spriteText.Text = value;
            }
        }

        public Color Color
        {
            get { return _spriteText.Color; }
            set { _spriteText.Color = value; }
        }

        public TextBlock(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            InitializeDrawElements();
            if (element.HasAttribute("Text")) Text = element.GetAttribute("Text");
        }

        public TextBlock(GuiSystem guiSystem)
            : base(guiSystem)
        {
            InitializeDrawElements();
        }

        private void InitializeDrawElements()
        {
            var descriptor = GuiSystem.GetSkinItemDescriptor<TextBlockSkinItemDescriptor>();
            _spriteText = new SpriteText(descriptor.NormalFont);

            _spriteText.HorizontalAlignment = HorizontalAlignment;
            _spriteText.VerticalAlignment = VerticalAlignment;
            _spriteText.Color = Color.Black;
        }

        internal Rectangle GetMinSize(bool includeHeightForEmptyText)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return ApplyMarginAndHandleSize(includeHeightForEmptyText ? new Rectangle(0,0,0,_spriteText.LineSpacing) : new Rectangle());
            }

            return ApplyMarginAndHandleSize(new Rectangle(0, 0, (int)Math.Round(_spriteText.TextSize.X), (int)Math.Round(_spriteText.TextSize.Y)));
        }

        public override Rectangle GetMinSize()
        {
            return GetMinSize(false);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);
            _spriteText.SetTargetRectangle(Bounds);
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            _spriteText.SetTargetRectangle(_spriteText.TargetRectangle.Translate(x, y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _spriteText.Draw(spriteBatch);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
