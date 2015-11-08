using System.Diagnostics;

namespace Pokemon3D.Rendering.Compositor
{
    public class RenderStatistics
    {
        private Stopwatch _frameStopwatch;

        public int DrawCalls => TransparentObjectDrawCalls + SolidObjectDrawCalls + ShadowCasterDrawCalls;
        public int TransparentObjectDrawCalls { get; set; }
        public int SolidObjectDrawCalls { get; set; }
        public int ShadowCasterDrawCalls { get; set; }
        public float AverageDrawTime { get; set; }

        internal void StartFrame()
        {
            _frameStopwatch = Stopwatch.StartNew();
            TransparentObjectDrawCalls = 0;
            SolidObjectDrawCalls = 0;
            ShadowCasterDrawCalls = 0;
        }

        internal void EndFrame()
        {
            _frameStopwatch.Stop();
            AverageDrawTime = (float) ((AverageDrawTime + _frameStopwatch.Elapsed.TotalMilliseconds)*0.5f);
        }
    }
}
