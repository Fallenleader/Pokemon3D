using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokémon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokémon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// The response from a player interfaction with an <see cref="EntityComponent"/>.
    /// </summary>
    enum FunctionResponse
    {
        False = 0,
        True = 1,
        NoValue
    }

    /// <summary>
    /// The result type of the render method call of an <see cref="EntityComponent"/>.
    /// </summary>
    enum RenderResultType
    {
        /// <summary>
        /// This entity component rendered something and was the last component in this entity to render something.
        /// </summary>
        Rendered,
        /// <summary>
        /// This entity component rendered something, but the next component might also render something.
        /// </summary>
        RenderedButPassed,
        /// <summary>
        /// This entity component did not render something.
        /// </summary>
        Passed
    }

    /// <summary>
    /// A component of an <see cref="Entity"/>, responsible for the Entity's functionality.
    /// </summary>
    class EntityComponent
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
