using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class BillboardEntityComponent : EntityComponent
    {
        public BillboardEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        { }

        public override void OnComponentAdded()
        {
            // Set the billboard property of the scene node, when this component got added to the entity:
            Parent.SceneNode.IsBillboard = Parent.RenderMethod == RenderMethod.Primitive;
        }

        public override void OnComponentRemove()
        {
            Parent.SceneNode.IsBillboard = false;
        }
    }
}
