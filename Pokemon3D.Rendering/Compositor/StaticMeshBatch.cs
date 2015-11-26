using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    class StaticMeshBatch : GameContextObject, DrawableElement, IDisposable
    {
        private readonly int _maxVertexCount;
        private readonly int _maxIndicesCount;
        private readonly List<VertexPositionNormalTexture> _vertices = new List<VertexPositionNormalTexture>(1000); 
        private readonly List<ushort> _indices = new List<ushort>(1000);

        public StaticMeshBatch(GameContext context, Material sharedMaterial) : base(context)
        { 
            Material = sharedMaterial.Clone();
            Material.TexcoordOffset = Vector2.Zero;
            Material.TexcoordScale = Vector2.One;

            _maxVertexCount = ushort.MaxValue;
            _maxIndicesCount = _maxVertexCount*3;
        }

        public bool AddBatch(SceneNode sceneNode)
        {
            var meshData = sceneNode.Mesh.GeometryData;
            if (_vertices.Count + meshData.Vertices.Length > _maxVertexCount) return false;
            if (_indices.Count + meshData.Indices.Length > _maxIndicesCount) return false;

            var world = sceneNode.GetWorldMatrix(null);

            
            for (var i = 0; i < meshData.Indices.Length; i++)
            {
                _indices.Add((ushort) (_vertices.Count + meshData.Indices[i]));
            }

            for (var i = 0; i < meshData.Vertices.Length; i++)
            {
                var currentVertex = meshData.Vertices[i];
                _vertices.Add(new VertexPositionNormalTexture(
                    Vector3.Transform(currentVertex.Position, world),
                    Vector3.TransformNormal(currentVertex.Normal, world),
                    currentVertex.TextureCoordinate*sceneNode.Material.TexcoordScale + sceneNode.Material.TexcoordOffset));
            }

            return true;
        }

        public void Build()
        {
            var geometryData = new GeometryData
            {
                Vertices = _vertices.ToArray(),
                Indices = _indices.ToArray()
            };

            Mesh = new Mesh(GameContext.GraphicsDevice, geometryData);

            BoundingBox = BoundingBox.CreateFromPoints(geometryData.Vertices.Select(v => v.Position));
        }

        public bool IsActive => true;
        public Mesh Mesh { get; private set; }
        public Material Material { get; }
        public Matrix GetWorldMatrix(Camera camera)
        {
            return Matrix.Identity;
        }

        public Vector3 GlobalPosition => Vector3.Zero;
        public BoundingBox BoundingBox { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StaticMeshBatch()
        {
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (Mesh != null)
                {
                    Mesh.Dispose();
                    Mesh = null;
                }
            }
            // free native resources if there are any.
        }
    }
}
