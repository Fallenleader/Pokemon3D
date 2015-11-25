using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps.Generators
{
    class TexturedCubeEntityGenerator : Singleton<TexturedCubeEntityGenerator>, EntityGenerator
    {
        public List<Entity> Generate(Map map, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            List<Entity> entities = new List<Entity>();

            // scale of the entity is the entire cube's scale.
            // generate 6 entities from the one definition that is passed in
            // the definition has to hold either 1, 5 or 6 textures.
            // if there are only 5 textures, then do not generate a bottom.
            // This is an option, because often times, there is no bottom needed.
            // if there is only 1 texture, it gets put on all 6 sides.

            int? textureEnumLength = entityDefinition?.Entity?.RenderMode?.Textures?.Length;

            if (textureEnumLength.HasValue && (textureEnumLength.Value == 1 || textureEnumLength.Value == 5 || textureEnumLength.Value == 6))
            {
                TextureSourceModel[] textures = null;

                if (textureEnumLength.Value == 1)
                {
                    textures = new TextureSourceModel[]
                    {
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[0]
                    };
                }
                else if (textureEnumLength.Value == 5)
                {
                    textures = new TextureSourceModel[]
                    {
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[1],
                        entityDefinition.Entity.RenderMode.Textures[2],
                        entityDefinition.Entity.RenderMode.Textures[3],
                        entityDefinition.Entity.RenderMode.Textures[4]
                    };
                }
                else if (textureEnumLength.Value == 6)
                {
                    textures = new TextureSourceModel[]
                    {
                        entityDefinition.Entity.RenderMode.Textures[0],
                        entityDefinition.Entity.RenderMode.Textures[1],
                        entityDefinition.Entity.RenderMode.Textures[2],
                        entityDefinition.Entity.RenderMode.Textures[3],
                        entityDefinition.Entity.RenderMode.Textures[4],
                        entityDefinition.Entity.RenderMode.Textures[5]
                    };
                }

                // Front side:
                var frontEntityModel = entityDefinition.Entity.CloneModel();
                var frontEntityPlacing = new EntityFieldPositionModel();

                frontEntityModel.RenderMode.Textures = new TextureSourceModel[] { textures[0] };
                frontEntityPlacing.CardinalRotation = true;
                frontEntityPlacing.Rotation = new Vector3Model()
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                };
                frontEntityPlacing.Scale = new Vector3Model()
                {
                    X = entityPlacing.Scale.X,
                    Y = entityPlacing.Scale.Y,
                    Z = 1
                };
                var frontEntityPosition = new Vector3()
                {
                    X = position.X,
                    Y = position.Y,
                    Z = position.Z + entityPlacing.Scale.Z / 2
                };

                entities.Add(new Entity(map, frontEntityModel, frontEntityPlacing, frontEntityPosition));

                // Back side:
                var backEntityModel = entityDefinition.Entity.CloneModel();
                var backEntityPlacing = new EntityFieldPositionModel();

                backEntityModel.RenderMode.Textures = new TextureSourceModel[] { textures[1] };
                backEntityPlacing.CardinalRotation = true;
                backEntityPlacing.Rotation = new Vector3Model()
                {
                    X = 0,
                    Y = 2,
                    Z = 0
                };
                backEntityPlacing.Scale = new Vector3Model()
                {
                    X = entityPlacing.Scale.X,
                    Y = entityPlacing.Scale.Y,
                    Z = 1
                };
                var backEntityPosition = new Vector3()
                {
                    X = position.X,
                    Y = position.Y,
                    Z = position.Z - entityPlacing.Scale.Z / 2
                };

                entities.Add(new Entity(map, backEntityModel, backEntityPlacing, backEntityPosition));

                //Left side:
                var leftEntityModel = entityDefinition.Entity.CloneModel();
                var leftEntityPlacing = new EntityFieldPositionModel();

                leftEntityModel.RenderMode.Textures = new TextureSourceModel[] { textures[2] };
                leftEntityPlacing.CardinalRotation = true;
                leftEntityPlacing.Rotation = new Vector3Model()
                {
                    X = 0,
                    Y = 3,
                    Z = 0
                };
                leftEntityPlacing.Scale = new Vector3Model()
                {
                    X = entityPlacing.Scale.X,
                    Y = entityPlacing.Scale.Y,
                    Z = 1
                };
                var leftEntityPosition = new Vector3()
                {
                    X = position.X - entityPlacing.Scale.X / 2,
                    Y = position.Y,
                    Z = position.Z
                };

                entities.Add(new Entity(map, leftEntityModel, leftEntityPlacing, leftEntityPosition));

                //right side:
                var rightEntityModel = entityDefinition.Entity.CloneModel();
                var rightEntityPlacing = new EntityFieldPositionModel();

                rightEntityModel.RenderMode.Textures = new TextureSourceModel[] { textures[3] };
                rightEntityPlacing.CardinalRotation = true;
                rightEntityPlacing.Rotation = new Vector3Model()
                {
                    X = 0,
                    Y = 1,
                    Z = 0
                };
                rightEntityPlacing.Scale = new Vector3Model()
                {
                    X = entityPlacing.Scale.X,
                    Y = entityPlacing.Scale.Y,
                    Z = 1
                };
                var rightEntityPosition = new Vector3()
                {
                    X = position.X + entityPlacing.Scale.X / 2,
                    Y = position.Y,
                    Z = position.Z
                };

                entities.Add(new Entity(map, rightEntityModel, rightEntityPlacing, rightEntityPosition));

                //top:
                var topEntityModel = entityDefinition.Entity.CloneModel();
                var topEntityPlacing = new EntityFieldPositionModel();

                topEntityModel.RenderMode.Textures = new TextureSourceModel[] { textures[4] };
                topEntityPlacing.CardinalRotation = true;
                topEntityPlacing.Rotation = new Vector3Model()
                {
                    X = 3,
                    Y = 0,
                    Z = 0
                };
                topEntityPlacing.Scale = new Vector3Model()
                {
                    X = entityPlacing.Scale.X,
                    Y = entityPlacing.Scale.Y,
                    Z = 1
                };
                var topEntityPosition = new Vector3()
                {
                    X = position.X,
                    Y = position.Y + entityPlacing.Scale.Y / 2,
                    Z = position.Z
                };

                entities.Add(new Entity(map, topEntityModel, topEntityPlacing, topEntityPosition));

                //bottom:
                if (textureEnumLength.Value != 5)
                {
                    var bottomEntityModel = entityDefinition.Entity.CloneModel();
                    var bottomEntityPlacing = new EntityFieldPositionModel();

                    bottomEntityModel.RenderMode.Textures = new TextureSourceModel[] { textures[5] };
                    bottomEntityPlacing.CardinalRotation = true;
                    bottomEntityPlacing.Rotation = new Vector3Model()
                    {
                        X = 1,
                        Y = 0,
                        Z = 0
                    };
                    bottomEntityPlacing.Scale = new Vector3Model()
                    {
                        X = entityPlacing.Scale.X,
                        Y = entityPlacing.Scale.Y,
                        Z = 1
                    };
                    var bottomEntityPosition = new Vector3()
                    {
                        X = position.X,
                        Y = position.Y - entityPlacing.Scale.Y / 2,
                        Z = position.Z
                    };

                    entities.Add(new Entity(map, bottomEntityModel, bottomEntityPlacing, bottomEntityPosition));
                }
            }

            return entities;
        }
    }
}
