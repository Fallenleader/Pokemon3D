using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.Rendering
{
    class Camera : SceneNode
    {
        public float NearClipDistance { get; set; }
        public float FarClipDistance { get; set; }
        public float FieldOfView { get; set; }
        public Viewport Viewport { get; set; }

        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        public Camera(Viewport viewport)
        {
            Viewport = viewport;
            NearClipDistance = 1.0f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
        }

        public override void Update()
        {
            base.Update();
            ViewMatrix = Matrix.Invert(World);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, Viewport.AspectRatio, NearClipDistance, FarClipDistance);
        }
    }
}
