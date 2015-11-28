using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Data
{
    public static class Primitives
    {
        private static readonly Vector2[] DefaultOffsetAndScaleSides =  {
            new Vector2(0,0),new Vector2(1,1),
            new Vector2(0,0),new Vector2(1,1),
            new Vector2(0,0),new Vector2(1,1),
            new Vector2(0,0),new Vector2(1,1),
            new Vector2(0,0),new Vector2(1,1),
            new Vector2(0,0),new Vector2(1,1),
        };

        /// <summary>
        /// Generates Cube with optional texcoord offset.
        /// </summary>
        /// <param name="offsetAndScaleSides">Offset then scale for each side in order: front, back, right, left, top, bottom.</param>
        /// <returns>Cube</returns>
        public static GeometryData GenerateCubeData(Vector2[] offsetAndScaleSides = null)
        {
            var t = offsetAndScaleSides == null || offsetAndScaleSides.Length < 12
                ? DefaultOffsetAndScaleSides
                : offsetAndScaleSides;

            var data = new GeometryData
            {
                Vertices = new[]
                {
                    //front
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Backward,
                        t[0] + new Vector2(0.0f, 1.0f) * t[1]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Backward,
                        t[0] + new Vector2(0.0f, 0.0f) * t[1]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Backward,
                        t[0] + new Vector2(1.0f, 1.0f) * t[1]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Backward,
                        t[0] + new Vector2(1.0f, 0.0f) * t[1]),

                    //back
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Forward,
                        t[2] + new Vector2(0.0f, 1.0f) * t[3]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Forward,
                        t[2] + new Vector2(0.0f, 0.0f) * t[3]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Forward,
                        t[2] + new Vector2(1.0f, 1.0f) * t[3]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Forward,
                        t[2] + new Vector2(1.0f, 0.0f) * t[3]),

                    //right
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Right,
                        t[4] + new Vector2(0.0f, 1.0f) * t[5]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Right,
                        t[4] + new Vector2(0.0f, 0.0f) * t[5]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Right,
                        t[4] + new Vector2(1.0f, 1.0f) * t[5]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Right,
                        t[4] + new Vector2(1.0f, 0.0f) * t[5]),

                    //left
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Left,
                        t[6] + new Vector2(0.0f, 1.0f) * t[7]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Left,
                        t[6] + new Vector2(0.0f, 0.0f) * t[7]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Left,
                        t[6] + new Vector2(1.0f, 1.0f) * t[7]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Left,
                        t[6] + new Vector2(1.0f, 0.0f) * t[7]),

                    //top
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Up,
                        t[8] + new Vector2(0.0f, 1.0f) * t[9]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Up,
                        t[8] + new Vector2(0.0f, 0.0f) * t[9]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Up,
                        t[8] + new Vector2(1.0f, 1.0f) * t[9]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Up,
                        t[8] + new Vector2(1.0f, 0.0f) * t[9]),

                    //bottom
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Down,
                        t[10] + new Vector2(0.0f, 1.0f) * t[11]),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Down,
                        t[10] + new Vector2(0.0f, 0.0f) * t[11]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Down,
                        t[10] + new Vector2(1.0f, 1.0f) * t[11]),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Down,
                        t[10] + new Vector2(1.0f, 0.0f) * t[11]),
                },
                Indices = new ushort[36]
            };


            for (var i = 0; i < 6; i++)
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
    }
}
