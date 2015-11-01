using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;
using Pokémon3D.DataModel.Json.GameMode.Map.Entities;
using Pokémon3D.GameModes.Maps;

namespace Pokémon3D.GameModes
{
    class Player
    {
        private Camera _camera;
        private SceneNode _walkingNode;

        public Player(Scene scene, Camera camera)
        {
            _camera = camera;
            _walkingNode = scene.CreateSceneNode();
        }
    }
}
