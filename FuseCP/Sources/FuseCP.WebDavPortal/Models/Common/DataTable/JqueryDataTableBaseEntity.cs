using System.Collections;
using System.Collections.Generic;

namespace FuseCP.WebDavPortal.Models.Common.DataTable
{
    public abstract class JqueryDataTableBaseEntity 
    {
        public abstract dynamic this[int index]
        {
            get; 
        }
    }
}
