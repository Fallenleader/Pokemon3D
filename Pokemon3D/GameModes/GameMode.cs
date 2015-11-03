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
        public GameModeInfo GameModeInfo { get; }

        public MapManager MapManager { get; private set; }
        public Resources.PrimitiveManager PrimitiveManager { get; private set; }
        
        public bool IsValid { get; private set; }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        public GameMode(GameModeInfo gameModeInfo)
        {
            GameModeInfo = gameModeInfo;

            // only continue if the game mode config file loaded correctly.
            if (GameModeInfo.IsValid)
            {
                PrimitiveManager = new Resources.PrimitiveManager(this);
                MapManager = new MapManager(this);
            }

            IsValid = true;
        }
        
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
