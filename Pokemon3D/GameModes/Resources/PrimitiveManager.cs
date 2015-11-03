using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode.Definitions;
using Pokemon3D.Rendering.Data;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Pokemon3D.GameModes.Resources
{
    class PrimitiveManager : IDisposable
    {
        private GameMode _gameMode;
        private readonly Dictionary<string, PrimitiveModel> _primitiveModels;

        public PrimitiveManager(GameMode gameMode)
        {
            _gameMode = gameMode;

            // todo: the loop breaks when a primitive fails to load correctly.
            _primitiveModels = JsonDataModel.FromFile<PrimitiveModel[]>(Path.Combine(_gameMode.DataPath, GameMode.FILE_DATA_PRIMITIVES))
                .ToDictionary(pm => pm.Id, pm => pm);
        }
        
        public GeometryData GetPrimitiveData(string primitiveName)
        {
            PrimitiveModel primitiveModel;
            if (_primitiveModels.TryGetValue(primitiveName, out primitiveModel))
            {
                return new GeometryData
                {
                    Vertices = primitiveModel.Vertices.Select(v => new VertexPositionNormalTexture
                    {
                        Position = v.Position.GetVector3(),
                        TextureCoordinate = v.TexCoord.GetVector2(),
                        Normal = v.Normal.GetVector3()
                    }).ToArray(),
                    Indices = primitiveModel.Indices.Select(i => (ushort)i).ToArray()
                };
            }

            throw new ApplicationException("Invalid Primitive Type: " + primitiveName);
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // todo: free managed resources
            }

            // todo: free unmanaged resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Add, if this class has unmanaged resources
        //~PrimitiveManager()
        //{
        //    // Destructor calls dipose to free unmanaged resources:
        //    Dispose(false);
        //}

        #endregion
    }
}
