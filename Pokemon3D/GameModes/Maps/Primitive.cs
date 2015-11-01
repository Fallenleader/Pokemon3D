using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.DataModel.Json.GameMode.Definitions;

namespace Pokemon3D.GameModes.Maps
{
    /// <summary>
    /// A primitive, which describes the rendered geometry data.
    /// </summary>
    class Primitive
    {
        public Primitive(PrimitiveModel dataModel)
        {
            Name = dataModel.Name;

            Vertices = dataModel.Vertices.ToList()
                .Select(m => new VertexPositionNormalTexture(
                    m.Position.GetVector3(), 
                    m.Normal.GetVector3(),
                    m.TexCoord.GetVector2()))
                    .ToArray();

            Indices = dataModel.Indices.Select(i => (ushort)i).ToArray();
        }
        
        public string Name { get; }

        public VertexPositionNormalTexture[] Vertices { get; }

        public ushort[] Indices { get; }
    }
}
