using System;

namespace Pokemon3D.Common.Animations
{
    public class DiscreteAnimation<T> : Animation
    {
        private readonly Action<T> _onUpdate;
        private readonly T[] _animationSteps;
        private readonly float _stepTime;
        
        public DiscreteAnimation(float durationSeconds, T[] animationSteps, Action<T> onUpdate, bool loops) : base(durationSeconds, loops)
        {
            _onUpdate = onUpdate;
            _animationSteps = animationSteps;
            _stepTime = durationSeconds/animationSteps.Length;
        }

        protected override void OnUpdate()
        {
            var currentAnimationStep = (int)(ElapsedSeconds / _stepTime);
            if (currentAnimationStep >= _animationSteps.Length) currentAnimationStep = 0;
            _onUpdate?.Invoke(_animationSteps[currentAnimationStep]);
        }
    }
}
