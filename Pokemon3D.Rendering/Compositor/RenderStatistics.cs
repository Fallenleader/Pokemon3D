using System.Diagnostics;

namespace Pokemon3D.Rendering.Compositor
{
    public class RenderStatistics
    {
        private Stopwatch _frameStopwatch;

        public int DrawCalls { get; set; }
        public float AverageDrawTime { get; set; }

        internal void StartFrame()
        {
            _frameStopwatch = Stopwatch.StartNew();
            DrawCalls = 0;
        }

        internal void EndFrame()
        {
            _frameStopwatch.Stop();
            AverageDrawTime = (float) ((AverageDrawTime + _frameStopwatch.Elapsed.TotalMilliseconds)*0.5f);
        }
    }
}
