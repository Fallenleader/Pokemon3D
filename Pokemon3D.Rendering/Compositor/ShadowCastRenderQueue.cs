using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    class ShadowCastRenderQueue : RenderQueue
    {
        public ShadowCastRenderQueue(GameContext context, Action<Material> handleEffect, Func<IEnumerable<SceneNode>> getSceneNodes, SceneEffect sceneEffect) : base(context, handleEffect, getSceneNodes, sceneEffect)
        {
        }

        public override void Draw(Camera camera, Light light, RenderStatistics renderStatistics, bool hasSceneNodesChanged)
        {
            var oldRenderTargets = GameContext.GraphicsDevice.GetRenderTargets();
            GameContext.GraphicsDevice.SetRenderTarget(light.ShadowMap);
            GameContext.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            base.Draw(camera, light, renderStatistics, hasSceneNodesChanged);

            GameContext.GraphicsDevice.SetRenderTargets(oldRenderTargets);
        }

        protected override bool IsValidForRendering(Camera camera, DrawableElement element)
        {
            return element.IsActive && camera.Frustum.Contains(element.BoundingBox) != ContainmentType.Disjoint;
        }
    }
}
