using System.Runtime.Serialization;
using System.Globalization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameCore
{
    [DataContract]
    public class ConfigurationModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string DisplayLanguage;

        [DataMember(Order = 1)]
        public int MusicVolume;

        [DataMember(Order = 2)]
        public int SoundVolume;

        [DataMember(Order = 3)]
        public SizeModel WindowSize;

        public static ConfigurationModel Default
        {
            get
            {
                return new ConfigurationModel()
                {
                    DisplayLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                    MusicVolume = 75,
                    SoundVolume = 100,
                    WindowSize = new SizeModel()
                    {
                        Width = 1024,
                        Height = 600
                    }
                };
            }
        }
    }
}
