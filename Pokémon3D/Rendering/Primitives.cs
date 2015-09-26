using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pokémon3D.Rendering
{
    static class Primitives
    {
        public static GeometryData GenerateCubeData()
        {
            var data = new GeometryData();

            data.Vertices = new[]
            {
                //front
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f,  0.5f), Vector3.Backward, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.Backward, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.Backward, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.Backward, new Vector2(1.0f,0.0f)),

                //back
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f, -0.5f), Vector3.Forward, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f, -0.5f), Vector3.Forward, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Forward, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f, -0.5f), Vector3.Forward, new Vector2(1.0f,0.0f)),

                //right
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.Right, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.Right, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f, -0.5f), Vector3.Right, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f, -0.5f), Vector3.Right, new Vector2(1.0f,0.0f)),

                //left
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Left, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f, -0.5f), Vector3.Left, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f,  0.5f), Vector3.Left, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.Left, new Vector2(1.0f,0.0f)),

                //top
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.Up, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f, -0.5f), Vector3.Up, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.Up, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f, -0.5f), Vector3.Up, new Vector2(1.0f,0.0f)),

                //bottom
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Down, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f,  0.5f), Vector3.Down, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f, -0.5f), Vector3.Down, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.Down, new Vector2(1.0f,0.0f)),
            };

            data.Indices = new UInt16[36];
            for(var i = 0; i < 6; i++)
            {
                data.Indices[i * 6 + 0] = (UInt16)(i * 4 + 0);
                data.Indices[i * 6 + 1] = (UInt16)(i * 4 + 1);
                data.Indices[i * 6 + 2] = (UInt16)(i * 4 + 2);
                data.Indices[i * 6 + 3] = (UInt16)(i * 4 + 1);
                data.Indices[i * 6 + 4] = (UInt16)(i * 4 + 3);
                data.Indices[i * 6 + 5] = (UInt16)(i * 4 + 2);
            }

            return data;
        }

        public static GeometryData GenerateQuadForYBillboard()
        {
            var data = new GeometryData();

            data.Vertices = new[]
            {
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.0f,  0.0f), Vector3.Backward, new Vector2(0.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  1.0f,  0.0f), Vector3.Backward, new Vector2(0.0f,0.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.0f,  0.0f), Vector3.Backward, new Vector2(1.0f,1.0f)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  1.0f,  0.0f), Vector3.Backward, new Vector2(1.0f,0.0f)),
            };

            data.Indices = new ushort[] { 0, 1, 2, 1, 3, 2 };

            return data;
        }
    }
}
