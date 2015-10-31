using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    class TextBoxSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "TextBox"; } }

        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public Rectangle NormalRectangle { get; set; }
        public Thickness Border { get; set; }
        
        public void Deserialize(XmlElement element)
        {
            Border = Thickness.Parse(element.GetAttribute("Border"));
            NormalRectangle = StringParser.ParseRectangle(element.GetAttribute("NormalRectangle"));
        }
    }
}
