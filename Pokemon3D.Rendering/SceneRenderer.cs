using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    internal interface SceneRenderer
    {
        void Draw(IList<SceneNode> allNodes, IList<Camera> cameras, Vector3 lightDirection, bool enableShadows);

        void DrawDebugShadowMap(SpriteBatch spriteBatch, Rectangle target);
    }
}
