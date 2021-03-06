﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor
{
    public interface SceneRenderer
    {
        Vector3 LightDirection { get; set; }
        Vector4 AmbientLight { get; set; }
        bool EnableShadows { get; set; }
        bool EnablePostProcessing { get; set; }

        void AddPostProcessingStep(PostProcessingStep step);
        void Draw(bool hasSceneNodesChanged, IList<SceneNode> allNodes, IList<Camera> cameras);

        void DrawDebugShadowMap(SpriteBatch spriteBatch, Rectangle target);
    }
}
