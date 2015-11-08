using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Specialized Scene Node representing a camera.
    /// </summary>
    public class Camera : SceneNode
    {
        public float NearClipDistance { get; set; }
        public float FarClipDistance { get; set; }
        public float FieldOfView { get; set; }
        public Viewport Viewport { get; set; }

        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        
        internal Camera(Viewport viewport)
        {
            Viewport = viewport;
            NearClipDistance = 1.0f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
        }

        protected override void HandleIsDirty()
        {
            base.HandleIsDirty();
            ViewMatrix = Matrix.Invert(GetWorldMatrix(null));
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, Viewport.AspectRatio, NearClipDistance, FarClipDistance);
        }
    }
}
