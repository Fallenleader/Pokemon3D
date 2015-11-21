using System.Linq;
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

        public void UpdateLightViewMatrixForCamera(Camera camera)
        {
            var forward = Vector3.Normalize(Direction);
            var upVector = Vector3.Cross(forward, -Vector3.UnitX);
            var lightViewMatrix = Matrix.CreateLookAt(Vector3.Zero, forward, upVector);

            var boundingBox = BoundingBox.CreateFromPoints(camera.Frustum.GetCorners().Select(f => Vector3.Transform(f, lightViewMatrix)));
            var width = boundingBox.Max.X - boundingBox.Min.X;
            var height = boundingBox.Max.Y - boundingBox.Min.Y;
            var depth = boundingBox.Max.Z - boundingBox.Min.Z;

            var cameraPositionTarget = camera.GlobalPosition + camera.Forward * (camera.FarClipDistance - camera.NearClipDistance) * 0.5f;
            var cameraPosition = cameraPositionTarget - Direction * depth * 0.5f;

            LightViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPositionTarget, Vector3.Up) * Matrix.CreateOrthographic(width * 0.5f, height * 0.5f, 0.1f, depth);
        }
    }
}
