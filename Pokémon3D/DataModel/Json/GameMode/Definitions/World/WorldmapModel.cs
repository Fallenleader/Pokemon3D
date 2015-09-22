using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    #region Enums

    /// <summary>
    /// Different types of routes.
    /// </summary>
    public enum RouteType
    {
        Land,
        Water
    }

    /// <summary>
    /// The different orientations and visual representations of routes on the map.
    /// </summary>
    public enum RouteDirection
    {
        Horizontal,
        Vertical,

        HorizontalEndRight,
        HorizontalEndLeft,

        VerticalEndUp,
        VerticalEndDown,

        CurveDownRight,
        CurveDownLeft,
        CurveUpLeft,
        CurveUpRight,

        TUp,
        TDown,
        TRight,
        TLeft,

        HorizontalConnection,
        VerticalConnection
    }

    /// <summary>
    /// The sizes of cities on the map.
    /// </summary>
    public enum CitySize
    {
        Small,
        Vertical,
        Horizontal,
        Big,
        Large
    }

    /// <summary>
    /// The sizes of places on the map.
    /// </summary>
    public enum PlaceSize
    {
        Small,
        Vertical,
        Round,
        Square,
        VerticalBig,
        Large
    }

    #endregion

    /// <summary>
    /// A data model to represent a region world map.
    /// </summary>
    [DataContract]
    class WorldmapModel
    {
        [DataMember(Order = 0)]
        public string Region;

        [DataMember(Order = 1)]
        public TextureSourceModel Texture;

        [DataMember(Order = 2)]
        public ColorModel BackColor;

        [DataMember(Order = 3)]
        public RouteModel[] Routes;

        [DataMember(Order = 4)]
        public CityModel[] Cities;

        [DataMember(Order = 5)]
        public PlaceModel[] Places;

        [DataMember(Order = 6)]
        public EnvironmentMapObjectModel[] Environment;
    }
}
