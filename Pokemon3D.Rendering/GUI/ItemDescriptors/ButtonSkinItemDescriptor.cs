using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    public class ButtonSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "Button"; } }

        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public void Deserialize(XmlElement element)
        {
            ButtonBorder = Thickness.Parse(element.GetAttribute("Border"));
            NormalRectangle = element.GetAttribute("NormalRectangle").ParseRectangle();
            HoverRectangle = element.GetAttribute("HoverRectangle").ParseRectangle();
        }

        public Thickness ButtonBorder { get; set; }
        public Rectangle NormalRectangle { get; set; }
        public Rectangle HoverRectangle { get; set; }
    }
}
