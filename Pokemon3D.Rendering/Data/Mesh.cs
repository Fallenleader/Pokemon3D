using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Data
{
    /// <summary>
    /// Holding Geometry Data uploaded to the GPU.
    /// </summary>
    public class Mesh
    {
        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;

        public int VertexCount { get; }
        public int IndexCount { get; }
        public GeometryData GeometryData { get; }

        public Mesh(GraphicsDevice device, GeometryData data)
        {
            GeometryData = data;
            VertexCount = data.Vertices.Length;
            IndexCount = data.Indices.Length;

            _vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, VertexCount, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, IndexCount, BufferUsage.WriteOnly);

            _vertexBuffer.SetData(data.Vertices);
            _indexBuffer.SetData(data.Indices);
        }

        public void Draw()
        {
            var device = _vertexBuffer.GraphicsDevice;
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,0, VertexCount, 0, IndexCount / 3);
        }

        internal Mesh Clone()
        {
            var geometryData = new GeometryData
            {
                Vertices = new VertexPositionNormalTexture[VertexCount],
                Indices = new ushort[IndexCount],
            };
            
            _vertexBuffer.GetData(geometryData.Vertices);
            _indexBuffer.GetData(geometryData.Indices);

            return new Mesh(_vertexBuffer.GraphicsDevice, geometryData);
        }
    }
}
