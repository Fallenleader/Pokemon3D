using Pokemon3D.Common.Localization;
using System;
using System.Text.RegularExpressions;

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
            var matches = Regex.Matches(text, @"{i18n:\w+:\w+}");

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match match = matches[i];
                
                string[] parts = match.Value.Trim('{', '}').Split(':');
                string result = _translationProvider.GetTranslation(parts[1], parts[2]);

                text = text.Remove(match.Index, match.Length);

                if (result != null)
                {
                    text = text.Insert(match.Index, result);
                }
                else
                {
                    text = text.Insert(match.Index, match.Value);
                }
            }

            return text;
        }
    }
}
