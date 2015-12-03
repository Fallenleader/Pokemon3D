using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    public class CheckBoxSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName => "CheckBox";
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }


        public void Deserialize(XmlElement element)
        {
            NormalRectangle = StringParser.ParseRectangle(element.GetAttribute("NormalRectangle"));
            HoverRectangle = StringParser.ParseRectangle(element.GetAttribute("HoverRectangle"));
            CheckmarkRectangle = StringParser.ParseRectangle(element.GetAttribute("CheckmarkRectangle"));
        }

        public Rectangle CheckmarkRectangle { get; private set; }

        public Rectangle HoverRectangle { get; private set; }

        public Rectangle NormalRectangle { get; private set; }
    }
}
