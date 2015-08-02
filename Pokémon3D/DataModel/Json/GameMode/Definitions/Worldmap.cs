using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions
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
    class Worldmap
    {
        /// <summary>
        /// The region this map represents.
        /// </summary>
        [DataMember(Order = 0)]
        public string Region { get; private set; }

        /// <summary>
        /// The background texture of the map.
        /// </summary>
        [DataMember(Order = 1)]
        public TextureSourceModel Texture { get; private set; }

        /// <summary>
        /// The background color of the map.
        /// </summary>
        [DataMember(Order = 2)]
        public ColorModel BackColor { get; private set; }

        /// <summary>
        /// The routes on this map.
        /// </summary>
        [DataMember(Order = 3)]
        public List<RouteModel> Routes { get; private set; }

        /// <summary>
        /// The cities on this map.
        /// </summary>
        [DataMember(Order = 4)]
        public List<CityModel> Cities { get; private set; }

        /// <summary>
        /// The places on this map.
        /// </summary>
        [DataMember(Order = 5)]
        public List<PlaceModel> Places { get; private set; }

        /// <summary>
        /// The additional environment objects on this map.
        /// </summary>
        [DataMember(Order = 6)]
        public List<EnvironmentMapObject> Environment { get; private set; }
    }

    /// <summary>
    /// A data model for a misc environment map object for decoration.
    /// </summary>
    [DataContract]
    sealed class EnvironmentMapObject : JsonDataModel
    {
        /// <summary>
        /// The position of the object on the map.
        /// </summary>
        [DataMember(Order = 0)]
        public Vector2Model Position { get; private set; }

        /// <summary>
        /// The size of the object.
        /// </summary>
        [DataMember(Order = 1)]
        public float Size { get; private set; }

        /// <summary>
        /// The texture of the object.
        /// </summary>
        [DataMember(Order = 2)]
        public TextureSourceModel Texture { get; private set; }
    }

    /// <summary>
    /// A data model for the fly destination used by a map object.
    /// </summary>
    [DataContract]
    class FlyToModel : JsonDataModel
    {
        /// <summary>
        /// The resulting fly position.
        /// </summary>
        [DataMember(Order = 0)]
        public Vector3Model Position { get; private set; }

        /// <summary>
        /// The target map file.
        /// </summary>
        [DataMember(Order = 1)]
        public string Mapfile { get; private set; }
    }

    /// <summary>
    /// A base data model for the map objects.
    /// </summary>
    [DataContract]
    abstract class MapObjectModel : JsonDataModel
    {
        /// <summary>
        /// The display name of the map object.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name { get; private set; }

        /// <summary>
        /// The position of the object on the map.
        /// </summary>
        [DataMember(Order = 1)]
        public Vector2Model Position { get; private set; }

        /// <summary>
        /// The map files that the player needs to be on in order to be displayed on this map object.
        /// </summary>
        [DataMember(Order = 2)]
        public List<string> Mapfiles { get; private set; }

        /// <summary>
        /// The FlyTo data of this map object.
        /// </summary>
        [DataMember(Order = 3)]
        public FlyToModel FlyTo { get; private set; }
    }

    /// <summary>
    /// The data model for a city object.
    /// </summary>
    [DataContract]
    class CityModel : MapObjectModel
    {
        #region CitySize

        [DataMember(Order = 4, Name = "CitySize")]
        private string _citySize;

        /// <summary>
        /// The size type of the city.
        /// </summary>
        public CitySize CitySize
        {
            get { return ConvertStringToEnum<CitySize>(_citySize); }
        }

        #endregion
    }

    /// <summary>
    /// The data model for a route object.
    /// </summary>
    [DataContract]
    class RouteModel : MapObjectModel
    {
        #region RouteType

        [DataMember(Order = 4, Name = "RouteType")]
        private string _routeType;

        /// <summary>
        /// The type of route.
        /// </summary>
        public RouteType RouteType
        {
            get { return ConvertStringToEnum<RouteType>(_routeType); }
        }

        #endregion

        #region RouteDirection

        [DataMember(Order = 5, Name = "RouteDirection")]
        private string _routeDirection;

        /// <summary>
        /// The direction this route object points to.
        /// </summary>
        public RouteDirection RouteDirection
        {
            get { return ConvertStringToEnum<RouteDirection>(_routeDirection); }
        }

        #endregion
    }

    /// <summary>
    /// The data model for a place object.
    /// </summary>
    [DataContract]
    class PlaceModel : MapObjectModel
    {
        #region PlaceSize

        [DataMember(Order = 4, Name = "PlaceSize")]
        private string _placeSize;

        /// <summary>
        /// The size of this place.
        /// </summary>
        public PlaceSize PlaceSize
        {
            get { return ConvertStringToEnum<PlaceSize>(_placeSize); }
        }

        #endregion
    }
}