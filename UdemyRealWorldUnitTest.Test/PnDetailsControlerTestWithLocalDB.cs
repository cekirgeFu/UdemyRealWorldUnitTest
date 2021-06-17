using Microsoft.AspNetCore.Mvc;
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
    public class PnDetailsControlerTestWithLocalDB : PnDetailControllerTest
    {
        public PnDetailsControlerTestWithLocalDB()
        {
            var conn = "Host=localhost;Database=ntrax;Username=postgres;Password=P@ssw0rd";
               SetContextOptions(new DbContextOptionsBuilder<ntraxContext>().UseNpgsql
                (conn).Options);
         
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnRedirectActionWithSavePnDetail()
        {
            
            var newPnDetail = new PnDetail
            {
                Id = Guid.NewGuid(),
                Desc = "yyy",
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

            Guid pnMasterId = new Guid("498afda5-3bdb-4c9e-b4bd-ff6ef202a766");
       
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
