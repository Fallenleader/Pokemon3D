using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor
{
    /// <summary>
    /// Post Processing step for rendering actions after everthing done.
    /// </summary>
    public interface PostProcessingStep
    {
        /// <summary>
        /// Step is Enabled or not.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Render PostProcess to target texture.
        /// </summary>
        /// <param name="source">Source Input</param>
        /// <param name="target">Rendered Output.</param>
        void Process(Texture2D source, RenderTarget2D target);
    }
}
