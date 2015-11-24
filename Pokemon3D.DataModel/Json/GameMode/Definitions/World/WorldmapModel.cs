using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// A data model to represent a region world map.
    /// </summary>
    [DataContract]
    public class WorldmapModel : JsonDataModel<WorldmapModel>
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

        public override object Clone()
        {
            var clone = (WorldmapModel)MemberwiseClone();
            clone.Texture = Texture.CloneModel();
            clone.BackColor = BackColor.CloneModel();
            clone.Routes = (RouteModel[])Routes.Clone();
            clone.Cities = (CityModel[])Cities.Clone();
            clone.Places = (PlaceModel[])Places.Clone();
            clone.Environment = (EnvironmentMapObjectModel[])Environment.Clone();
            return clone;
        }
    }
}
