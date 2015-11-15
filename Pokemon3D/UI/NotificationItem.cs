using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Animations;

namespace Pokemon3D.UI
{
    class NotificationItem
    {
        private Animator _animator;

        public NotificationItem(float lifeTime, NotificationKind notificationKind, string message)
        {
            NotificationKind = notificationKind;
            Message = message;

            _animator = new Animator();
            _animator.AnimatorFinished += () => IsFinished = true;

            var transit = lifeTime*0.125f;

            _animator.AddAnimation("TransitIn", Animation.CreateDelta(transit, d => Alpha = MathHelper.SmoothStep(0.0f, 1.0f, d)));
            _animator.AddAnimation("Visible", Animation.CreateWait(lifeTime - 2*transit));
            _animator.AddAnimation("TransitOut", Animation.CreateDelta(transit, d => Alpha = MathHelper.SmoothStep(1.0f, 0.0f, d)));
            _animator.AddTransitionChain("TransitIn", "Visible", "TransitOut");
            _animator.SetAnimation("TransitIn");
        }

        public NotificationKind NotificationKind { get; }
        public string Message { get; }

        public void Update(float elapsedTime)
        {
            _animator.Update(elapsedTime);
        }

        public bool IsFinished { get; private set; }
        public float Alpha { get; private set; }
    }
}
