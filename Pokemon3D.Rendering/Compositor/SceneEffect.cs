using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor
{
    /// <summary>
    /// Concrete Implementation for Effects used by the scene.
    /// This is abstracted because of different platform implementations
    /// and because of loading the effects from the game itself.
    /// </summary>
    public interface SceneEffect
    {
        /// <summary>
        /// Effect for drawing shadow map on a spritebatch. For Debugging purposes.
        /// </summary>
        Effect ShadowMapDebugEffect { get; }

        /// <summary>
        /// Activates a technique for drawing shadow depth map objects.
        /// </summary>
        void ActivateShadowDepthMapPass();

        /// <summary>
        /// Sets Technique for rendering billboards.
        /// </summary>
        void ActivateBillboardingTechnique();

        /// <summary>
        /// Activate Lighting technique, optional with shadows.
        /// </summary>
        /// <param name="withShadows"></param>
        void ActivateLightingTechnique(bool withShadows);

        /// <summary>
        /// Light Matrix for Shadow Map.
        /// </summary>
        Matrix LightWorldViewProjection { get; set; }

        /// <summary>
        /// Passes of current activated technique.
        /// </summary>
        EffectPassCollection CurrentTechniquePasses { get; } 

        /// <summary>
        /// World Matrix for normal mesh rendering with lighting.
        /// </summary>
        Matrix World { get; set; }

        /// <summary>
        /// Camera Matrix for normal mesh rendering with lighting.
        /// </summary>
        Matrix View { get; set; }

        /// <summary>
        /// Projection Matrix for normal mesh rendering with lighting.
        /// </summary>
        Matrix Projection { get; set; }

        /// <summary>
        /// Light Direction for directional light.
        /// </summary>
        Vector3 LightDirection { get; set; }

        /// <summary>
        /// Shadow Map for Rendering shadowed objects.
        /// </summary>
        Texture2D ShadowMap { get; set; }

        /// <summary>
        /// Texture for rendering.
        /// </summary>
        Texture2D DiffuseTexture { get; set; }

        /// <summary>
        /// Post-Processing Effects Container.
        /// </summary>
        Effect PostProcessingEffect { get; }

        /// <summary>
        /// Offset of Tex-Coords.
        /// </summary>
        Vector2 TexcoordOffset { get; set; }

        /// <summary>
        /// Texcoord Scales.
        /// </summary>
        Vector2 TexcoordScale { get; set; }
    }
}