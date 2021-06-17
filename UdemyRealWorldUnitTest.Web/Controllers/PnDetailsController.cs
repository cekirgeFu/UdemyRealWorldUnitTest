using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UdemyRealWorldUnitTest.Web.Models;

namespace UdemyRealWorldUnitTest.Web.Controllers
{
    public class PnDetailsController : Controller
    {
        private readonly ntraxContext _context;

        public PnDetailsController(ntraxContext context)
        {
            _context = context;
        }

        // GET: PnDetails
        public async Task<IActionResult> Index()
        {
            var ntraxContext = _context.PnDetails.Include(p => p.PnMaster);
            return View(await ntraxContext.ToListAsync());
        }

        // GET: PnDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pnDetail = await _context.PnDetails
                .Include(p => p.PnMaster)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pnDetail == null)
            {
                return NotFound();
            }

            return View(pnDetail);
        }

        // GET: PnDetails/Create
        public IActionResult Create()
        {
            ViewData["PnMasterId"] = new SelectList(_context.PnMasters, "Id", "Desc");
            return View();
        }

        // POST: PnDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Desc,PnMasterId,IsActive,IsDeleted")] PnDetail pnDetail)
        {
            if (ModelState.IsValid)
            {
                pnDetail.Id = Guid.NewGuid();
                _context.Add(pnDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PnMasterId"] = new SelectList(_context.PnMasters, "Id", "Desc", pnDetail.PnMasterId);
            return View(pnDetail);
        }

        // GET: PnDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pnDetail = await _context.PnDetails.FindAsync(id);
            if (pnDetail == null)
            {
                return NotFound();
            }
            ViewData["PnMasterId"] = new SelectList(_context.PnMasters, "Id", "Desc", pnDetail.PnMasterId);
            return View(pnDetail);
        }

        // POST: PnDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Desc,PnMasterId,IsActive,IsDeleted")] PnDetail pnDetail)
        {
            if (id != pnDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pnDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PnDetailExists(pnDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PnMasterId"] = new SelectList(_context.PnMasters, "Id", "Desc", pnDetail.PnMasterId);
            return View(pnDetail);
        }

        // GET: PnDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pnDetail = await _context.PnDetails
                .Include(p => p.PnMaster)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pnDetail == null)
            {
                return NotFound();
            }

            return View(pnDetail);
        }

        // POST: PnDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pnDetail = await _context.PnDetails.FindAsync(id);
            _context.PnDetails.Remove(pnDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PnDetailExists(Guid id)
        {
            return _context.PnDetails.Any(e => e.Id == id);
        }
    }
}
