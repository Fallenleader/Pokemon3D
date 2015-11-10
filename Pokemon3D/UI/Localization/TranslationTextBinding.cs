using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Common.Localization;

namespace Pokemon3D.UI.Localization
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
            _resolveText(_translationProvider.TranslateText(_text));
        }
    }
}
