using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameCore;
using Pokemon3D.FileSystem;
using System.IO;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Common.FileSystem;
using System;

namespace Pokemon3D.GameCore
{
    /// <summary>
    /// Contains the global settings of the game, loaded from the configuration.json file.
    /// </summary>
    class GameConfiguration : GameObject
    {
        private ConfigurationModel _dataModel;

        public event EventHandler ConfigFileLoaded;

        public string DisplayLanguage => _dataModel.DisplayLanguage;

        public SizeModel WindowSize
        {
            get
            {
                if (_dataModel.WindowSize == null)
                {
                    _dataModel.WindowSize = new SizeModel()
                    {
                        Width = 1024,
                        Height = 600
                    };
                }
                return _dataModel.WindowSize;
            }
        }
        
        public GameConfiguration()
        {
            if (File.Exists(StaticFileProvider.ConfigFile))
            {
                try
                {
                    _dataModel = JsonDataModel<ConfigurationModel>.FromFile(StaticFileProvider.ConfigFile);
                }
                catch (JsonDataLoadException)
                {
                    GameLogger.Instance.Log(MessageType.Error, "Error trying to load the configuration file of the game. Resetting file.");

                    _dataModel = ConfigurationModel.Default;
                    Save();
                }
            }
            else
            {
                GameLogger.Instance.Log(MessageType.Warning, "Configuration file not found. Creating new one.");

                _dataModel = ConfigurationModel.Default;
                Save();
            }

            Game.Exiting += OnGameExiting;

            FileObserver.Instance.StartFileObserve(StaticFileProvider.ConfigFile, ReloadFile);
        }

        private void ReloadFile(object sender, FileSystemEventArgs e)
        {
            try
            {
                _dataModel = JsonDataModel<ConfigurationModel>.FromFile(StaticFileProvider.ConfigFile);
                ConfigFileLoaded(this, EventArgs.Empty);
            }
            catch (JsonDataLoadException)
            {
                GameLogger.Instance.Log(MessageType.Error, "Error trying to load the configuration file of the game.");
            }
            catch (IOException)
            {
                GameLogger.Instance.Log(MessageType.Error, "Failed to access the contents of the file.");
            }
        }

        private void OnGameExiting(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {
            GameLogger.Instance.Log(MessageType.Message, "Saving configuration file.");
            _dataModel.ToFile(StaticFileProvider.ConfigFile);
        }
    }
}
