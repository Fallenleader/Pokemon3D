using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel.Json.GameMode.Map;
using System;
using System.IO;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.DataModel.Json;

namespace Pokemon3D.GameModes.Maps
{
    class MapManager : IDisposable
    {
        private const string MapFileExtension = ".json";
        private const string MapFragmentFileExtension = ".json";

        private GameMode _gameMode;
        private MapModel[] _mapModels;
        private MapFragmentModel[] _mapFragments;

        public MapManager(GameMode gameMode)
        {
            _gameMode = gameMode;

            LoadMaps();
            LoadMapFragments();
        }

        private void LoadMaps()
        {
            List<MapModel> mapModels = new List<MapModel>();

            var files = Directory.GetFiles(_gameMode.MapPath, "*.*", SearchOption.AllDirectories)
                .Where(m => m.EndsWith(MapFileExtension, StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                try
                {
                    mapModels.Add(JsonDataModel<MapModel>.FromFile(file));
                }
                catch (JsonDataLoadException ex)
                {
                    GameLogger.Instance.Log(ex);
                }
            }

            _mapModels = mapModels.ToArray();
        }

        private void LoadMapFragments()
        {
            List<MapFragmentModel> mapFragments = new List<MapFragmentModel>();

            var files = Directory.GetFiles(_gameMode.FragmentsPath, "*.*", SearchOption.AllDirectories)
                .Where(m => m.EndsWith(MapFragmentFileExtension, StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                try
                {
                    mapFragments.Add(JsonDataModel<MapFragmentModel>.FromFile(file));
                }
                catch (JsonDataLoadException)
                {
                    // todo: log exception.
                }
            }

            _mapFragments = mapFragments.ToArray();
        }

        public Map GetMap(string mapId, Scene scene, ResourceManager resourceManager)
        {
            var mapModel = _mapModels.Single(m => m.Id == mapId);
            return new Map(_gameMode, mapModel, scene, resourceManager);
        }

        public MapFragmentModel GetMapFragment(string fragmentId)
        {
            return _mapFragments.Single(m => m.Id == fragmentId);
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
