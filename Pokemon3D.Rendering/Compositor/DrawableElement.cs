using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    interface DrawableElement
    {
        Mesh Mesh { get; }
        Material Material { get; }
        Matrix GetWorldMatrix(Camera camera);
        Vector3 GlobalPosition { get; }
        BoundingBox BoundingBox { get; }
    }
}
