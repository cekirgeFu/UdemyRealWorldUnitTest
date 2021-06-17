using System;
using System.Collections.Generic;

#nullable disable

namespace UdemyRealWorldUnitTest.Web.Models
{
    public partial class PnMaster
    {
        public PnMaster()
        {
            PnDetails = new HashSet<PnDetail>();
        }

        public Guid Id { get; set; }
        public string Desc { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; } 
        public virtual ICollection<PnDetail> PnDetails { get; set; }
    }
}
