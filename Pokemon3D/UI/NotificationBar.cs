using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.UI
{
    class NotificationBar : GameObject
    {
        private const int ElementPadding = 5;
        private const int ElementMargin = 2;

        private readonly int _width;
        private readonly Color _backgroundColor;
        private readonly SpriteFont _spriteFont;
        private readonly float _notificationTime;
        private readonly List<NotificationItem> _notifications = new List<NotificationItem>();
        
        public NotificationBar(int barWidth)
        {
            _width = barWidth;
            _notificationTime = 2.0f;
            _spriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);
            _backgroundColor = new Color(70, 70, 70);
        }

        public void PushNotification(NotificationKind notificationKind, string message)
        {
            _notifications.Add(new NotificationItem(_notificationTime, notificationKind, message));
        }

        public void Update(float elapsedTime)
        {
            _notifications.ForEach(n => n.Update(elapsedTime));
            _notifications.RemoveAll(n => n.IsFinished);
        }

        public void Draw()
        {
            if (!_notifications.Any()) return;
            
            var elementHeight = _spriteFont.LineSpacing + 2 * ElementPadding;

            var startY = Game.ScreenBounds.Height;
            var startX = (Game.ScreenBounds.Width - _width) /2;

            Game.SpriteBatch.Begin();
            foreach (var notification in _notifications)
            {
                startY -= elementHeight;
                Game.ShapeRenderer.DrawFilledRectangle(startX, startY, _width, elementHeight, _backgroundColor * notification.Alpha);
                Game.SpriteBatch.DrawString(_spriteFont, notification.Message, new Vector2(startX + ElementPadding, startY + ElementPadding), Color.White * notification.Alpha);

                startY -= ElementMargin;
            }
            Game.SpriteBatch.End();
        }
    }
}
