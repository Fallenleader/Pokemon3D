using System;
using System.Collections.Generic;

namespace Pokemon3D.Common
{
    /// <summary>
    /// Animator-class for handling animations and animation series.
    /// </summary>
    public class Animator
    {
        private readonly Dictionary<string, Animation> _animations;
        private readonly Dictionary<string, string> _transitions;
        private string _currentAnimationName;

        /// <summary>
        /// Gets called when an animation has finished.
        /// </summary>
        public event Action<string> AnimationFinished;

        /// <summary>
        /// Gets called when an animation has started.
        /// </summary>
        public event Action<string> AnimationStarted;

        /// <summary>
        /// Gets called when the last animation of animator has finished.
        /// </summary>
        public event Action AnimatorFinished; 
        
        /// <summary>
        /// Currently active animation.
        /// </summary>
        public Animation CurrentAnimation { get; private set; }

        /// <summary>
        /// Creates Animator.
        /// </summary>
        public Animator()
        {
            _animations = new Dictionary<string, Animation>();
            _transitions = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets current animation and starts it.
        /// </summary>
        /// <param name="name">Name of animation</param>
        public void SetAnimation(string name)
        {
            _currentAnimationName = name;
            CurrentAnimation = _animations[name];
            CurrentAnimation.Start();
            AnimationStarted?.Invoke(name);
        }

        /// <summary>
        /// Registeres new animation.
        /// </summary>
        /// <param name="name">name of animation</param>
        /// <param name="animation">Object</param>
        public void AddAnimation(string name, Animation animation)
        {
            _animations.Add(name, animation);
        }

        /// <summary>
        /// Registers transition between animation
        /// </summary>
        /// <param name="sourceName">Source Animation Name</param>
        /// <param name="targetName">Target Animation Name</param>
        public void AddTransition(string sourceName, string targetName)
        {
            _transitions.Add(sourceName, targetName);
        }

        /// <summary>
        /// Updates Animator.
        /// </summary>
        /// <param name="elapsedSeconds">elapsed time seconds since last call.</param>
        public void Update(float elapsedSeconds)
        {
            if (CurrentAnimation == null) return;

            CurrentAnimation.Update(elapsedSeconds);
            if (CurrentAnimation.IsFinished)
            {
                AnimationFinished?.Invoke(_currentAnimationName);

                string newAnimation;
                if (_transitions.TryGetValue(_currentAnimationName, out newAnimation))
                {
                    SetAnimation(newAnimation);
                }
                else
                {
                    CurrentAnimation = null;
                    AnimatorFinished?.Invoke();
                }
            }
        }
    }
}
