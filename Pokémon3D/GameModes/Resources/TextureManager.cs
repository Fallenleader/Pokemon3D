using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Pokémon3D.DataModel.Json;
using System.IO;

namespace Pokémon3D.GameModes.Resources
{
    /// <summary>
    /// Manages <see cref="Texture2D"/> instances for a <see cref="GameMode"/>.
    /// </summary>
    class TextureManager : IGameModeComponent, IDisposable
    {
        private static Texture2D _defaultTexture;

        private GameMode _gameMode;

        private ContentManager _contentManager;
        private Dictionary<string, Texture2D> _textures;

        public void Activated(GameMode gameMode)
        {
            _contentManager = new ContentManager(GameCore.GameController.Instance.Services, gameMode.TexturePath);
            _textures = new Dictionary<string, Texture2D>();

            _gameMode = gameMode;
        }

        public void FreeResources()
        {
            for (int i = 0; i < _textures.Count; i++)
                _textures.Values.ElementAt(i).Dispose();

            _contentManager.Dispose();

            _textures.Clear();
        }

        public void Dispose()
        {
            FreeResources();
        }

        /// <summary>
        /// Returns a texture loaded from a resource path relative to the texture path of the GameMode.
        /// </summary>
        public Texture2D GetTexture(string resource)
        {
            return InternalGetTexture(resource, _gameMode.TexturePath, null);
        }

        /// <summary>
        /// Returns a subregion of a texture loaded from a resource path relative to the texture path of the GameMode.
        /// </summary>
        public Texture2D GetTexture(string resource, Rectangle rectangle)
        {
            return InternalGetTexture(resource, _gameMode.TexturePath, rectangle);
        }

        /// <summary>
        /// Returns a texture defined by a <see cref="DataModel.Json.TextureSourceModel"/>.
        /// </summary>
        public Texture2D GetTexture(TextureSourceModel resourceModel)
        {
            return InternalGetTexture(resourceModel.Source, _gameMode.TexturePath, resourceModel.Rectangle.GetRectangle());
        }

        private Texture2D InternalGetTexture(string resource, string basePath, Rectangle? rectangle)
        {
            // Build an identifier for the resource file & rectangle:
            string identifier = resource.ToLowerInvariant();
            if (rectangle.HasValue)
            {
                identifier += rectangle.Value.X + "," +
                              rectangle.Value.Y + "," +
                              rectangle.Value.Width + "," +
                              rectangle.Value.Height;
            }

            Texture2D texture = null;

            if (!_textures.Keys.Contains(identifier))
            {
                try
                {
                    if (rectangle.HasValue && _textures.Keys.Contains(resource))
                    {
                        texture = _textures[resource];
                    }
                    else
                    {
                        // 1. Look up if an .xnb file exists.
                        // 2. If so, load with the default ContentManager.Load<Texture2D> method.
                        // 3. If no .xnb file is found, try looking for a .png file instead and try to load that.

                        string filePathWithoutExtension = Path.Combine(new string[] { basePath, resource });

                        if (File.Exists(filePathWithoutExtension + ".xnb"))
                        {
                            texture = _contentManager.Load<Texture2D>(resource);
                        }
                        else if (File.Exists(filePathWithoutExtension + ".png"))
                        {
                            using (Stream stream = File.Open(filePathWithoutExtension + ".png", FileMode.Open))
                            {
                                texture = Texture2D.FromStream(GameCore.GameController.Instance.GraphicsDevice, stream);
                            }
                        }
                        
                        _textures.Add(resource, texture);
                    }

                    if (rectangle.HasValue)
                    {
                        _textures.Add(identifier, GetTextureRectangle(texture, rectangle.Value));
                    }
                }
                catch (Exception)
                {
                    //If anything goes wrong during loading, add the default texture for this identifier:
                    _textures.Add(identifier, texture);
                }
            }

            texture = _textures[identifier];
            return texture;
        }

        private Texture2D GetTextureRectangle(Texture2D texture, Rectangle rectangle)
        {
            //Copies a subregion of a texture and returns it in a new texture.

            int dataLength = rectangle.Width * rectangle.Height;
            Color[] colorData = new Color[dataLength];

            texture.GetData(0, rectangle, colorData, 0, dataLength);

            Texture2D croppedTexture = new Texture2D(GameCore.GameController.Instance.GraphicsDevice, rectangle.Width, rectangle.Height);
            croppedTexture.SetData(colorData);

            return croppedTexture;
        }

        private Texture2D GetDefaultTexture()
        {
            //If the default texture has not been loaded yet, do so.
            //When this does not work, crash the game.
            if (_defaultTexture == null)
                _defaultTexture = GameCore.GameController.Instance.Content.Load<Texture2D>(ResourceNames.Textures.default_texture);

            return _defaultTexture;
        }

    }
}
