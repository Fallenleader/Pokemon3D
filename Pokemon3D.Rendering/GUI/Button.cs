using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering.GUI.ItemDescriptors;

namespace Pokemon3D.Rendering.GUI
{
    public class Button : ContentControl
    {
        private NinePatchSprite _normalSprite;
        private NinePatchSprite _hoverSprite;

        public event Action Click;

        public Button(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements(guiSystem);
        }

        public Button(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            InitializeDrawElements(guiSystem);
        }

        private void InitializeDrawElements(GuiSystem guiSystem)
        {
            var itemDescriptor = guiSystem.GetSkinItemDescriptor<ButtonSkinItemDescriptor>();

            _normalSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.NormalRectangle, itemDescriptor.ButtonBorder);
            _hoverSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.HoverRectangle, itemDescriptor.ButtonBorder);
        }

        public override Rectangle GetMinSize()
        {
            if (Child == null) return ApplyMarginAndHandleSize(_normalSprite.MinSize);

            var childMinSize = Child.GetMinSize();
            return ApplyMarginAndHandleSize(new Rectangle(0, 0, childMinSize.Width + _normalSprite.FixedBorder.Horizontal, childMinSize.Height + _normalSprite.FixedBorder.Vertical));
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            _normalSprite.SetBounds(Bounds);
            _hoverSprite.SetBounds(Bounds);

            if (Child != null)
            {
                var rectangle = Bounds;
                rectangle.X += _normalSprite.FixedBorder.Top;
                rectangle.Y += _normalSprite.FixedBorder.Left;
                rectangle.Width -= _normalSprite.FixedBorder.Vertical;
                rectangle.Height -= _normalSprite.FixedBorder.Vertical;
                Child.Arrange(rectangle);
            }
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            _normalSprite.SetBounds(_normalSprite.Bounds.Translate(x,y));
            _hoverSprite.SetBounds(_normalSprite.Bounds);

            if (Child!= null) Child.Translate(x,y);
        }

        public override void OnMouseUp(EventHandler handler)
        {
            if (Click != null) Click();
        }

        public override void OnMouseDown(EventHandler handler)
        {
            handler.Handled = true;
        }

        public override void OnMouseOver(EventHandler handler)
        {
            handler.Handled = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsMouseOver)
            {
                _hoverSprite.Draw(spriteBatch);
            }
            else
            {
                _normalSprite.Draw(spriteBatch);   
            }
            base.Draw(spriteBatch);
        }
    }
}
