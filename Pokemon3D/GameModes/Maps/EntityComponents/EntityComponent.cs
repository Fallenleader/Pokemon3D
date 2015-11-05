using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// A component of an <see cref="Entity"/>, responsible for the Entity's functionality.
    /// </summary>
    partial class EntityComponent
    {
        /// <summary>
        /// The original name of this component.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The raw data of this component.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The owning parent <see cref="Entity"/> of this component.
        /// </summary>
        protected Entity Parent { get; }

        protected EntityComponent(EntityComponentDataCreationStruct parameters)
        {
            Name = parameters.Name;
            Data = parameters.Data;
            Parent = parameters.Parent;
        }

        /// <summary>
        /// Updates this property's logic.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Renders this component, if this component has special render settings for the entity.
        /// </summary>
        public virtual RenderResultType Render() { return RenderResultType.Passed; }

        public T GetData<T>()
        {
            return TypeConverter.Convert<T>(Data);
        }

        public virtual void OnComponentAdded() { }

        public virtual void OnComponentRemove() { }

        #region Behaviour

        /// <summary>
        /// Gets executed when the player interacts with the entity.
        /// </summary>
        public virtual void Click() { }

        /// <summary>
        /// The player is about to collide with the entity.
        /// </summary>
        public virtual FunctionResponse Collision() { return FunctionResponse.NoValue; }

        #endregion
    }
}
