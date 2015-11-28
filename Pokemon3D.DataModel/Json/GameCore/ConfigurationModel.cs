using System.Runtime.Serialization;
using System.Globalization;
using System;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameCore
{
    [DataContract]
    public class ConfigurationModel : JsonDataModel<ConfigurationModel>
    {
        [DataMember(Order = 0)]
        public string DisplayLanguage;

        [DataMember(Order = 1)]
        public int MusicVolume;

        [DataMember(Order = 2)]
        public int SoundVolume;

        [DataMember(Order = 3)]
        public SizeModel WindowSize;

        [DataMember(Order = 4)]
        public bool ShadowsEnabled;

        [DataMember(Order = 5, Name = "ShadowQuality")]
        private string _shadowQuality;

        public ShadowQuality ShadowQuality
        {
            get { return ConvertStringToEnum<ShadowQuality>(_shadowQuality); }
            set { _shadowQuality = value.ToString(); }
        }

        [DataMember(Order = 6)]
        public bool SoftShadows;

        public static ConfigurationModel Default
        {
            get
            {
                return new ConfigurationModel()
                {
                    DisplayLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                    MusicVolume = 75,
                    SoundVolume = 100,
                    ShadowsEnabled = true,
                    SoftShadows = true,
                    ShadowQuality = ShadowQuality.Medium,
                    WindowSize = new SizeModel()
                    {
                        Width = 1024,
                        Height = 600
                    }
                };
            }
        }

        public override object Clone()
        {
            var clone = (ConfigurationModel)MemberwiseClone();
            clone.WindowSize = WindowSize.CloneModel();
            return clone;
        }
    }
}
