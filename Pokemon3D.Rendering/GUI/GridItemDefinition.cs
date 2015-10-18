using System;
using System.Xml;

namespace Pokemon3D.Rendering.GUI
{
    internal class GridItemDefinition
    {
        public const int AutoSize = int.MinValue;
        public const int MaxSize = int.MaxValue;

        public int Size { get; set; }
        public int Index { get; set; }

        public GridItemDefinition(int size, int index)
        {
            Size = size;
            Index = index;
        }

        public GridItemDefinition(string sizeAttributeName, int index, XmlElement element)
        {
            Index = index;
            Size = MaxSize;
            if (element.HasAttribute(sizeAttributeName))
            {
                var widthAsString = element.GetAttribute(sizeAttributeName);
                if(widthAsString.Equals("auto", StringComparison.OrdinalIgnoreCase))
                {
                    Size = AutoSize;
                }
                else if(widthAsString.Equals("*"))
                {
                    Size = MaxSize;
                }
                else
                {
                    Size = int.Parse(widthAsString);
                }
            }
        }
    }
}