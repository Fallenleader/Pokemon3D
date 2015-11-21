using System.Linq;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering
{
    static class LightHelper
    {
        public static Matrix CalculateLightViewMatrix(Vector3 lightDirection, Camera camera)
        {
            var forward = Vector3.Normalize(lightDirection);
            var upVector = Vector3.Cross(forward, -Vector3.UnitX);
            var lightViewMatrix = Matrix.CreateLookAt(Vector3.Zero, forward, upVector);

            var boundingBox = BoundingBox.CreateFromPoints(camera.Frustum.GetCorners().Select(f => Vector3.Transform(f, lightViewMatrix)));
            var width = boundingBox.Max.X - boundingBox.Min.X;
            var height = boundingBox.Max.Y - boundingBox.Min.Y;
            var depth = boundingBox.Max.Z - boundingBox.Min.Z;

            var cameraPositionTarget = camera.GlobalPosition + camera.Forward * (camera.FarClipDistance - camera.NearClipDistance) * 0.5f;
            var cameraPosition = cameraPositionTarget - lightDirection * depth * 0.5f;

            return Matrix.CreateLookAt(cameraPosition, cameraPositionTarget, Vector3.Up) * Matrix.CreateOrthographic(width * 0.5f, height * 0.5f, 0.1f, depth);
        }
    }
}
