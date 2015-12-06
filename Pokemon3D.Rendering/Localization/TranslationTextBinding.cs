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
        private readonly TranslationProvider _translationProvider;
        private readonly string _text;
        private readonly Action<string> _resolveText;

        public TranslationTextBinding(TranslationProvider translationProvider, string text, Action<string> resolveText)
        {
            _translationProvider = translationProvider;
            _text = text;
            _resolveText = resolveText;
            _translationProvider.LanguageChanged += (s,e) => Resolve();

            Resolve();
        }

        private void Resolve()
        {
            var translateText = TranslateText(_text);
            _resolveText(translateText);
        }

        private string TranslateText(string text)
        {
            var matches = Regex.Matches(text, @"{i18n:\w+:\w+}");

            for (var i = matches.Count - 1; i >= 0; i--)
            {
                var match = matches[i];
                var parts = match.Value.Trim('{', '}').Split(':');
                var result = _translationProvider.GetTranslation(parts[1], parts[2]);

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
