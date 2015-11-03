using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode;
using Pokemon3D.DataModel.Json.GameMode.Definitions;
using Pokemon3D.DataModel.Json.GameMode.Map;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes.Maps;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// The main class to control a GameMode.
    /// </summary>
    partial class GameMode : IDataModelContainer, IDisposable, GameModeDataProvider
    {
        private readonly GameModeModel _dataModel;
        private readonly string _gameModeFolder;
        private readonly bool _isValid;

        /// <summary>
        /// Returns if the container loaded the data correctly.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        public GameMode(string gameModeFile)
        {
            try
            {
                _gameModeFolder = Path.GetDirectoryName(gameModeFile);
                _dataModel = JsonDataModel.FromFile<GameModeModel>(gameModeFile);

                PrimitiveManager = new Resources.PrimitiveManager(this);
                MapManager = new MapManager(this);

                _isValid = true;
            }
            catch (JsonDataLoadException ex)
            {
                //Something went wrong processing the data from a GameMode config file.
                //Log the error and mark the instance as invalid.

                GameLogger.Instance.Log(MessageType.Error, "An error occurred trying to load the GameMode config file \"" + gameModeFile + "\".");

                _isValid = false;
            }
        }

        public string StartMap => _dataModel.StartConfiguration.Map;

        public MapManager MapManager { get; private set; }
        public Resources.PrimitiveManager PrimitiveManager { get; private set; }
        
        public GeometryData GetPrimitiveData(string primitiveName)
        {
            return PrimitiveManager.GetPrimitiveData(primitiveName);
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                MapManager.Dispose();
                PrimitiveManager.Dispose();
            }

            // todo: free unmanaged resources.
        }

        /// <summary>
        /// Frees all resources consumed by this GameMode.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Add, if this class has unmanaged resources
        //~GameMode()
        //{
        //    Dispose(false);
        //}

        #endregion
    }
}
