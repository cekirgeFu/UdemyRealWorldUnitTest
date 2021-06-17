using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using Xunit;

namespace UdemyRealWorldUnitTest.Test
{
    public class PnDetailsControlerTestWithInMemorySQLite : PnDetailControllerTest
    {
        public PnDetailsControlerTestWithInMemorySQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            SetContextOptions(new DbContextOptionsBuilder<ntraxContext>().UseSqlite
                (connection).Options);
            Seed();
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnRedirectActionWithSavePnDetail()
        {
            var newPnDetail = new PnDetail
            {
                Id = Guid.NewGuid(),
                Desc = "xxx",
                IsActive = true,
                IsDeleted = false
            };

            using (var context = new ntraxContext(_contextOptions))
            {
                var pnMaster = context.PnMasters.First();

                newPnDetail.PnMasterId = pnMaster.Id;

                var controller = new PnDetailsController(context);
                var result = await controller
                    .Create(newPnDetail);
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new ntraxContext(_contextOptions))
            {
                var pnDetail = context.PnDetails.FirstOrDefault(x => x.Desc == newPnDetail.Desc);
                Assert.Equal(newPnDetail.Desc, pnDetail.Desc);
            }

        }

        [Fact]
        public async Task DeletePnMaster_PnMasterValidId_DeleteAllPnDetails()
        {

            Guid pnMasterId = new Guid("5C60F693-BEF5-E011-A485-80EE7300C695");
       
            using (var context = new ntraxContext(_contextOptions))
            {
                var pnMaster =  await context.PnMasters.Where(x => x.Id == pnMasterId).FirstAsync();
                context.PnMasters.Remove(pnMaster);
                context.SaveChanges();              
            }

            using (var context = new ntraxContext(_contextOptions))
            {
                var pnDetails = await context.PnDetails.Where(x => x.PnMasterId == pnMasterId).ToListAsync();
                Assert.Empty(pnDetails);
            }

        }

    }
}
