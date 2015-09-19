using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a place object.
    /// </summary>
    [DataContract]
    class PlaceModel : MapObjectModel
    {
        #region PlaceSize

        [DataMember(Order = 4, Name = "PlaceSize")]
        private string _placeSize;
        
        public PlaceSize PlaceSize
        {
            get { return ConvertStringToEnum<PlaceSize>(_placeSize); }
        }

        #endregion
    }
}
