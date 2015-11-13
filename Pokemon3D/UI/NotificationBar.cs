using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.UI
{
    class NotificationBar : GameObject
    {
        private Color _backgroundColor;
        private SpriteFont _spriteFont;
        private float _notificationTime;
        private List<NotificationItem> _notifications = new List<NotificationItem>();
        
        public NotificationBar()
        {
            _notificationTime = 2.0f;
            _spriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);
            _backgroundColor = new Color(70, 70, 70);
        }

        public void PushNotification(string message)
        {
            _notifications.Add(new NotificationItem
            {
                Message = message,
                RemainingLifeTime = _notificationTime
            });
        }

        public void Update(float elapsedTime)
        {
            foreach(var notification in _notifications)
            {
                notification.RemainingLifeTime = MathHelper.Max(notification.RemainingLifeTime - elapsedTime, 0.0f);
            }
            _notifications.RemoveAll(n => n.RemainingLifeTime <= 0.0f);
        }

        public void Draw()
        {
            if (!_notifications.Any()) return;
            
            const int Width = 400;
            const int elementPadding = 5;
            const int elementMargin = 2;
            var elementHeight = _spriteFont.LineSpacing + 2 * elementPadding;

            var startY = Game.ScreenBounds.Height;
            var startX = (Game.ScreenBounds.Width - Width) /2;

            Game.SpriteBatch.Begin();
            foreach (var notification in _notifications)
            {
                var alpha = 1.0f;
                if (notification.RemainingLifeTime > 2.0 - 0.25f)
                {
                    alpha = 1.0f - (notification.RemainingLifeTime - 1.75f) / 0.25f;
                }
                else if (notification.RemainingLifeTime < 0.25f)
                {
                    alpha = notification.RemainingLifeTime / 0.25f;
                }

                startY -= elementHeight;
                Game.ShapeRenderer.DrawFilledRectangle(startX, startY, Width, elementHeight, _backgroundColor * alpha);

                Game.SpriteBatch.DrawString(_spriteFont, notification.Message, new Vector2(startX + elementPadding, startY + elementPadding), Color.White * alpha);

                startY -= elementMargin;
            }
            Game.SpriteBatch.End();
        }
    }
}
