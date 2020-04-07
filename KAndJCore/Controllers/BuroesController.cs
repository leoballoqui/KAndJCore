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
    public class BuroesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuroesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Buroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Buro.ToListAsync());
        }

        // GET: Buroes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buro = await _context.Buro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buro == null)
            {
                return NotFound();
            }

            return View(buro);
        }

        // GET: Buroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buroes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddressLine1,AddressLine2,AddressLine3")] Buro buro)
        {
            if (ModelState.IsValid)
            {
                buro.Id = Guid.NewGuid();
                _context.Add(buro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buro);
        }

        // GET: Buroes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buro = await _context.Buro.FindAsync(id);
            if (buro == null)
            {
                return NotFound();
            }
            return View(buro);
        }

        // POST: Buroes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,AddressLine1,AddressLine2,AddressLine3")] Buro buro)
        {
            if (id != buro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuroExists(buro.Id))
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
            return View(buro);
        }

        // GET: Buroes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buro = await _context.Buro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buro == null)
            {
                return NotFound();
            }

            return View(buro);
        }

        // POST: Buroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var buro = await _context.Buro.FindAsync(id);
            _context.Buro.Remove(buro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuroExists(Guid id)
        {
            return _context.Buro.Any(e => e.Id == id);
        }
    }
}
