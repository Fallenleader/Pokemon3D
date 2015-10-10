using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Holding geometry Data for Meshs in the RAM. Can be used for creating
    /// Mesh data independent from the source (file, generated from code, merged...).
    /// </summary>
    public class GeometryData
    {
        /// <summary>
        /// Vertex Data to manipulate or Upload to Mesh.
        /// </summary>
        public VertexPositionNormalTexture[] Vertices;

        /// <summary>
        /// Indices to manipulate or Upload to Mesh.
        /// </summary>
        public ushort[] Indices;
    }
}