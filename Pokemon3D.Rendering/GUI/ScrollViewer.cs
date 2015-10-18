using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI
{
    public class ScrollViewer : ContentControl
    {
        public bool CanScrollHorizontally { get; set; }
        public bool CanScrollVertically { get; set; }

        private RasterizerState _scissorTestRasterizerState;
        private Rectangle _clientClipBounds;
        private bool _isOverScrollbar;
        private ScrollRepresenter _verticalScrollRepresenter;
        private ScrollRepresenter _horizontalScrollRepresenter;

        public ScrollViewer(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements();
        }

        private void InitializeDrawElements()
        {
            var original = GuiSystem.GameContext.GraphicsDevice.RasterizerState;

            _scissorTestRasterizerState = new RasterizerState
            {
                CullMode = original.CullMode,
                DepthBias = original.DepthBias,
                DepthClipEnable = original.DepthClipEnable,
                FillMode = original.FillMode,
                MultiSampleAntiAlias = original.MultiSampleAntiAlias,
                ScissorTestEnable = true,
                SlopeScaleDepthBias = original.SlopeScaleDepthBias,
            };

            _verticalScrollRepresenter = new ScrollRepresenter();
            _horizontalScrollRepresenter = new ScrollRepresenter();
        }

        public ScrollViewer(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            if (element.HasAttribute("CanScrollHorizontally"))
            {
                CanScrollHorizontally = Boolean.Parse(element.GetAttribute("CanScrollHorizontally"));
            }
            if (element.HasAttribute("CanScrollVertically"))
            {
                CanScrollVertically = Boolean.Parse(element.GetAttribute("CanScrollVertically"));
            }

            InitializeDrawElements();
        }

        private const int SliderSize = 20;

        public override Rectangle GetMinSize()
        {
            var childSize = Child != null ? Child.GetMinSize() : Rectangle.Empty;

            var size = new Rectangle
            {
                Width = CanScrollHorizontally ? SliderSize : (childSize.Width + (CanScrollVertically ? SliderSize : 0)),
                Height = CanScrollVertically ? SliderSize : (childSize.Height + (CanScrollHorizontally ? SliderSize : 0))
            };

            return ApplyMarginAndHandleSize(size);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            var childSize = Child != null ? Child.GetMinSize() : Rectangle.Empty;

            var childArrange = Bounds;
            childArrange.Width = CanScrollHorizontally ? childSize.Width : Bounds.Width;
            childArrange.Height = CanScrollVertically ? childSize.Height : Bounds.Height;

            _clientClipBounds = Bounds;
            _clientClipBounds.Width -= CanScrollVertically ? SliderSize : 0;
            _clientClipBounds.Height -= CanScrollHorizontally ? SliderSize : 0;

            if (CanScrollVertically)
            {
                _verticalScrollRepresenter.SetRange(_clientClipBounds.Height, childArrange.Height, SliderSize);
                if (!_verticalScrollRepresenter.CanContentScroll)
                {
                    _clientClipBounds.Width += SliderSize;
                }
            }

            if (CanScrollHorizontally)
            {
                _horizontalScrollRepresenter.SetRange(_clientClipBounds.Width, childArrange.Width, SliderSize);
                if (!_horizontalScrollRepresenter.CanContentScroll)
                {
                    _clientClipBounds.Height += SliderSize;
                }
            }

            if (Child != null) Child.Arrange(childArrange);
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x,y);
            if (Child != null) Child.Translate(x,y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawContentWithScissoring(spriteBatch);

            if (CanScrollVertically && _verticalScrollRepresenter.CanContentScroll)
            {
                GuiSystem.GameContext.ShapeRenderer.DrawFilledRectangle(_clientClipBounds.Right + 1, _clientClipBounds.Top, SliderSize, _clientClipBounds.Height, Color.Red);

                var thumb = GetThumbRectangleVertical();

                GuiSystem.GameContext.ShapeRenderer.DrawFilledRectangle(thumb.X, thumb.Y, thumb.Width, thumb.Height, Color.Blue);
            }

            if (CanScrollHorizontally && _horizontalScrollRepresenter.CanContentScroll)
            {
                GuiSystem.GameContext.ShapeRenderer.DrawFilledRectangle(_clientClipBounds.Left, _clientClipBounds.Bottom - SliderSize, _clientClipBounds.Width, SliderSize, Color.Red);

                var thumb = GetThumbRectangleHorizontal();

                GuiSystem.GameContext.ShapeRenderer.DrawFilledRectangle(thumb.X, thumb.Y, thumb.Width, thumb.Height, Color.Blue);
            }
        }

        private void DrawContentWithScissoring(SpriteBatch spriteBatch)
        {
            var graphicsDevice = spriteBatch.GraphicsDevice;

            spriteBatch.End();

            graphicsDevice.ScissorRectangle = _clientClipBounds;

            spriteBatch.Begin(rasterizerState: _scissorTestRasterizerState);
            Child?.Translate(-_horizontalScrollRepresenter.ScrollValueCalculated, -_verticalScrollRepresenter.ScrollValueCalculated);
            base.Draw(spriteBatch);
            spriteBatch.End();
            Child?.Translate(_horizontalScrollRepresenter.ScrollValueCalculated, _verticalScrollRepresenter.ScrollValueCalculated);

            spriteBatch.Begin();
        }

        private Rectangle GetThumbRectangleVertical()
        {
            return new Rectangle(_clientClipBounds.Right + 1, _clientClipBounds.Top + _verticalScrollRepresenter.ScrollValueDisplayed, SliderSize, _verticalScrollRepresenter.ThumbSize);
        }

        private Rectangle GetThumbRectangleHorizontal()
        {
            return new Rectangle(_clientClipBounds.Left + _verticalScrollRepresenter.ScrollValueDisplayed,
                                 _clientClipBounds.Bottom - SliderSize,
                                 _verticalScrollRepresenter.ThumbSize, SliderSize);
        }

        public override void OnMouseLeft(EventHandler handler)
        {
            _isOverScrollbar = false;
        }

        public override void OnMouseOver(EventHandler handler)
        {
            var mouseState = Mouse.GetState();
            handler.Handled = GetThumbRectangleVertical().Contains(mouseState.X, mouseState.Y);
        }

        public override void OnMouseDown(EventHandler handler)
        {
            var mouseState = Mouse.GetState();
            _isOverScrollbar = GetThumbRectangleVertical().Contains(mouseState.X, mouseState.Y);
            if (_isOverScrollbar)
            {
                handler.Handled = true;
            }
        }

        public override void OnMouseMove(MouseMovedEventHandler handler)
        {
            if (!_isOverScrollbar) return;
            _verticalScrollRepresenter.Scroll(handler.Y);
        }

        public override void OnMouseUp(EventHandler handler)
        {
            _isOverScrollbar = false;
        }
    }
}
