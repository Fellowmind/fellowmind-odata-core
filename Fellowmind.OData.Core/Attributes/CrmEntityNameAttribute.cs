using System;

namespace Fellowmind.OData.Core.Attributes
{
    public class CrmEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CrmEntityNameAttribute"/>.
        /// </summary>
        /// <param name="entityName">Name of the entity</param>
        /// <param name="entityNamePlural">Name of the entity in plural. If not specified, then plural is EntityName + 's'</param>
        public CrmEntityNameAttribute(string entityName, string entityNamePlural = null)
        {
            EntityName = entityName;
            EntityNamePlural = entityNamePlural ?? entityName + "s";
        }

        public string EntityName { get; set; }

        public string EntityNamePlural { get; set; }
    }
}
