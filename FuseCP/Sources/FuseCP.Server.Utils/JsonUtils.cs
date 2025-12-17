// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace FuseCP.Providers.Utils
{
    public class JsonUtils
    {
        /// <summary>
        /// A brute force convertion to Json. Any serialization Exceptions are ignored. Formatting: Indented
        /// </summary>
        /// <remarks>
        /// See also
        /// <seealso cref="Objects.CloneBySerialization{T}"/>  //todo alternatieven bespreken
        /// <seealso cref="Objects.CloneBySerializationSimple{T}"/>
        /// </remarks>
        public static string ConvertToJsonForLogging<T>(T source, bool indent = true, string method = "")
        {
            try
            {
                if (source is JObject retval) // no need to convert a JObject
                    return retval.ToString();

                // Create settings to suppress any serialization Exceptions
                var settings = new JsonSerializerSettings
                {
                    Error = (serializer, err) => err.ErrorContext.Handled = true,
                    Formatting = (indent ? Formatting.Indented : Formatting.None)
                };

                // Serialze the sourceObject
                return JsonConvert.SerializeObject(source, settings);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"[{method}] ConvertToJson failed. Error: {ex.Message}");
            }
        }
    }
}
