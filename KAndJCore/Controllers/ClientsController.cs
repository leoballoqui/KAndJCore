using KAndJCore.Data;
using KAndJCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClientsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string  key = "")
        {
            if(String.IsNullOrEmpty(key))
                return View(await _context.Client.Take(100).OrderByDescending(c => c.Created).ToListAsync());
            return View(await _context.Client.Where(c => c.LastName.ToLower().Contains(key.ToLower()) || c.Name.ToLower().Contains(key.ToLower())).Take(100).OrderByDescending(c => c.Created).ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            Client client = new Client();
            return View(client);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MiddleName,LastName,DOB,SSN,CurrentStatus,Created,Completed,Address,PreviousAddress,CellPhone,HomePhone,WorkPhone,OtherPhone,Fax,Email,WorkEmail")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.Id = Guid.NewGuid();
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,MiddleName,LastName,DOB,SSN,CurrentStatus,Created,Completed,Address,PreviousAddress,CellPhone,HomePhone,WorkPhone,OtherPhone,Fax,Email,WorkEmail")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        public async Task<IActionResult> Notes(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Notes(Guid id, [Bind("Id,FullName,Notes")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            try
            {
                var dbclient = await _context.Client.FindAsync(id);
                if (dbclient == null)
                {
                    return NotFound();
                }
                dbclient.Notes = client.Notes;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
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

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = await _context.Client.FindAsync(id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Affiliation(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(client);
        }

        public async Task<IActionResult> AffiliationFile(Guid? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var dbclient = await _context.Client.FirstOrDefaultAsync(c => c.Id == id);
                if (dbclient == null)
                    return NotFound();

                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertUrl(Url.Action("Affiliation", "Clients", new { id = id }, Request.Scheme));
                string path = String.Format("{0}\\documents\\{1}.pdf", this._hostingEnvironment.WebRootPath, id);
                doc.Save(path);
                doc.Close();

                string name = String.Format("{0}_Affiliation_{1}.pdf", dbclient.FullName.Replace(" ", "_"), DateTime.Now.Ticks);
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                System.IO.File.Delete(path);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id.Value))
                    return Json(new { Success = "False", responseText = "Client Not Found" });
                else
                    throw;
            }
        }

        private bool ClientExists(Guid id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}
