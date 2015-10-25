using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

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
        /// <param name="gameContext">Game Context</param>
        /// <param name="source">Source Input</param>
        /// <param name="target">Rendered Output.</param>
        void Process(GameContext gameContext, Texture2D source, RenderTarget2D target);

        /// <summary>
        /// Initialize and Bind Effect data.
        /// </summary>
        /// <param name="contextEffect">Effect holding data.</param>
        void Initialize(Effect contextEffect);
    }
}
