using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Rendering.GUI
{
    public class Grid : GuiElement
    {
        private readonly List<GridItemDefinition> _rowDefinitions = new List<GridItemDefinition>();
        private readonly List<GridItemDefinition> _columnDefinitions = new List<GridItemDefinition>();
        private readonly Dictionary<Tuple<int, int>, GuiElement> _childElementsByLinearTableIndex = new Dictionary<Tuple<int, int>, GuiElement>();

        public Grid(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        public Grid(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            var columnDefinitionNodes = element.SelectNodes("Grid.ColumnDefinitions/ColumnDefinition");
            var rowDefinitionNodes = element.SelectNodes("Grid.RowDefinitions/RowDefinition");

            if(columnDefinitionNodes == null || columnDefinitionNodes.Count == 0)
            {
                _columnDefinitions.Add(new GridItemDefinition(GridItemDefinition.MaxSize, 0));
            }
            else
            {
                var index = 0;
                foreach(XmlElement columnElement in columnDefinitionNodes)
                {
                    _columnDefinitions.Add(new GridItemDefinition("Width", index++, columnElement));
                }
            }

            if (rowDefinitionNodes == null || rowDefinitionNodes.Count == 0)
            {
                _rowDefinitions.Add(new GridItemDefinition(GridItemDefinition.MaxSize, 0));
            }
            else
            {
                var index = 0;
                foreach (XmlElement columnElement in rowDefinitionNodes)
                {
                    _rowDefinitions.Add(new GridItemDefinition("Height", index++, columnElement));
                }
            }

            foreach (XmlElement childElement in element.ChildNodes)
            {
                if (childElement.LocalName == "Grid.ColumnDefinitions") continue;
                if (childElement.LocalName == "Grid.RowDefinitions") continue;

                var gridRow = Math.Min(_rowDefinitions.Count-1, GetAttachedRow(childElement));
                var gridColumn = Math.Min(_columnDefinitions.Count-1, GetAttachedColumn(childElement));
                var childGuiElement = CreateFromXmlType(GuiSystem, childElement);

                AddChild(gridColumn, gridRow, childGuiElement);
            }
        }

        private int GetAttachedRow(XmlElement element)
        {
            return element.HasAttribute("Grid.Row") ? int.Parse(element.GetAttribute("Grid.Row")) : 0;
        }

        private int GetAttachedColumn(XmlElement element)
        {
            return element.HasAttribute("Grid.Column") ? int.Parse(element.GetAttribute("Grid.Column")) : 0;
        }

        private void AddChild(int column, int row, GuiElement element)
        {
            _childElementsByLinearTableIndex.Add(new Tuple<int, int>(column, row), element);
            Children.Add(element);
        }

        public override Rectangle GetMinSize()
        {
            var maxHeightOfRow = new Dictionary<int, int>();
            var maxWidthOfColumn = new Dictionary<int, int>();

            foreach (var elementByIndex in _childElementsByLinearTableIndex)
            {
                var column = elementByIndex.Key.Item1;
                var row = elementByIndex.Key.Item2;
                var minSizeOfElement = elementByIndex.Value.GetMinSize();

                if (maxHeightOfRow.ContainsKey(row))
                {
                    maxHeightOfRow[row] = Math.Max(maxHeightOfRow[row], minSizeOfElement.Height);    
                }
                else
                {
                    maxHeightOfRow[row] = minSizeOfElement.Height;
                }

                if (maxWidthOfColumn.ContainsKey(column))
                {
                    maxWidthOfColumn[column] = Math.Max(maxWidthOfColumn[column], minSizeOfElement.Width);    
                }
                else
                {
                    maxWidthOfColumn[column] = minSizeOfElement.Width;    
                }
            }

            var totalHeight = maxHeightOfRow.Sum(r => r.Value);
            var totalWidth = maxWidthOfColumn.Sum(r => r.Value);

            return ApplyMarginAndHandleSize(new Rectangle(0, 0, totalWidth, totalHeight));
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            var rowHeights = new Dictionary<int, int>();
            var columnWidths = new Dictionary<int, int>();

            var totalHorizontalSpace = Bounds.Width;
            var totalVerticalSpace = Bounds.Height;

            var autoColumns = _columnDefinitions.Where(c => c.Size == GridItemDefinition.AutoSize).ToArray();
            var maxWidthColumns = _columnDefinitions.Where(c => c.Size == GridItemDefinition.MaxSize).ToArray();

            foreach (var columnDefinition in autoColumns)
            {
                var width = GetMaxWidthInColumn(columnDefinition.Index);
                columnWidths.Add(columnDefinition.Index, width);
                totalHorizontalSpace -= width;
            }

            if (maxWidthColumns.Length > 0)
            {
                var sizePerMaxWidthColumn = totalHorizontalSpace / maxWidthColumns.Length;
                foreach (var columnDefinition in maxWidthColumns)
                {
                    columnWidths.Add(columnDefinition.Index, sizePerMaxWidthColumn);
                }   
            }
            
            var autoRows = _rowDefinitions.Where(c => c.Size == GridItemDefinition.AutoSize).ToArray();
            var maxHeightRows = _rowDefinitions.Where(c => c.Size == GridItemDefinition.MaxSize).ToArray();

            foreach (var rowDefinition in autoRows)
            {
                var height = GetMaxHeightInRow(rowDefinition.Index);
                rowHeights.Add(rowDefinition.Index, height);
                totalVerticalSpace -= height;
            }

            if (maxHeightRows.Length > 0)
            {
                var sizePerMaxHeightRow = totalVerticalSpace / maxHeightRows.Length;
                foreach (var rowDefininition in maxHeightRows)
                {
                    rowHeights.Add(rowDefininition.Index, sizePerMaxHeightRow);
                }
            }
            
            var startY = Bounds.Y;
            for (var y = 0; y < _rowDefinitions.Count; y++)
            {
                var startX = Bounds.X;

                for (var x = 0; x < _columnDefinitions.Count; x++)
                {
                    var element = _childElementsByLinearTableIndex.FirstOrDefault(c => c.Key.Item1 == x && c.Key.Item2 == y).Value;
                    if (element != null)
                    {
                        element.Arrange(new Rectangle(startX, startY, columnWidths[x], rowHeights[y]));
                    }

                    startX += columnWidths[x];
                }

                startY += rowHeights[y];
            }
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            foreach (var child in Children)
            {
                child.Translate(x,y);
            }
        }

        private int GetMaxWidthInColumn(int column)
        {
            return _childElementsByLinearTableIndex.Where(kvp => kvp.Key.Item1 == column)
                                                   .Max(kvp => kvp.Value.GetMinSize().Width);
        }

        private int GetMaxHeightInRow(int row)
        {
            return _childElementsByLinearTableIndex.Where(kvp => kvp.Key.Item2 == row)
                                                   .Max(kvp => kvp.Value.GetMinSize().Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var guiElement in Children)
            {
                guiElement.Draw(spriteBatch);
            }
        }

        public override void Update(float elapsedTime)
        {
            foreach (var guiElement in Children)
            {
                guiElement.Update(elapsedTime);
            }
        }
    }
}
