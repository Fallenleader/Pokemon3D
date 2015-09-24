using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Pokémon3D.DataModel;
using Pokémon3D.DataModel.Json;
using Pokémon3D.DataModel.Json.GameMode;
using Pokémon3D.GameCore;
using Pokémon3D.Diagnostics;

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
            return GameController.Instance.GameModeManager.ActiveGameMode;
        }

        /// <summary>
        /// Creates an instance of the <see cref="GameMode"/> class and loads the data model.
        /// </summary>
        public GameMode(string gameModeFile)
        {
            try
            {
                _dataModel = JsonDataModel.FromFile<GameModeModel>(gameModeFile);
                _components = new List<IGameModeComponent>();
                _gameModeFolder = System.IO.Path.GetDirectoryName(gameModeFile);

                _isValid = true;
            }
            catch (JsonDataLoadException)
            {
                //Something went wrong processing the data from a GameMode config file.
                //Log the error and mark the instance as invalid.

                GameLogger.Instance.Log(Diagnostics.MessageType.Error, "An error occurred trying to load the GameMode config file \"" + gameModeFile + "\".");

                _isValid = false;
            }
        }

        private void InitializeComponents()
        {
            // Search the assembly for types that implement the IGameModeComponent interface:
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetType().IsAssignableFrom(typeof(IGameModeComponent)));

            // Create instances of those types and add those instances to the component list.
            foreach (var type in types)
                AddComponent((IGameModeComponent)Activator.CreateInstance(type));

            _initializedComponents = true;
        }

        private void AddComponent(IGameModeComponent component)
        {
            _components.Add(component);
            component.Activated(this);
        }

        /// <summary>
        /// Returns a component of this GameMode.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        public T GetComponent<T>() where T : IGameModeComponent
        {
            if (!_initializedComponents)
                InitializeComponents();

            return (T)_components.Find(c => c.GetType() == typeof(T));
        }

        /// <summary>
        /// Frees all resources consumed by this GameMode.
        /// </summary>
        public void FreeResources()
        {
            foreach (IGameModeComponent component in _components)
                component.FreeResources();
        }
    }
}
