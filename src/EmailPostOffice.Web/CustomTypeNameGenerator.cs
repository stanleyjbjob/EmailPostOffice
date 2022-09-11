using NJsonSchema;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EmailPostOffice.Web
{
    public class CustomTypeNameGenerator : DefaultTypeNameGenerator
    {
        /// <inheritdoc />
        public override string Generate(NJsonSchema.JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            if (!string.IsNullOrEmpty(typeNameHint) && typeNameHint.Contains("KeyValuePair`2"))
            {
                var match = Regex.Match(typeNameHint, @"^.*KeyValuePair`2\[\[([a-zA-Z+\.]+),.*\],\[([a-zA-Z+\.]+),.*\]\]");
                var customType = $"KeyValuePair<{match.Groups[1]},{match.Groups[2]}>";
                return base.Generate(schema, customType);
            }
            // else
            return base.Generate(schema, typeNameHint, reservedTypeNames);
        }
    }
}
