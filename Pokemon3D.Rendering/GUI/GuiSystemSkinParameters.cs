using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.GUI
{
    public class GuiSystemSkinParameters
    {
        public string XmlSkinDescriptorFile { get; set; }
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }
    }
}