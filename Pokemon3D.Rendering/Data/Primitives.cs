using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Data
{
    public static class Primitives
    {
        public static GeometryData GenerateCubeData()
        {
            var data = new GeometryData
            {
                Vertices = new[]
                {
                    //front
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Backward,
                        new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Backward,
                        new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Backward,
                        new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Backward,
                        new Vector2(1.0f, 0.0f)),

                    //back
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Forward,
                        new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Forward,
                        new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Forward,
                        new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Forward,
                        new Vector2(1.0f, 0.0f)),

                    //right
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Right,
                        new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Right,
                        new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Right,
                        new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Right,
                        new Vector2(1.0f, 0.0f)),

                    //left
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Left,
                        new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Left,
                        new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Left,
                        new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Left,
                        new Vector2(1.0f, 0.0f)),

                    //top
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Up, new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Up, new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Up, new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Up, new Vector2(1.0f, 0.0f)),

                    //bottom
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Down,
                        new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Down,
                        new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Down,
                        new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Down,
                        new Vector2(1.0f, 0.0f)),
                },
                Indices = new ushort[36]
            };


            for(var i = 0; i < 6; i++)
            {
                data.Indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                data.Indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                data.Indices[i * 6 + 2] = (ushort)(i * 4 + 2);
                data.Indices[i * 6 + 3] = (ushort)(i * 4 + 1);
                data.Indices[i * 6 + 4] = (ushort)(i * 4 + 3);
                data.Indices[i * 6 + 5] = (ushort)(i * 4 + 2);
            }

            return data;
        }

        public static GeometryData GenerateQuadForYBillboard()
        {
            return new GeometryData
            {
                Vertices = new[]
                {
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.0f, 0.0f), Vector3.Backward,
                        new Vector2(0.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 1.0f, 0.0f), Vector3.Backward,
                        new Vector2(0.0f, 0.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.0f, 0.0f), Vector3.Backward,
                        new Vector2(1.0f, 1.0f)),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 1.0f, 0.0f), Vector3.Backward,
                        new Vector2(1.0f, 0.0f)),
                },
                Indices = new ushort[] {0, 1, 2, 1, 3, 2}
            };
        }

        public static GeometryData Merge(GeometryData[] geometryDatas)
        {
            var totalVertexCount = geometryDatas.Sum(g => g.Vertices.Length);
            var indicesCount = geometryDatas.Sum(g => g.Indices.Length);

            var mergedVertices = new List<VertexPositionNormalTexture>(totalVertexCount);
            var mergedIndices = new List<ushort>(indicesCount);

            var currentVertexIndex = 0;
            foreach(var geometryData in geometryDatas)
            {
                mergedVertices.AddRange(geometryData.Vertices);
                mergedIndices.AddRange(geometryData.Indices.Select(i => (ushort)(currentVertexIndex + i)));
                currentVertexIndex += geometryData.Vertices.Length;
            }

            return new GeometryData
            {
                Vertices = mergedVertices.ToArray(),
                Indices = mergedIndices.ToArray()
            };
        }

        public static GeometryData GenerateQuadXZ(Vector3? offset = null, ushort? startIndex = null, Vector2? texCoordStart = null, Vector2? texCoordScale = null)
        {
            var baseIndex = startIndex.GetValueOrDefault(0);
            var startTexCoord = texCoordStart.GetValueOrDefault(Vector2.Zero);
            var scaleTexCoord = texCoordScale.GetValueOrDefault(Vector2.One);
            var baseOffset = offset.GetValueOrDefault(Vector3.Zero);

            return new GeometryData
            {
                Vertices = new[]
                {
                    new VertexPositionNormalTexture(baseOffset + new Vector3(-0.5f, 0.0f, 0.5f), Vector3.Up,
                        startTexCoord + new Vector2(0.0f, 1.0f)*scaleTexCoord),
                    new VertexPositionNormalTexture(baseOffset + new Vector3(-0.5f, 0.0f, -0.5f), Vector3.Up,
                        startTexCoord + new Vector2(0.0f, 0.0f)*scaleTexCoord),
                    new VertexPositionNormalTexture(baseOffset + new Vector3(0.5f, 0.0f, 0.5f), Vector3.Up,
                        startTexCoord + new Vector2(1.0f, 1.0f)*scaleTexCoord),
                    new VertexPositionNormalTexture(baseOffset + new Vector3(0.5f, 0.0f, -0.5f), Vector3.Up,
                        startTexCoord + new Vector2(1.0f, 0.0f)*scaleTexCoord),
                },
                Indices = new[]
                {
                    (ushort) (baseIndex + 0),
                    (ushort) (baseIndex + 1),
                    (ushort) (baseIndex + 2),
                    (ushort) (baseIndex + 1),
                    (ushort) (baseIndex + 3),
                    (ushort) (baseIndex + 2)
                }
            };
        }
    }
}
