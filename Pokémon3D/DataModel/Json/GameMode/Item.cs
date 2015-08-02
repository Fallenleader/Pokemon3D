using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json.GameMode
{
    #region Enums

    public enum ItemCategory
    {
        Standard,
        Medicine,
        Plant,
        Pokeball,
        Machine,
        KeyItem,
        Mail,
        BattleItem
    }

    public enum BattleItemCategory
    {
        None,
        Healing,
        Status,
        Pokeball,
        BattleItem
    }

    #endregion

    class ItemModel
    {
        public string Name { get; set; }

        public string PluralName { get; set; }

        public int Price { get; set; }

        public int Id { get; set; }

        public TextureSourceModel Texture { get; set; }

        public string Description { get; set; }

        public string ScriptBinding { get; set; }
    }

    sealed class InventoryDefinitionModel : JsonDataModel
    {
        private string _itemCategory;

        public ItemCategory ItemCategory
        {
            get { return ConvertStringToEnum<ItemCategory>(_itemCategory); }
            set { _itemCategory = value.ToString(); }
        }
    }
}