using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    internal class Light
    {
        public Vector3 Direction { get; set; }

        public RenderTarget2D ShadowMap { get; private set; }
        public Matrix LightViewMatrix { get; private set; }

        public Light(GraphicsDevice device, int size)
        {
            ShadowMap = new RenderTarget2D(device, size, size, false, SurfaceFormat.Single, DepthFormat.Depth24);
        }

        public void UpdateLightViewMatrixForCamera(Camera camera, IList<SceneNode> shadowCasters)
        {
            var forward = Vector3.Normalize(Direction);
            var lightViewMatrix = Matrix.CreateLookAt(Vector3.Zero, forward, Vector3.Up);

            var mergedBox = new BoundingBox();
            for (var i = 0; i < shadowCasters.Count; i++)
            {
                var drawableElement = shadowCasters[i];
                if (camera.Frustum.Contains(drawableElement.BoundingBox) == ContainmentType.Disjoint) continue;
                mergedBox = BoundingBox.CreateMerged(mergedBox, drawableElement.BoundingBox);
            }
            var sphere = BoundingSphere.CreateFromBoundingBox(mergedBox);

            var edges = new Vector3[8];
            edges[0] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Min.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[1] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Min.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[2] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Min.Y, mergedBox.Max.Z), lightViewMatrix);
            edges[3] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Min.Y, mergedBox.Max.Z), lightViewMatrix);
            edges[4] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Max.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[5] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Max.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[6] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Max.Y, mergedBox.Max.Z), lightViewMatrix);
            edges[7] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Max.Y, mergedBox.Max.Z), lightViewMatrix);

            var boundingBox = BoundingBox.CreateFromPoints(edges);
            var width = boundingBox.Max.X - boundingBox.Min.X;
            var height = boundingBox.Max.Y - boundingBox.Min.Y;
            var depth = boundingBox.Max.Z - boundingBox.Min.Z;

            var cameraPosition = sphere.Center - Direction*sphere.Radius;
            var cameraPositionTarget = cameraPosition + Direction;
                
            LightViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPositionTarget, Vector3.Up) * Matrix.CreateOrthographic(width *1.2f, height*1.2f, 0.1f, depth*2.0f);
        }
    }
}
