using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes.Maps.Generators
{
    static class EntityGeneratorSupplier
    {
        private const string TexturedCubeGeneratorName = "TexturedCube";

        public static EntityGenerator GetGenerator(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return SimpleEntityGenerator.Instance;
            }
            else if (identifier.Equals(TexturedCubeGeneratorName, StringComparison.OrdinalIgnoreCase))
            {
                return TexturedCubeEntityGenerator.Instance;
            }

            return SimpleEntityGenerator.Instance;
        }
    }
}
