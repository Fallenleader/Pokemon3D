using System;

namespace Pokemon3D.Common
{
    /// <summary>
    /// Basic class for all animation kinds.
    /// </summary>
    public abstract class Animation
    {
        /// <summary>
        /// Creates a new animation.
        /// </summary>
        /// <param name="durationSeconds">Time of animation.</param>
        protected Animation(float durationSeconds)
        {
            DurationSeconds = durationSeconds;
        }

        /// <summary>
        /// Time for the animation.
        /// </summary>
        public float DurationSeconds { get; }

        /// <summary>
        /// Elapsed Seconds since start of animation.
        /// </summary>
        public float ElapsedSeconds { get; private set; }

        /// <summary>
        /// Starts the current animation.
        /// </summary>
        public void Start()
        {
            IsFinished = false;
            ElapsedSeconds = 0.0f;
        }

        /// <summary>
        /// Updates the current animation.
        /// </summary>
        /// <param name="elapsedSeconds">elapsed time in seconds since last call.</param>
        public void Update(float elapsedSeconds)
        {
            if (IsFinished) return;
            ElapsedSeconds += elapsedSeconds;
            IsFinished = ElapsedSeconds >= DurationSeconds;
            OnUpdate();
        }

        protected abstract void OnUpdate();

        /// <summary>
        /// Whether the animation is finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Creates an Animation interpolating from 0 to 1 over time.
        /// </summary>
        /// <param name="durationSeconds">Animation duration</param>
        /// <param name="onUpdate">called when updating</param>
        /// <returns>Animation to add to animator</returns>
        public static Animation CreateDelta(float durationSeconds, Action<float> onUpdate)
        {
            return new DeltaAnimation(durationSeconds, onUpdate);
        }
    }
}