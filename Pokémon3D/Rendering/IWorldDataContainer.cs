using Microsoft.Xna.Framework;

namespace Pokémon3D.Rendering
{
    /// <summary>
    /// An interface to access world matrix related data: Position, Scale and Rotation.
    /// </summary>
    interface IWorldDataContainer
    {
        Vector3 Position { get; set; }

        Vector3 Scale { get; set; }

        Quaternion Rotation { get; set; }
    }
}
