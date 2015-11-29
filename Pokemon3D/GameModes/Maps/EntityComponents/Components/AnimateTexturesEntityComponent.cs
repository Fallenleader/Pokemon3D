using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    /// <summary>
    /// An entity component that flips through the entity's textures at a set interval.
    /// </summary>
    class AnimateTexturesEntityComponent : EntityComponent
    {
        float _animationDelay;
        int _textureIndex;

        public AnimateTexturesEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            SetInitialAnimationDelay();
            _textureIndex = 0;
        }

        private void SetInitialAnimationDelay()
        {
            _animationDelay = GetData<float>();
        }

        private void SetEntityTexture()
        {
            Parent.SetTexture(_textureIndex);
        }

        public override void Update(float elapsedTime)
        {
            _animationDelay -= elapsedTime;
            if (_animationDelay <= 0f)
            {
                // Flip to next texture:
                _textureIndex++;
                if (_textureIndex >= Parent.TextureSources.Length)
                    _textureIndex = 0;

                SetEntityTexture();

                // Reset delay after flip:
                SetInitialAnimationDelay();
            }
        }
    }
}
