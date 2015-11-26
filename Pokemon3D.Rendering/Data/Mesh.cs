using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.Rendering.Data
{
    /// <summary>
    /// Holding Geometry Data uploaded to the GPU.
    /// </summary>
    public class Mesh : IDisposable
    {
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        public int VertexCount { get; }
        public int IndexCount { get; }
        public GeometryData GeometryData { get; }
        public BoundingBox LocalBounds { get; private set; }

        public Mesh(GraphicsDevice device, GeometryData data)
        {
            GeometryData = data;
            VertexCount = data.Vertices.Length;
            IndexCount = data.Indices.Length;

            _vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, VertexCount, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, IndexCount, BufferUsage.WriteOnly);

            _vertexBuffer.SetData(data.Vertices);
            _indexBuffer.SetData(data.Indices);

            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max  = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var vertex in GeometryData.Vertices)
            {
                min.X = MathHelper.Min(min.X, vertex.Position.X);
                min.Y = MathHelper.Min(min.Y, vertex.Position.Y);
                min.Z = MathHelper.Min(min.Z, vertex.Position.Z);

                max.X = MathHelper.Max(max.X, vertex.Position.X);
                max.Y = MathHelper.Max(max.Y, vertex.Position.Y);
                max.Z = MathHelper.Max(max.Z, vertex.Position.Z);
            }
            LocalBounds = new BoundingBox(min, max);
        }

        public void Draw()
        {
            var device = _vertexBuffer.GraphicsDevice;
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,0, VertexCount, 0, IndexCount / 3);
            RenderStatistics.Instance.DrawCalls++;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Mesh()
        {
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_vertexBuffer != null)
                {
                    _vertexBuffer.Dispose();
                    _vertexBuffer = null;
                }
                if (_indexBuffer != null)
                {
                    _indexBuffer.Dispose();
                    _indexBuffer = null;
                }
            }
            // free native resources if there are any.
        }
    }
}
