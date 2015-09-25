using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

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
