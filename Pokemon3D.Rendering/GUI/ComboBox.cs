using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering.GUI.ItemDescriptors;

namespace Pokemon3D.Rendering.GUI
{
    public class ComboBox : GuiElement
    {
        private NinePatchSprite _normalBackgroundSprite;
        private NinePatchSprite _hoverBackgroundSprite;
        private NinePatchSprite _popupSprite;
        private Sprite _toggleButtonSprite;
        private SpriteText _displayedText;

        private List<SpriteText> _items;

        public int SelectedIndex { get; set; }

        public ComboBox(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements(guiSystem);
        }

        public ComboBox(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            InitializeDrawElements(guiSystem);
        }

        private void InitializeDrawElements(GuiSystem guiSystem)
        {
            _items = new List<SpriteText>();

            var itemDescriptor = guiSystem.GetSkinItemDescriptor<ComboBoxItemDescriptor>();

            _normalBackgroundSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.NormalRectangle, itemDescriptor.Border);
            _hoverBackgroundSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.HoverRectangle, itemDescriptor.Border);
            _popupSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.PopupRectangle, itemDescriptor.PopupBorder);
            _toggleButtonSprite = new Sprite(itemDescriptor.SkinTexture)
            {
                SourceRectangle = itemDescriptor.ToggleButtonRectangle
            };

            _displayedText = new SpriteText(itemDescriptor.NormalFont)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Middle,
                Color = Color.Black
            };
        }

        public void AddItem(string value)
        {
            _items.Add(new SpriteText(_displayedText.Font, value)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Middle,
                Color = Color.Black
            });
        }

        public override Rectangle GetMinSize()
        {
            var widthOfText = GetLongestTextWidth();

            var width = widthOfText + _normalBackgroundSprite.FixedBorder.Horizontal + (int)_toggleButtonSprite.Width;
            var height = (int)Math.Max(_toggleButtonSprite.Height, _displayedText.Font.LineSpacing) + _normalBackgroundSprite.FixedBorder.Vertical;
            var contentSize = new Rectangle(0,0, width, height);

            return ApplyMargin(contentSize);
        }

        private int GetLongestTextWidth()
        {
            return (int)Math.Ceiling(_items.Count > 0 ? _items.Max(i => i.TextSize.X) : 0.0f);
        }

        public override void Arrange(Rectangle target)
        {
            var minSize = RemoveMargin(GetMinSize());
            var availableBounds = RemoveMargin(target);

            Bounds = ArrangeToAlignments(availableBounds, minSize);
            _normalBackgroundSprite.SetBounds(Bounds);
            _hoverBackgroundSprite.SetBounds(Bounds);
            _toggleButtonSprite.Position = new Vector2(Bounds.Right - _normalBackgroundSprite.FixedBorder.Right - _toggleButtonSprite.Width * 0.5f,
                                                       Bounds.Top + Bounds.Height / 2);
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            _normalBackgroundSprite.SetBounds(_normalBackgroundSprite.Bounds.Translate(x, y));
            _hoverBackgroundSprite.SetBounds(_hoverBackgroundSprite.Bounds);
            _popupSprite.SetBounds(_popupSprite.Bounds.Translate(x,y));
            _toggleButtonSprite.Position += new Vector2(x,y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _normalBackgroundSprite.Draw(spriteBatch);
            _toggleButtonSprite.Draw(spriteBatch);

            if (SelectedIndex >= 0 && SelectedIndex <= _items.Count)
            {
                _displayedText.Text = _items[SelectedIndex].Text;
                var bounds = _normalBackgroundSprite.Bounds;
                bounds.X += _normalBackgroundSprite.FixedBorder.Left;
                _displayedText.SetTargetRectangle(bounds);
                _displayedText.Draw(spriteBatch);
            }
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
