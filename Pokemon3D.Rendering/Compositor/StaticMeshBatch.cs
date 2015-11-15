using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    class StaticMeshBatch : GameContextObject, DrawableElement
    {
        private readonly List<VertexPositionNormalTexture> _vertices = new List<VertexPositionNormalTexture>(1000); 
        private readonly List<ushort> _indices = new List<ushort>(1000);

        public StaticMeshBatch(GameContext context, Material sharedMaterial) : base(context)
        { 
            Material = sharedMaterial.Clone();
            Material.TexcoordOffset = Vector2.Zero;
            Material.TexcoordScale = Vector2.One;
        }

        public void AddBatch(SceneNode sceneNode)
        {
            var world = sceneNode.GetWorldMatrix(null);

            var meshData = sceneNode.Mesh.GeometryData;
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
        }

        public void Build()
        {
            var geometryData = new GeometryData
            {
                Vertices = _vertices.ToArray(),
                Indices = _indices.ToArray()
            };

            Mesh = new Mesh(GameContext.GraphicsDevice, geometryData);
        }

        public Mesh Mesh { get; private set; }
        public Material Material { get; }
        public Matrix GetWorldMatrix(Camera camera)
        {
            return Matrix.Identity;
        }
    }
}
