using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.GUI
{
    public class GuiPanel
    {
        private readonly List<GuiElement> _elements = new List<GuiElement>();

        private GuiElement _currentMouseOverElement;
        private GuiElement _focusedElement;
        private MouseState _lastMouseState;
        private readonly GameContext _gameContext;

        public bool IsEnabled { get; set; }

        public GuiPanel(GameContext gameContext)
        {
            _gameContext = gameContext;
            _lastMouseState = Mouse.GetState();
            IsEnabled = true;
        }

        public void AddElement(GuiElement element)
        {
            _elements.Add(element);
            ArrangeElement(element);
        }

        private void ArrangeElement(GuiElement element)
        {
            element.Arrange(_gameContext.ScreenBounds);
        }

        public void Update(float elapsedTime)
        {
            if (!IsEnabled) return;
            var newMouseState = Mouse.GetState();

            HandleDisabledElements();

            foreach (var element in _elements.Where(e => e.IsActive))
            {
                UpdateElementUiEvents(element, newMouseState);
                element.Update(elapsedTime);
            }

            _lastMouseState = newMouseState;
        }

        private void HandleDisabledElements()
        {
            if (_focusedElement != null && !_focusedElement.IsActive)
            {
                _focusedElement = null;
            }
        }

        private static GuiElement InvokeEventForChildren<THandler>(MouseState state, GuiElement parent, THandler handler, Action<GuiElement, THandler> action) where THandler : EventHandler
        {
            foreach (var childElement in parent.Children)
            {
                if (childElement.Bounds.Contains(state.X, state.Y))
                {
                    action(childElement, handler);
                    if (handler.Handled) return childElement;
                     return InvokeEventForChildren(state, childElement,handler, action);
                }
            }
            return null;
        }

        private static void HandleIsMouseOver(GuiElement element, MouseState newState)
        {
            if (!element.Bounds.Contains(newState.X, newState.Y)) return;
            HandleIsMouseOverForAllChildren(element, newState);
        }

        private static void HandleIsMouseOverForAllChildren(GuiElement parent, MouseState newState)
        {
            foreach (var childElement in parent.Children)
            {
                if (childElement.Bounds.Contains(newState.X, newState.Y) )
                {
                    if (!childElement.IsMouseOver)
                    {
                        childElement.OnMouseOver(new EventHandler());
                        childElement.IsMouseOver = true;
                    }
                    HandleIsMouseOverForAllChildren(childElement, newState);
                }
                else if (childElement.IsMouseOver)
                {
                    childElement.IsMouseOver = false;
                    childElement.OnMouseLeft(new EventHandler());
                }
            }
        }

        private void UpdateElementUiEvents(GuiElement element, MouseState newState)
        {
            var mouseMoveDeltaX = newState.X - _lastMouseState.X;
            var mouseMoveDeltaY = newState.Y - _lastMouseState.Y;
            var isLeftMouseUp = _lastMouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released;
            var isLeftMouseDown = _lastMouseState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed;
            var isMouseMoved = mouseMoveDeltaX != 0 || mouseMoveDeltaY != 0;

            if (isMouseMoved)
            {
                HandleMouseMoved(element, newState, new MouseMovedEventHandler {X = mouseMoveDeltaX, Y = mouseMoveDeltaY});
                HandleIsMouseOver(element, newState);
            }

            if (isLeftMouseDown)
            {
                InvokeEventForChildren(newState, element, new EventHandler(), (g, e) => g.OnMouseDown(e));
                var newFocusedElement = InvokeEventForChildren(newState, element, new EventHandler(), (g, e) => g.OnGotFocus(e));
                if (newFocusedElement != _focusedElement)
                {
                    if (_focusedElement != null) _focusedElement.OnFocusLost(new EventHandler());
                    _focusedElement = newFocusedElement;
                }
            }

            if (isLeftMouseUp)
            {
                InvokeEventForChildren(newState, element, new EventHandler(), (g, e) => g.OnMouseUp(e));
            }
        }

        private void HandleMouseMoved(GuiElement element, MouseState newState, MouseMovedEventHandler handler)
        {
            if (!element.Bounds.Contains(newState.X, newState.Y)) return;

            HandleMouseMovedForChildren(element, newState, handler);
        }

        private void HandleMouseMovedForChildren(GuiElement element, MouseState newState, MouseMovedEventHandler handler)
        {
            foreach (var childElement in element.Children)
            {
                if (childElement.Bounds.Contains(newState.X, newState.Y) && childElement.IsMouseOver)
                {
                    childElement.OnMouseMove(handler);
                    if (handler.Handled) return;
                    HandleMouseMovedForChildren(childElement, newState, handler);
                }
            }
        }

        public void Draw()
        {
            if (!IsEnabled) return;
            foreach (var element in _elements.Where(e => e.IsActive))
            {
                element.Draw(_gameContext.SpriteBatch);
            }
        }
    }
}
