using Pokemon3D.Common.Diagnostics;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.i18n;
using Pokemon3D.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon3D.UI.Localization
{
    class CoreTranslationManager : TranslationManager
    {
        public CoreTranslationManager()
        {
            if (Directory.Exists(StaticFileProvider.i18nDirectory))
            {
                List<SectionModel> sectionModels = new List<SectionModel>();

                var files = Directory.GetFiles(StaticFileProvider.i18nDirectory).
                    Where(m => m.EndsWith(i18nFileExtension, StringComparison.OrdinalIgnoreCase));

                foreach (var file in files)
                {
                    try
                    {
                        sectionModels.AddRange(JsonDataModel.FromFile<SectionModel[]>(file));
                    }
                    catch (JsonDataLoadException)
                    {
                        GameLogger.Instance.Log(MessageType.Error, "Error trying to load internationalization file (" + file + ").");
                    }
                }

                Load(sectionModels.ToArray());
            }
            else
            {
                GameLogger.Instance.Log(MessageType.Warning, "The internationalization folder (\"i18n\") was not found.");
            }
        }
    }
}
