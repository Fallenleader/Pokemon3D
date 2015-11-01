using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Rendering.Data;
using Pokémon3D.DataModel;
using Pokémon3D.DataModel.Json;
using Pokémon3D.DataModel.Json.GameMode;
using Pokémon3D.DataModel.Json.GameMode.Definitions;
using Pokémon3D.DataModel.Json.GameMode.Map;
using Pokémon3D.GameCore;
using Pokémon3D.GameModes.Maps;

namespace Pokémon3D.GameModes
{
    /// <summary>
    /// The main class to control a GameMode.
    /// </summary>
    partial class GameMode : IDataModelContainer, GameModeDataProvider
    {
        private const string PrimitivesFileName = "Primitives.json";

        private readonly GameModeModel _dataModel;
        private readonly string _gameModeFolder;
        private readonly List<IGameModeComponent> _components;
        private bool _initializedComponents;
        private readonly bool _isValid;
        private readonly Dictionary<string, PrimitiveModel> _primitiveModels; 

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
                _gameModeFolder = System.IO.Path.GetDirectoryName(gameModeFile);
                _dataModel = JsonDataModel.FromFile<GameModeModel>(gameModeFile);
                _components = new List<IGameModeComponent>();
                _primitiveModels = JsonDataModel.FromFile<PrimitiveModel[]>(Path.Combine(DataPath, PrimitivesFileName)).ToDictionary(pm => pm.Name, pm => pm);


                var mapModels = Directory.GetFiles(MapPath)
                                         .Where(m => m.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                                         .Select(m => JsonDataModel.FromFile<MapModel>(m)).ToArray();
                MapManager = new MapManager(mapModels);

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
                    Indices = primitiveModel.Indices.Select(i => (ushort) i).ToArray()
                };
            }
            
            throw new ApplicationException("Invalid Primitive Type: " + primitiveName);
        }
    }
}
