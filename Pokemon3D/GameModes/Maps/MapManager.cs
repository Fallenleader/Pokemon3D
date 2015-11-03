using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel.Json.GameMode.Map;
using System;
using System.IO;
using Pokemon3D.DataModel.Json;

namespace Pokemon3D.GameModes.Maps
{
    class MapManager : IDisposable
    {
        private const string MapFileExtension = ".json";

        private GameMode _gameMode;
        private readonly MapModel[] _mapModels;

        public MapManager(GameMode gameMode)
        {
            _gameMode = gameMode;

            List<MapModel> mapModels = new List<MapModel>();

            var files = Directory.GetFiles(_gameMode.MapPath)
                .Where(m => m.EndsWith(MapFileExtension, StringComparison.OrdinalIgnoreCase));
            
            foreach (var file in files)
            {
                try
                {
                    mapModels.Add(JsonDataModel.FromFile<MapModel>(file));
                }
                catch (JsonDataLoadException ex)
                {
                    // todo: log exception.
                }
            }

            _mapModels = mapModels.ToArray();
        }

        public Map LoadMap(string mapName, Scene scene, ResourceManager resourceManager)
        {
            var mapModel = _mapModels.Single(m => m.Name == mapName);
            return new Map(mapModel, scene, resourceManager);
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
        //~MapManager()
        //{
        //    // Destructor calls dipose to free unmanaged resources:
        //    Dispose(false);
        //}

        #endregion
    }
}
