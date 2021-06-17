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
    public class PnDetailsControlerTestWithInMemory : PnDetailControllerTest
    {
        public PnDetailsControlerTestWithInMemory()
        {
            SetContextOptions(new DbContextOptionsBuilder<ntraxContext>().UseInMemoryDatabase
                ("ntraxContextInMemory").Options);
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

    }
}
