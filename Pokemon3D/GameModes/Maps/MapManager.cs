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

            // todo: when a map file is invalid, the loop stops at that file.
            _mapModels = Directory.GetFiles(_gameMode.MapPath)
                                         .Where(m => m.EndsWith(MapFileExtension, StringComparison.OrdinalIgnoreCase))
                                         .Select(m => JsonDataModel.FromFile<MapModel>(m)).ToArray();
        }

        public void Dispose()
        {
            // todo: dispose map manager resources
        }

        public Map LoadMap(string mapName, Scene scene, ResourceManager resourceManager)
        {
            var mapModel = _mapModels.Single(m => m.Name == mapName);
            return new Map(mapModel, scene, resourceManager);
        }
    }
}
