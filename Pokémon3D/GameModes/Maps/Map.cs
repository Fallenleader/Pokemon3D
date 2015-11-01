using System.Collections.Generic;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokémon3D.GameModes.Maps
{
    class Map
    {
        private List<Entity> _allEntitis;

        public Scene Scene { get; private set; }
        public ResourceManager ResourceManager { get; private set; }

        public Map(Scene scene, ResourceManager resourceManager)
        {
            _allEntitis = new List<Entity>();
            Scene = scene;
            ResourceManager = resourceManager;
        } 

        public void Update(float elapsedTime)
        {
        }
    }
}
