using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.GUI.ItemDescriptors;

namespace Pokemon3D.Rendering.GUI
{
    class CheckBox : ContentControl
    {
        private Sprite _normalBorder;
        private Sprite _hoverBorder;
        private Sprite _checkmarkSprite;

        public CheckBox(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeComponents();
        }

        public CheckBox(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            InitializeComponents();
        }

        public bool IsChecked { get; set; }

        private void InitializeComponents()
        {
            var itemDescriptor = GuiSystem.GetSkinItemDescriptor<CheckBoxSkinItemDescriptor>();

            _normalBorder = new Sprite(itemDescriptor.SkinTexture);
            _normalBorder.SourceRectangle = itemDescriptor.NormalRectangle;
            _normalBorder.Origin = new Vector2(itemDescriptor.NormalRectangle.Width * 0.5f, itemDescriptor.NormalRectangle.Height * 0.5f);

            _hoverBorder = new Sprite(itemDescriptor.SkinTexture);
            _hoverBorder.SourceRectangle =  itemDescriptor.HoverRectangle;
            _hoverBorder.Origin = new Vector2(itemDescriptor.HoverRectangle.Width * 0.5f, itemDescriptor.HoverRectangle.Height * 0.5f);

            _checkmarkSprite = new Sprite(itemDescriptor.SkinTexture);
            _checkmarkSprite.SourceRectangle = itemDescriptor.CheckmarkRectangle;
            _checkmarkSprite.Origin = new Vector2(itemDescriptor.CheckmarkRectangle.Width * 0.5f, itemDescriptor.CheckmarkRectangle.Height * 0.5f);
        }

        public override Rectangle GetMinSize()
        {
            var contentSize = Child?.GetMinSize() ?? Rectangle.Empty;

            contentSize.Width += (int)20;
            contentSize.Height = (int) Math.Max(contentSize.Height, 20);

            return ApplyMarginAndHandleSize(contentSize);
        }

        public override void Arrange(Rectangle target)
        {
            var widthOfCheckboxMarker = (int)20;
            var targetWithoutMargin = RemoveMargin(target);

            
            targetWithoutMargin.X += widthOfCheckboxMarker;
            targetWithoutMargin.Width -= widthOfCheckboxMarker;

            Child?.Arrange(targetWithoutMargin);

            var usedSize = Child?.Bounds ?? Rectangle.Empty;
            usedSize.Width += widthOfCheckboxMarker;
            usedSize.X -= widthOfCheckboxMarker;

            Bounds = ArrangeToAlignments(targetWithoutMargin, usedSize);
            _normalBorder.Position = new Vector2(Bounds.X, Bounds.Y + Bounds.Height / 2);
            _hoverBorder.Position = new Vector2(Bounds.X, Bounds.Y + Bounds.Height / 2);
            _checkmarkSprite.Position = new Vector2(Bounds.X, Bounds.Y + Bounds.Height / 2);
        }

        public override void Translate(int x, int y)
        {
            _normalBorder.Position += new Vector2(x,y);
            _hoverBorder.Position += new Vector2(x, y);
            _checkmarkSprite.Position += new Vector2(x, y);
        }

        public override void OnMouseUp(EventHandler handler)
        {
            IsChecked = !IsChecked;

            base.OnMouseUp(handler);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsMouseOver)
            {
                _hoverBorder.Draw(spriteBatch);
            }
            else
            {
                _normalBorder.Draw(spriteBatch);
            }
            
            if (IsChecked) _checkmarkSprite.Draw(spriteBatch);
            Child?.Draw(spriteBatch);
        }
    }
}
