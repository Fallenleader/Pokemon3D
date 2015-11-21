using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    class ShadowCastRenderQueue : RenderQueue
    {
        private readonly BoundingFrustum _frustum = new BoundingFrustum(Matrix.Identity);

        public RenderTarget2D ShadowMap { get; set; }

        public ShadowCastRenderQueue(GameContext context, Action<Material> handleEffect, Func<IEnumerable<SceneNode>> getSceneNodes, SceneEffect sceneEffect) : base(context, handleEffect, getSceneNodes, sceneEffect)
        {
        }

        public override void Draw(Camera camera, Vector3 lightDirection, RenderStatistics renderStatistics)
        {
            var oldRenderTargets = GameContext.GraphicsDevice.GetRenderTargets();
            GameContext.GraphicsDevice.SetRenderTarget(ShadowMap);
            GameContext.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            var lightViewProjection = CalculateLightViewMatrix(lightDirection, camera);
            SceneEffect.LightWorldViewProjection = lightViewProjection;

            base.Draw(camera, lightDirection, renderStatistics);

            GameContext.GraphicsDevice.SetRenderTargets(oldRenderTargets);
        }

        private Matrix CalculateLightViewMatrix(Vector3 lightDirection, Camera camera)
        {
            var forward = Vector3.Normalize(lightDirection);
            var upVector = Vector3.Cross(forward, -Vector3.UnitX);
            var lightViewMatrix = Matrix.CreateLookAt(Vector3.Zero, forward, upVector);

            _frustum.Matrix = camera.ViewMatrix * camera.ProjectionMatrix;

            var boundingBox = BoundingBox.CreateFromPoints(_frustum.GetCorners().Select(f => Vector3.Transform(f, lightViewMatrix)));
            var width = boundingBox.Max.X - boundingBox.Min.X;
            var height = boundingBox.Max.Y - boundingBox.Min.Y;
            var depth = boundingBox.Max.Z - boundingBox.Min.Z;

            var cameraPositionTarget = camera.GlobalPosition + camera.Forward*(camera.FarClipDistance - camera.NearClipDistance)*0.5f;
            var cameraPosition = cameraPositionTarget - lightDirection * depth*0.5f;

            return Matrix.CreateLookAt(cameraPosition, cameraPositionTarget, Vector3.Up) * Matrix.CreateOrthographic(width *0.5f, height * 0.5f, 0.1f, depth);
        }

        protected override bool IsValidForRendering(Camera camera, DrawableElement element)
        {
            return element.IsActive && _frustum.Contains(element.BoundingBox) != ContainmentType.Disjoint;
        }
    }
}
