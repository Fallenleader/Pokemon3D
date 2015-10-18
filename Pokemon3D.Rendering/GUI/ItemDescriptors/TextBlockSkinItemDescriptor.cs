using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    class TextBlockSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "TextBlock"; } }
        
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public void Deserialize(XmlElement element)
        {

        }
    }
}
