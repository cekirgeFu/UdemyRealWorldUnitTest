using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Web.Models;

namespace UdemyRealWorldUnitTest.Test
{
    public class PnDetailControllerTest
    {

        protected static DbContextOptions<ntraxContext> _contextOptions { get; private set; }

        public void SetContextOptions(DbContextOptions<ntraxContext> contextOptions)
        {
            _contextOptions = contextOptions;
            
        }

        public static void Seed()
        {
            Guid pnMasterId = new Guid("5C60F693-BEF5-E011-A485-80EE7300C695");
            using(ntraxContext context = new ntraxContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var master_guid = pnMasterId;
                context.PnMasters.Add(new PnMaster()
                {
                    Id = master_guid,
                    Desc = "xxx",                   
                    IsActive = true,
                    IsDeleted = false
                });
                context.SaveChanges();

                context.PnDetails.Add(new PnDetail()
                {
                    Id = Guid.NewGuid(),
                    Desc = "xxx",
                    PnMasterId = master_guid,
                    IsActive = true,
                    IsDeleted = false
                });
                context.SaveChanges();
            }
        }
    }
}
