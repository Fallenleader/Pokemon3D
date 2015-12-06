using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.GUI
{
    public class EventHandler
    {
        public bool Handled { get; set; }
    }

    public class MouseMovedEventHandler : EventHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public abstract class GuiElement
    {
        private bool _isActive;
        public const int SizeNotSet = int.MinValue;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    Children.ForEach(c => c.IsActive = _isActive);
                }
            }
        }

        public bool IsMouseOver { get; internal set; }

        protected GuiSystem GuiSystem { get; private set; }

        public static GuiElement CreateFromXmlType(GuiSystem guiSystem, XmlElement element)
        {
            switch (element.LocalName)
            {
                case "Frame": return new Frame(guiSystem, element);
                case "TextBlock": return new TextBlock(guiSystem, element);
                case "StackPanel": return new StackPanel(guiSystem, element);
                case "Image": return new Image(guiSystem, element);
                case "Grid": return new Grid(guiSystem, element);
                case "Button": return new Button(guiSystem, element);
                case "TextBox": return new TextBox(guiSystem, element);
                case "ScrollViewer": return new ScrollViewer(guiSystem, element);
                case "CheckBox": return new CheckBox(guiSystem, element);
            }

            throw new ArgumentException("Invalid Element Type", "element");
        }

        protected GuiElement(GuiSystem guiSystem)
        {
            GuiSystem = guiSystem;
            Children = new List<GuiElement>();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Middle;
            IsActive = true;
        }

        protected GuiElement(GuiSystem guiSystem, XmlElement element)
        {
            GuiSystem = guiSystem;
            Children = new List<GuiElement>();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Middle;
            IsActive = true;

            if (element.HasAttribute("Width")) Width = int.Parse(element.GetAttribute("Width"));
            if (element.HasAttribute("Height")) Height = int.Parse(element.GetAttribute("Height"));
            if (element.HasAttribute("Margin")) Margin = Thickness.Parse(element.GetAttribute("Margin"));
            if (element.HasAttribute("Id")) Id = element.GetAttribute("Id");
            if (element.HasAttribute("HorizontalAlignment"))
            {
                HorizontalAlignment =
                    (HorizontalAlignment)
                        Enum.Parse(typeof (HorizontalAlignment), element.GetAttribute("HorizontalAlignment"));
            }

            if (element.HasAttribute("VerticalAlignment"))
            {
                VerticalAlignment =
                    (VerticalAlignment)
                        Enum.Parse(typeof(VerticalAlignment), element.GetAttribute("VerticalAlignment"));
            }
        }

        public TGuiElement FindGuiElementById<TGuiElement>(string id) where TGuiElement : GuiElement
        {
            if (Id == id) return (TGuiElement)this;
            foreach (var child in Children)
            {
                var element = child.FindGuiElementById<TGuiElement>(id);
                if (element != null) return element;
            }

            return null;
        }

        protected Rectangle ApplyMarginAndHandleSize(Rectangle rectangle)
        {
            rectangle.X -= Margin.Left;
            rectangle.Y -= Margin.Top;
            rectangle.Width = Math.Max(Width, rectangle.Width + Margin.Horizontal);
            rectangle.Height = Math.Max(Height, rectangle.Height + Margin.Vertical);

            return rectangle;
        }

        protected Rectangle RemoveMargin(Rectangle rectangle)
        {
            rectangle.X += Margin.Left;
            rectangle.Y += Margin.Top;
            rectangle.Width -= Margin.Horizontal;
            rectangle.Height -= Margin.Vertical;
            return rectangle;
        }

        public string Id { get; }
        public List<GuiElement> Children { get; }
        public Rectangle Bounds { get; protected set; }
        public Thickness Margin { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        public abstract Rectangle GetMinSize();

        public abstract void Arrange(Rectangle target);

        internal Rectangle ArrangeToAlignments(Rectangle availableBounds, Rectangle elementBounds)
        {
            var finalRectangle = new Rectangle(0,0,elementBounds.Width, elementBounds.Height);

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    finalRectangle.X = availableBounds.X;
                    break;
                case HorizontalAlignment.Center:
                    finalRectangle.X = availableBounds.X + (availableBounds.Width/2 - elementBounds.Width/2);
                    break;
                case HorizontalAlignment.Right:
                    finalRectangle.X = availableBounds.Right - elementBounds.Width;
                    break;
                case HorizontalAlignment.Stretch:
                    finalRectangle.X = availableBounds.X;
                    finalRectangle.Width = availableBounds.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    finalRectangle.Y = availableBounds.Y;
                    break;
                case VerticalAlignment.Middle:
                    finalRectangle.Y = availableBounds.Y + (availableBounds.Height / 2 - elementBounds.Height / 2);
                    break;
                case VerticalAlignment.Bottom:
                    finalRectangle.Y = availableBounds.Bottom - elementBounds.Height;
                    break;
                case VerticalAlignment.Stretch:
                    finalRectangle.Y = availableBounds.Y;
                    finalRectangle.Height = availableBounds.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return finalRectangle;
        }

        public abstract void Translate(int x, int y);

        public virtual void OnGotFocus(EventHandler handler)
        {
        }

        public virtual void OnFocusLost(EventHandler handler)
        {

        }

        public virtual void OnMouseOver(EventHandler handler)
        {
        }

        public virtual void OnMouseLeft(EventHandler handler)
        {
        }

        public virtual void OnMouseDown(EventHandler handler)
        {
            
        }

        public virtual void OnMouseUp(EventHandler handler)
        {
           
        }

        public virtual void OnMouseMove(MouseMovedEventHandler handler)
        {
            
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(float elapsedTime);
    }
}
