using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaySenac.Data;
using PlaySenac.Models;

namespace PlaySenac.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly PlaySenacContext _context;

        public SalesRecordsController(PlaySenacContext context)
        {
            _context = context;
        }

        // GET: SalesRecords
        public async Task<IActionResult> Index()
        {
            var playSenacContext = _context.SalesRecord.Include(s => s.Seller);
            return View(await playSenacContext.ToListAsync());
        }

        // GET: SalesRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SalesRecord == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        // GET: SalesRecords/Create
        public IActionResult Create()
        {
            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name");
            return View();
        }

        // POST: SalesRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Amount,Status, SellerId")] SalesRecord salesRecord)
        {

            _context.Add(salesRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name", salesRecord.SellerId);
            return View(salesRecord);
        }

        // GET: SalesRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SalesRecord == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord.FindAsync(id);
            if (salesRecord == null)
            {
                return NotFound();
            }
            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name", salesRecord.SellerId);
            return View(salesRecord);
        }

        // POST: SalesRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Amount,Status,SellerId")] SalesRecord salesRecord)
        {
            if (id != salesRecord.Id)
            {
                return NotFound();
            }
                       
            try
            {
                _context.Update(salesRecord);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesRecordExists(salesRecord.Id))
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

        // GET: SalesRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SalesRecord == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }

        // POST: SalesRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SalesRecord == null)
            {
                return Problem("Entity set 'PlaySenacContext.SalesRecord'  is null.");
            }
            var salesRecord = await _context.SalesRecord.FindAsync(id);
            if (salesRecord != null)
            {
                _context.SalesRecord.Remove(salesRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesRecordExists(int id)
        {
          return (_context.SalesRecord?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
