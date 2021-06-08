using System;
using System.Collections.Generic;

#nullable disable

namespace UdemyRealWorldUnitTest.Web.Models
{
    public partial class PnDetail
    {
        public Guid Id { get; set; }
        public string Desc { get; set; }
        public Guid PnMasterId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual PnMaster PnMaster { get; set; }
    }
}
