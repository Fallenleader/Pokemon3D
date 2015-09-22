using Microsoft.Xna.Framework;
using System;

namespace Pokémon3D.Rendering
{
    /// <summary>
    /// Flags indicating which parts of a world matrix are dirty.
    /// </summary>
    [Flags]
    enum WorldDirtyFlags
    {
        None = 0,
        Scale = 1,
        Rotation = 2,
        Position = 4
    }

    /// <summary>
    /// Manages the state of scale, rotation and position for a world matrix.
    /// </summary>
    class WorldCreator
    {
        private IWorldDataContainer _parent;

        private const WorldDirtyFlags ALL_DIRTY = WorldDirtyFlags.Scale | WorldDirtyFlags.Rotation | WorldDirtyFlags.Position;

        private WorldDirtyFlags _dirtyFlags = ALL_DIRTY;

        private Matrix _scaleM;
        private Matrix _rotationM;
        private Matrix _positionM;

        private Matrix _worldM;

        public WorldCreator(IWorldDataContainer parent)
        {
            _parent = parent;
        }

        private bool IsDirty(WorldDirtyFlags worldPart)
        {
            return (_dirtyFlags & worldPart) == worldPart;
        }

        /// <summary>
        /// Sets the dirty state for part of a world matrix so it gets recalculated at the next world request.
        /// </summary>
        public void SetDirty(WorldDirtyFlags worldPart)
        {
            _dirtyFlags |= worldPart;
        }

        /// <summary>
        /// Gets the world matrix and recalculates it, if neccessary.
        /// </summary>
        public Matrix GetWorldMatrix()
        {
            if (_dirtyFlags != WorldDirtyFlags.None)
            {
                if (IsDirty(WorldDirtyFlags.Scale))
                {
                    _dirtyFlags &= ~WorldDirtyFlags.Scale;
                    _scaleM = Matrix.CreateScale(_parent.Scale);
                }
                if (IsDirty(WorldDirtyFlags.Rotation))
                {
                    _dirtyFlags &= ~WorldDirtyFlags.Rotation;
                    _rotationM = Matrix.CreateFromQuaternion(_parent.Rotation);
                }
                if (IsDirty(WorldDirtyFlags.Position))
                {
                    _dirtyFlags &= ~WorldDirtyFlags.Position;
                    _positionM = Matrix.CreateTranslation(_parent.Position);
                }

                _worldM = _scaleM * _rotationM * _positionM;
            }

            return _worldM;
        }
    }
}
