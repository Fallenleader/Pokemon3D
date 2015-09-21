using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokémon3D.DataModel;
using Pokémon3D.DataModel.Json;
using Pokémon3D.DataModel.Json.GameMode;

namespace Pokémon3D.GameModes
{
    /// <summary>
    /// The main class to control a GameMode.
    /// </summary>
    partial class GameMode : IDataModelContainer
    {
        private GameModeModel _dataModel;
        private string _gameModeFolder;
        private List<IGameModeComponent> _components;
        private bool _initializedComponents;
        private bool _isValid;

        /// <summary>
        /// Returns if the container loaded the data correctly.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }

        /// <summary>
        /// A shortcut to the active GameMode instance.
        /// </summary>
        public static GameMode Active()
        {
            return GameCore.State.GameModeManager.ActiveGameMode;
        }

        public GameMode(string gameModeFile)
        {
            try
            {
                _dataModel = JsonDataModel.FromFile<GameModeModel>(gameModeFile);

                _isValid = true;
            }
            catch (JsonDataLoadException)
            {
                //Something went wrong processing the data from a GameMode config file.
                //Log the error and mark the instance as invalid.

                GameCore.State.Logger.Log(Debug.MessageType.Error, "An error occurred trying to load the GameMode config file \"" + gameModeFile + "\".");

                _isValid = false;
            }
        }

        public void Unload()
        {

        }
    }
}
