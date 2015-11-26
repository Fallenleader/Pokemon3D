using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes.Maps.Generators
{
    static class EntityGeneratorSupplier
    {
        private static Dictionary<string, EntityGenerator> _generators;

        public static EntityGenerator GetGenerator(string identifier)
        {
            if (_generators == null)
            {
                _generators = new Dictionary<string, EntityGenerator>()
                {
                    { "simple", new SimpleEntityGenerator() },
                    { "texturedcube", new TexturedCubeEntityGenerator() }
                };
            }

            if (string.IsNullOrWhiteSpace(identifier))
            {
                return _generators["simple"];
            }
            else
            {
                identifier = identifier.ToLowerInvariant();
                if (_generators.ContainsKey(identifier))
                {
                    return _generators[identifier];
                }
                else
                {
                    return _generators["simple"];
                }
            }
        }
    }
}
