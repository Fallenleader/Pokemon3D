using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps;

namespace Pokemon3D.GameModes
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
