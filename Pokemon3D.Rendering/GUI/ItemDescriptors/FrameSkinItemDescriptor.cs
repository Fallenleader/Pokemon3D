using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    internal class FrameSkinItemDescriptor : SkinItemDescriptor
    {
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }
        public string NodeName { get { return "Frame"; } }

        public void Deserialize(XmlElement element)
        {
            FrameBorder = Thickness.Parse(element.GetAttribute("Border"));
            SourceRectangle = StringParser.ParseRectangle(element.GetAttribute("NormalRectangle"));
        }

        public Thickness FrameBorder { get; set; }
        public Rectangle SourceRectangle { get; set; }
    }
}
