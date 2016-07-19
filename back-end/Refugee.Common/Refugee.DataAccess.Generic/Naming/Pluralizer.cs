using System.Collections.Generic;
using System.Text.RegularExpressions;
using EnsureThat;

namespace Refugee.DataAccess.Generic.Naming
{
    public static class Pluralizer
    {
        #region Constructors

        static Pluralizer()
        {
            AddPluralRule("$", "s");
            AddPluralRule("s$", "s");
            AddPluralRule("^(ax|test)is$", "$1es");
            AddPluralRule("(octop|vir)us$", "$1i");
            AddPluralRule("(alias|status)$", "$1es");
            AddPluralRule("(bu)s$", "$1ses");
            AddPluralRule("(buffal|tomat)o$", "$1oes");
            AddPluralRule("([ti])um$", "$1a");
            AddPluralRule("sis$", "ses");
            AddPluralRule("(?:([^f])fe|([lr])f)$", "$1$2ves");
            AddPluralRule("(hive)$", "$1s");
            AddPluralRule("([^aeiouy]|qu)y$", "$1ies");
            AddPluralRule("(x|ch|ss|sh)$", "$1es");
            AddPluralRule("(matr|vert|ind)(?:ix|ex)$", "$1ices");
            AddPluralRule("^(m|l)ouse$", "$1ice");
            AddPluralRule("^(ox)$", "$1en");
            AddPluralRule("(quiz)$", "$1zes");
        }

        #endregion

        #region Private Methods

        private static void AddPluralRule(string rule, string replacement)
        {
            PluralRules.Add(new KeyValuePair<string, string>(rule, replacement));
        }

        private static string ApplyRules(IList<KeyValuePair<string, string>> rules, string word)
        {
            string result = word;

            for (int i = rules.Count - 1; i >= 0; i--)
            {
                Regex regex = new Regex(rules[i].Key, RegexOptions.IgnoreCase);

                if (regex.IsMatch(word))
                {
                    result = regex.Replace(word, rules[i].Value);

                    if (word == word.ToUpper())
                    {
                        result = result.ToUpper();
                    }

                    break;
                }
            }

            return result;
        }

        #endregion

        #region Public Methods

        public static string ToPlural(string word)
        {
            Ensure.That(nameof(word)).IsNotNullOrWhiteSpace();

            return ApplyRules(PluralRules, word);
        }

        #endregion

        #region Private Readonly Fields

        private static readonly IList<KeyValuePair<string, string>> PluralRules = new List<KeyValuePair<string, string>>();

        #endregion
    }
}