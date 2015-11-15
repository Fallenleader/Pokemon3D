using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    class StaticMeshBatch : DrawableElement
    {
        public StaticMeshBatch(GraphicsDevice device, Material sharedMaterial, IList<SceneNode> sceneNodes)
        {
            Material = sharedMaterial.Clone();
            Material.TexcoordOffset = Vector2.Zero;
            Material.TexcoordScale = Vector2.One;

            var geometryData = new GeometryData
            {
                Vertices =  new VertexPositionNormalTexture[sceneNodes.Sum(s => s.Mesh.VertexCount)],
                Indices = new ushort[sceneNodes.Sum(s => s.Mesh.IndexCount)]
            };

            var baseVertexIndex = 0;
            var baseIndicesIndex = 0;
            foreach (var sceneNode in sceneNodes)
            {
                //todo: what about billboards?
                var world = sceneNode.GetWorldMatrix(null);

                var meshData = sceneNode.Mesh.GeometryData;
                for (var i = 0; i < meshData.Indices.Length; i++)
                {
                    geometryData.Indices[baseIndicesIndex+i] = (ushort) (baseVertexIndex + meshData.Indices[i]);
                }
                baseIndicesIndex += (ushort)meshData.Indices.Length;

                for (var i = 0; i < meshData.Vertices.Length; i++)
                {
                    var currentVertex = meshData.Vertices[i];
                    geometryData.Vertices[baseVertexIndex+i] = new VertexPositionNormalTexture(
                        Vector3.Transform(currentVertex.Position, world),
                        Vector3.TransformNormal(currentVertex.Normal, world),
                        currentVertex.TextureCoordinate * sceneNode.Material.TexcoordScale + sceneNode.Material.TexcoordOffset);
                }
                baseVertexIndex += meshData.Vertices.Length;
                baseIndicesIndex += meshData.Indices.Length;
            }
            
            Mesh = new Mesh(device, geometryData);
        }

        public Mesh Mesh { get; }
        public Material Material { get; }
    }
}
