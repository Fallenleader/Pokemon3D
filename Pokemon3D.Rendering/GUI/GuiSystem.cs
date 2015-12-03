using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Pokemon3D.Rendering.GUI.ItemDescriptors;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.GUI
{
    public class GuiSystem : GameContextObject
    {
        private GuiSystemSkinParameters _parameters;
        private readonly List<SkinItemDescriptor> _itemDescriptors;

        public GuiSystem(GameContext gameContext) : base(gameContext)
        {
            _itemDescriptors = new List<SkinItemDescriptor>
            {
                new FrameSkinItemDescriptor(),
                new TextBlockSkinItemDescriptor(),
                new ButtonSkinItemDescriptor(),
                new TextBoxSkinItemDescriptor(),
                new CheckBoxSkinItemDescriptor()
            };
        }

        public void SetSkin(GuiSystemSkinParameters parameters)
        {
            if(parameters == null) throw new ArgumentNullException(nameof(parameters));
            _parameters = parameters;

            var document = new XmlDocument();
            document.Load(_parameters.XmlSkinDescriptorFile);
            foreach(XmlElement element in document.DocumentElement.ChildNodes)
            {
                var descriptor = _itemDescriptors.FirstOrDefault(i => i.NodeName == element.LocalName);
                if(descriptor == null)
                {
                    throw new InvalidOperationException($"Descriptor for Element Type {element.LocalName} not found");
                }

                descriptor.Deserialize(element);
            }

            UpdateSkinDefinitions();
        }

        private void UpdateSkinDefinitions()
        {
            _itemDescriptors.ForEach(i =>
            {
                i.SkinTexture = _parameters.SkinTexture;
                i.NormalFont = _parameters.NormalFont;
                i.BigFont = _parameters.BigFont;
            });
        }

        public TGuiElement CreateGuiHierarchyFromXml<TGuiElement>(string xmlFile) where TGuiElement : GuiElement
        {
            var document = new XmlDocument();
            document.Load(xmlFile);

            return (TGuiElement)GuiElement.CreateFromXmlType(this, document.DocumentElement);
        }

        internal TSkinItemDescriptor GetSkinItemDescriptor<TSkinItemDescriptor>()
            where TSkinItemDescriptor : SkinItemDescriptor
        {
            return _itemDescriptors.OfType<TSkinItemDescriptor>().First();
        }
    }
}
