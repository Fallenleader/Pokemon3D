using System;

namespace Pokemon3D.Common.Animations
{
    public class ActionTimer
    {
        private readonly Action _action;
        private readonly float _triggerTime;
        private readonly bool _loop;
        private float _elapsedTime;

        public ActionTimer(Action action, float triggerTime, bool loop = false)
        {
            _action = action;
            _triggerTime = triggerTime;
            _loop = loop;
        }

        public bool IsRunning { get; private set; }
        public float TotalElapsedTime { get; private set; }

        public void Start()
        {
            IsRunning = true;
            _elapsedTime = 0;
            TotalElapsedTime = 0;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Update(float elapsed)
        {
            if (!IsRunning) return;
            _elapsedTime += elapsed;
            TotalElapsedTime += elapsed;
            if (_elapsedTime >= _triggerTime)
            {
                _action();
                if (_loop)
                {
                    _elapsedTime -= _triggerTime;
                }
                else
                {
                    Stop();
                }
                    
            }
        }
    }
}
