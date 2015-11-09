using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// The render mode model for an entity.
    /// </summary>
    [DataContract]
    public class EntityRenderModeModel : JsonDataModel
    {
        [DataMember(Name = "RenderMethod", Order = 0)]
        private string RenderMethodStr;

        public RenderMethod RenderMethod
        {
            get
            {
                return ConvertStringToEnum<RenderMethod>(RenderMethodStr);
            }
            set
            {
                RenderMethodStr = value.ToString();
            }
        }

        [DataMember(Order = 1)]
        public TextureSourceModel[] Textures;
        
        [DataMember(Order = 3)]
        public string PrimitiveModelId;

        [DataMember(Order = 4)]
        public bool RenderBackfaces;

        [DataMember(Order = 5)]
        public string ModelPath;

        [DataMember(Order = 6)]
        public bool Visible;

        [DataMember(Order = 7)]
        public double Opacity;

        [DataMember(Order = 8)]
        public Vector3Model Shading;

        [DataMember(Order = 9)]
        public bool ObstructCamera;

        [DataMember(Order = 10)]
        public EntitySeasonPaletteModel[] SeasonPalettes;
    }
}
