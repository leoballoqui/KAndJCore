using KAndJCore.Data;
using KAndJCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Controllers
{
    [Authorize]
    public class ReasonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReasonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reasons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reason.ToListAsync());
        }

        // GET: Reasons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reason = await _context.Reason
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reason == null)
            {
                return NotFound();
            }

            return View(reason);
        }

        // GET: Reasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value")] Reason reason)
        {
            if (ModelState.IsValid)
            {
                reason.Id = Guid.NewGuid();
                _context.Add(reason);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reason);
        }

        // GET: Reasons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reason = await _context.Reason.FindAsync(id);
            if (reason == null)
            {
                return NotFound();
            }
            return View(reason);
        }

        // POST: Reasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Value")] Reason reason)
        {
            if (id != reason.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reason);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReasonExists(reason.Id))
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
            return View(reason);
        }

        // GET: Reasons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reason = await _context.Reason
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reason == null)
            {
                return NotFound();
            }

            return View(reason);
        }

        // POST: Reasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reason = await _context.Reason.FindAsync(id);
            _context.Reason.Remove(reason);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReasonExists(Guid id)
        {
            return _context.Reason.Any(e => e.Id == id);
        }
    }
}
