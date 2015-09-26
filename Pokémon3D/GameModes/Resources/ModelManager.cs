using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.GameModes.Resources
{
    /// <summary>
    /// Manages <see cref="Model"/> instances for a <see cref="GameMode"/>.
    /// </summary>
    class ModelManager : IGameModeComponent, IDisposable
    {
        private GameMode _gameMode;

        private ContentManager _contentManager;
        private Dictionary<string, Model> _models;

        public void Activated(GameMode gameMode)
        {
            _contentManager = new ContentManager(GameCore.GameController.Instance.Services, gameMode.ModelPath);
            _models = new Dictionary<string, Model>();

            _gameMode = gameMode;
        }

        public void FreeResources()
        {
            _contentManager.Dispose();
            _models.Clear();
        }

        public void Dispose()
        {
            FreeResources();
        }
        
        /// <summary>
        /// Returns a <see cref="Model"/> loaded from a resource path relative to the model path of the GameMode.
        /// </summary>
        public Model GetModel(string resource)
        {
            string identifier = resource.ToLowerInvariant();

            if (!_models.Keys.Contains(identifier))
            {
                try
                {
                    Model model = _contentManager.Load<Model>(resource);
                    _models.Add(identifier, model);
                }
                catch (ContentLoadException ex)
                {
                    Diagnostics.GameLogger.Instance.Log(ex);
                    _models.Add(identifier, null);
                }
            }

            return _models[identifier];
        }

    }
}
