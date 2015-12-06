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
                new CheckBoxSkinItemDescriptor(),
                new ComboBoxItemDescriptor()
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

        internal GuiElement CreateFromXmlType(XmlElement element)
        {
            switch (element.LocalName)
            {
                case "Frame": return new Frame(this, element);
                case "TextBlock": return new TextBlock(this, element);
                case "StackPanel": return new StackPanel(this, element);
                case "Image": return new Image(this, element);
                case "Grid": return new Grid(this, element);
                case "Button": return new Button(this, element);
                case "TextBox": return new TextBox(this, element);
                case "ScrollViewer": return new ScrollViewer(this, element);
                case "CheckBox": return new CheckBox(this, element);
                case "ComboBox": return new ComboBox(this, element);
            }

            throw new ArgumentException("Invalid Element Type", nameof(element));
        }

        public TGuiElement CreateGuiHierarchyFromXml<TGuiElement>(string xmlFile) where TGuiElement : GuiElement
        {
            var document = new XmlDocument();
            document.Load(xmlFile);

            return (TGuiElement)CreateFromXmlType(document.DocumentElement);
        }

        internal TSkinItemDescriptor GetSkinItemDescriptor<TSkinItemDescriptor>()
            where TSkinItemDescriptor : SkinItemDescriptor
        {
            return _itemDescriptors.OfType<TSkinItemDescriptor>().First();
        }
    }
}
