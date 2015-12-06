using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    class ComboBoxItemDescriptor : SkinItemDescriptor
    {
        public string NodeName => "ComboBox";

        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public Rectangle NormalRectangle { get; set; }
        public Rectangle HoverRectangle { get; set; }
        public Thickness Border { get; set; }
        public Rectangle PopupRectangle { get; set; }
        public Thickness PopupBorder { get; set; }
        public Rectangle ToggleButtonRectangle { get; set; }

        public void Deserialize(XmlElement element)
        {
            NormalRectangle = StringParser.ParseRectangle(element.GetAttribute("NormalRectangle"));
            HoverRectangle = StringParser.ParseRectangle(element.GetAttribute("HoverRectangle"));
            Border = Thickness.Parse(element.GetAttribute("Border"));
            PopupRectangle = StringParser.ParseRectangle(element.GetAttribute("PopupRectangle"));
            PopupBorder = Thickness.Parse(element.GetAttribute("PopupBorder"));
            ToggleButtonRectangle = StringParser.ParseRectangle(element.GetAttribute("ToggleButtonRectangle"));
        }
    }
}
