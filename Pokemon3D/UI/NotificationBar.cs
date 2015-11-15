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
        private const int IconSize = 16;

        private readonly int _width;
        private readonly Color _backgroundColor;
        private readonly SpriteFont _spriteFont;
        private readonly float _notificationTime;
        private readonly List<NotificationItem> _notifications = new List<NotificationItem>();
        private readonly Texture2D _notificationIcons;

        private readonly Dictionary<NotificationKind, Rectangle> _notificationRectangle = new Dictionary<NotificationKind, Rectangle>
        {
            { NotificationKind.Error, new Rectangle(0,0,IconSize, IconSize) },
            { NotificationKind.Information, new Rectangle(0,IconSize,IconSize, IconSize) },
            { NotificationKind.Warning, new Rectangle(IconSize,0,IconSize, IconSize) }
        };
        
        public NotificationBar(int barWidth)
        {
            _width = barWidth;
            _notificationTime = 2.0f;
            _spriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NotificationFont);
            _backgroundColor = new Color(70, 70, 70);
            _notificationIcons = Game.Content.Load<Texture2D>(ResourceNames.Textures.NotificationIcons);
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

                var currentX = startX + ElementMargin;
                var sourceRectangle = _notificationRectangle[notification.NotificationKind];
                var position = new Vector2(currentX, startY + (elementHeight-IconSize)/2);
                Game.SpriteBatch.Draw(_notificationIcons, position, sourceRectangle, Color.White);

                currentX += IconSize + ElementMargin;
                Game.SpriteBatch.DrawString(_spriteFont, notification.Message, new Vector2(currentX, startY + ElementPadding), Color.White * notification.Alpha);

                startY -= ElementMargin;
            }
            Game.SpriteBatch.End();
        }
    }
}
