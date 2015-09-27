using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Holding Geometry Data uploaded to the GPU.
    /// </summary>
    public class Mesh
    {
        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;

        public Mesh(GraphicsDevice device, GeometryData data)
        {
            _vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, data.Vertices.Length, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, data.Indices.Length, BufferUsage.WriteOnly);

            _vertexBuffer.SetData(data.Vertices);
            _indexBuffer.SetData(data.Indices);
        }

        public void Draw()
        {
            var device = _vertexBuffer.GraphicsDevice;
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            var primitiveCount = _indexBuffer.IndexCount / 3;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,0,_vertexBuffer.VertexCount, 0, primitiveCount);
        }
    }
}
