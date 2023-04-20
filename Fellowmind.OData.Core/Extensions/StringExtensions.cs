using System;

namespace Fellowmind.OData.Core.Extensions
{
    public enum PluralOperation
    {
        /// <summary>
        /// When PluralExepton is omitted, we assume normal.
        /// </summary>
        NotDefined,

        /// <summary>
        /// the default way: add an s... For example: Quote => Quotes
        /// </summary>
        Normal,

        /// <summary>
        /// Assumes a couple of rules. Give it a try:) 
        /// </summary>
        Auto,

        /// <summary>
        /// For example: Fish => Fish
        /// </summary>
        DoNothing,

        /// <summary>
        /// For example: Opportinity => Opportunities
        /// </summary>
        YtoIES,

        /// <summary>
        /// For example: Potato => Potatoes
        /// </summary>
        OtoOES,

        /// <summary>
        /// For example: Criterion => Criteria
        /// </summary>
        ONtoA,

        /// <summary>
        /// For nouns ending with the letters -s, -x, -z, -sh or -ch, add -es, For example: Bus => Busses
        /// </summary>
        XtoES,

        /// <summary>
        /// For example: Axis => Axes
        /// </summary>
        IStoES,

        /// <summary>
        /// For example: fungus ? fungi
        /// </summary>
        UStoI,

        /// <summary>
        /// For example: Child => Children
        /// </summary>
        Irregular 
    }

    public static class StringExtensions
    {
        public static string Pluralize(this string name, string irregularForm)
        {
            return irregularForm;
        }
        public static string Pluralize(this string name)
        {
            string plural = string.Empty;
            string lowerCase = name.ToLower();
            if (lowerCase.EndsWith("o"))
                return name.Pluralize(PluralOperation.OtoOES);
            if (lowerCase.EndsWith("on"))
                return name.Pluralize(PluralOperation.ONtoA);
            if (lowerCase.EndsWith("is"))
                return name.Pluralize(PluralOperation.IStoES);
            if (lowerCase.EndsWith("us"))
                return name.Pluralize(PluralOperation.UStoI);

            // -s, -x, -z, -sh or -ch
            if (lowerCase.EndsWith("s") ||
                    lowerCase.EndsWith("x") ||
                    lowerCase.EndsWith("z") ||
                    lowerCase.EndsWith("sh") ||
                    lowerCase.EndsWith("ch")
                )
                return name.Pluralize(PluralOperation.XtoES);
            if (lowerCase.EndsWith("y"))
                return name.Pluralize(PluralOperation.YtoIES);


            return name.Pluralize(PluralOperation.Normal);
        }

        public static string Pluralize(this string name, PluralOperation pluralOperation = PluralOperation.NotDefined, string irregularForm = "")
        {
            string plural = string.Empty;

            switch (pluralOperation)
            {
                case PluralOperation.Auto:
                    plural = Pluralize(name);
                    break;
                case PluralOperation.Normal:
                case PluralOperation.NotDefined:
                    plural = name + "s";
                    break;
                case PluralOperation.DoNothing:
                    plural = name;
                    break;
                case PluralOperation.IStoES:
                    plural = name.Substring(0, name.Length - 2) + "es";
                    break;
                case PluralOperation.ONtoA:
                    plural = name.Substring(0, name.Length - 2) + "a";
                    break;
                case PluralOperation.OtoOES:
                    plural = name + "es";
                    break;
                case PluralOperation.UStoI:
                    plural = name.Substring(0, name.Length - 2) + "i";
                    break;
                case PluralOperation.XtoES:
                    plural = name + "es";
                    break;
                case PluralOperation.YtoIES:
                    plural = name.Substring(0, name.Length - 1) + "ies";
                    break;
                case PluralOperation.Irregular:
                    if (!String.IsNullOrWhiteSpace(name))
                        plural = irregularForm;
                    else
                        throw new Exception("Irregular form was specified, but no irregular form was specified.");
                    break;
            }

            return plural;

        }

    }
}

