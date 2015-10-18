using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering.GUI.ItemDescriptors;

namespace Pokemon3D.Rendering.GUI
{
    public class Frame : ContentControl
    {
        private SpriteFont _headerFont;
        private NinePatchSprite _contentBorder;
        private NinePatchSprite _headerBorder;
        
        public string Title { get; set; }

        public Frame(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            InitializeDrawElements(guiSystem);
     
            if (element.HasAttribute("Title"))
            {
                Title = element.GetAttribute("Title");
            }
        }

        public Color Color
        {
            get { return _contentBorder.Color; }
            set
            {
                _contentBorder.Color = value;
                _headerBorder.Color = value;
            }
        }

        public Frame(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        private void InitializeDrawElements(GuiSystem guiSystem)
        {
            var itemDescriptor = guiSystem.GetSkinItemDescriptor<FrameSkinItemDescriptor>();

            _headerFont = itemDescriptor.BigFont;
            _contentBorder = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.SourceRectangle, itemDescriptor.FrameBorder);
            _headerBorder = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.SourceRectangle, itemDescriptor.FrameBorder);
        }

        public override Rectangle GetMinSize()
        {
            var headerSize = GetHeaderSize();
            var childContentSize = Child == null ? Rectangle.Empty : Child.GetMinSize();

            var width = Math.Max(childContentSize.Width + _contentBorder.FixedBorder.Horizontal, headerSize.Width);

            return new Rectangle(0, 0, width, childContentSize.Height+_contentBorder.FixedBorder.Vertical+headerSize.Height/2);
        }

        private Rectangle GetHeaderSize()
        {
            if (string.IsNullOrEmpty(Title)) return Rectangle.Empty;

            var textSize = _headerFont.MeasureString(Title).ToPoint();
            return new Rectangle(0, 0, textSize.X + _headerBorder.MinSize.Width, textSize.Y + _headerBorder.MinSize.Height);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            var headerSize = GetHeaderSize();
            var childContentSize = Child == null ? Rectangle.Empty : Child.GetMinSize();
            var topContentPadding = Math.Max(headerSize.Height/2, _contentBorder.FixedBorder.Top);

            var contentBorderBounds = new Rectangle(Bounds.X, Bounds.Y + headerSize.Height/2,
                childContentSize.Width + _contentBorder.FixedBorder.Horizontal,
                childContentSize.Height + _contentBorder.FixedBorder.Bottom + topContentPadding);

            var headerBorderBounds = string.IsNullOrEmpty(Title)
                ? contentBorderBounds
                : new Rectangle(contentBorderBounds.X + contentBorderBounds.Width / 2 - headerSize.Width / 2, Bounds.Y, headerSize.Width,
                    headerSize.Height);

            var contentBounds = childContentSize;
            contentBounds.X = contentBorderBounds.X + _contentBorder.FixedBorder.Left;
            contentBounds.Y = contentBorderBounds.Y + topContentPadding;
            
            var merged = Rectangle.Union(contentBorderBounds, headerBorderBounds);
            var arranged = ArrangeToAlignments(Bounds, merged);

            var differenceX = arranged.X - merged.X;
            var differenceY = arranged.Y - merged.Y;

            contentBorderBounds = contentBorderBounds.Translate(differenceX, differenceY);
            contentBounds = contentBounds.Translate(differenceX, differenceY);
            headerBorderBounds = headerBorderBounds.Translate(differenceX, differenceY);

            _contentBorder.SetBounds(contentBorderBounds);
            if (Child != null) Child.Arrange(contentBounds);
            if (!string.IsNullOrEmpty(Title)) _headerBorder.SetBounds(headerBorderBounds);
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            _headerBorder.SetBounds(_headerBorder.Bounds.Translate(x,y));
            _contentBorder.SetBounds(_contentBorder.Bounds.Translate(x, y));
            if (Child != null) Child.Translate(x,y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _contentBorder.Draw(spriteBatch);
            if (!string.IsNullOrEmpty(Title))
            {
                _headerBorder.Draw(spriteBatch);
                spriteBatch.DrawString(_headerFont, Title, new Vector2(_headerBorder.Bounds.X + _headerBorder.FixedBorder.Left, _headerBorder.Bounds.Y + _headerBorder.FixedBorder.Top), Color.White);
            }

            base.Draw(spriteBatch);
        }
    }
}
