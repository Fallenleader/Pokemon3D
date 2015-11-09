namespace Pokemon3D.Common.Localization
{
    public interface TranslationProvider
    {
        string GetTranslation(string languageId, string sectionId, string tokenId);

        string TranslateText(string text);
    }
}
