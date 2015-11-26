using System.Diagnostics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor
{
    public class RenderStatistics : Singleton<RenderStatistics>
    {
        private Stopwatch _frameStopwatch;

        public int DrawCalls { get; set; }
        public float AverageDrawTime { get; set; }

        private RenderStatistics()
        {
            
        }

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
