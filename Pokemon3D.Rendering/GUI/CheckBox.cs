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

            _hoverBorder = new Sprite(itemDescriptor.SkinTexture);
            _hoverBorder.SourceRectangle =  itemDescriptor.HoverRectangle;

            _checkmarkSprite = new Sprite(itemDescriptor.SkinTexture);
            _checkmarkSprite.SourceRectangle = itemDescriptor.CheckmarkRectangle;
        }

        public override Rectangle GetMinSize()
        {
            var contentSize = Child?.GetMinSize() ?? Rectangle.Empty;

            contentSize.Width += (int)_normalBorder.Width;
            contentSize.Height = (int) Math.Max(contentSize.Height, _normalBorder.Width);

            return ApplyMarginAndHandleSize(contentSize);
        }

        public override void Arrange(Rectangle target)
        {
            var targetWithoutMargin = RemoveMargin(target);
            var minSizeOfContent = Child?.GetMinSize() ?? Rectangle.Empty;

            var usedWidth = Math.Min(targetWithoutMargin.Width, (int)_normalBorder.Width + minSizeOfContent.Width);
            var usedHeight = Math.Min(targetWithoutMargin.Height, Math.Max(minSizeOfContent.Height, (int)_normalBorder.Height));
            var usedSize = new Rectangle(0,0, usedWidth, usedHeight);
            
            Bounds = ArrangeToAlignments(targetWithoutMargin, usedSize);
            _normalBorder.Position = new Vector2(Bounds.X + _normalBorder.Width * 0.5f, Bounds.Y + Bounds.Height / 2);
            _hoverBorder.Position = new Vector2(Bounds.X + _normalBorder.Width * 0.5f, Bounds.Y + Bounds.Height / 2);
            _checkmarkSprite.Position = new Vector2(Bounds.X + _normalBorder.Width * 0.5f, Bounds.Y + Bounds.Height / 2);

            if (Child != null)
            {
                var boundsOfChild = Bounds;
                boundsOfChild.X += (int)_normalBorder.Width;
                boundsOfChild.Width -= (int)_normalBorder.Width;
                Child.Arrange(boundsOfChild);
            }
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
