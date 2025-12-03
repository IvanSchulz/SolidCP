using System.Collections.Generic;
using System.Linq;

namespace FuseCP.WebDav.Core.Config.Entities
{
    public class ElementsRendering : AbstractConfigCollection
    {
        public int DefaultCount { get; private set; }
        public int AddElementsCount { get; private set; }

        public ElementsRendering()
        {
            DefaultCount = ConfigSection.ElementsRendering.DefaultCount;
            AddElementsCount = ConfigSection.ElementsRendering.AddElementsCount;
        }
    }
}
