using Pokemon3D.Common.Localization;
using System;

namespace Pokemon3D.Rendering.Localization
{
    /// <summary>
    /// A translation binding, which dynamically reloads the translation when the language was changed.
    /// </summary>
    class TranslationTextBinding
    {
        private TranslationProvider _translationProvider;
        private string _text;
        private Action<string> _resolveText;

        public static void Create(TranslationProvider translationProvider, string text, Action<string> resolveText)
        {
            new TranslationTextBinding(translationProvider, text, resolveText);
        }

        private TranslationTextBinding(TranslationProvider translationProvider, string text, Action<string> resolveText)
        {
            _translationProvider = translationProvider;
            _text = text;
            _resolveText = resolveText;
            _translationProvider.LanguageChanged += new EventHandler(TranslationChanged);

            // Resolve initially:
            Resolve();
        }

        private void TranslationChanged(object sender, EventArgs e)
        {
            Resolve();
        }

        private void Resolve()
        {
            _resolveText(TranslateText(_text));
        }

        private string TranslateText(string text)
        {
            int searchIndex = 0;
            int foundIndex = text.IndexOf("{i18n>", searchIndex);
            int endIndex = -1;

            while (foundIndex > -1)
            {
                endIndex = text.IndexOf("}", foundIndex);
                if (endIndex > -1)
                {
                    string replace = text.Substring(foundIndex, endIndex - foundIndex + 1);
                    string[] parts = replace.Trim('{', '}').Split('>');

                    if (parts.Length == 3)
                    {
                        text = text.Remove(foundIndex, endIndex - foundIndex + 1);

                        string result = _translationProvider.GetTranslation(parts[1], parts[2]);

                        if (result != null)
                        {
                            text = text.Insert(foundIndex, result);
                            searchIndex += result.Length;
                        }
                        else
                        {
                            text = text.Insert(foundIndex, replace);
                            searchIndex += replace.Length;
                        }
                    }
                    else
                    {
                        searchIndex = endIndex;
                    }
                }
                else
                {
                    searchIndex = foundIndex + 6;
                }
                foundIndex = text.IndexOf("{i18n>", searchIndex);
            }

            return text;
        }
    }
}
