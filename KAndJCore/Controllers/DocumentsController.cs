using KAndJCore.Data;
using KAndJCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KAndJCore.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DocumentsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Documents
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("clientId", id.ToString());

            var applicationDbContext = _context.Document.Where(a => a.ClientId == id).Include(d => d.Client).Include(d => d.DocumentType);
            ViewData["Client"] = client;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Client)
                .Include(d => d.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["Client"] = client;
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentType, "Id", "Name");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,DocumentTypeId,FileFullName")] Document document)
        {
            if (ModelState.IsValid)
            {
                document.Id = Guid.NewGuid();
                _context.Add(document);
                await _context.SaveChangesAsync();

                string from = this._hostingEnvironment.WebRootPath + "\\uploads\\" + document.FileFullName;
                string to = this._hostingEnvironment.WebRootPath + "\\documents\\" + document.FileFullName;
                string thumbnail = this._hostingEnvironment.WebRootPath + "\\documents\\thumbnails\\" + document.FileFullName;

                using (FileStream output = System.IO.File.Create(to))
                {
                    using (FileStream original = System.IO.File.OpenRead(from))
                    {
                        await original.CopyToAsync(output);
                        if (Path.GetExtension(from) != ".pdf")
                        {
                            Image image = Image.FromStream(original);
                            image.GetThumbnailImage(100, (image.Height * 100) / image.Width, () => false, IntPtr.Zero).Save(thumbnail);
                        }
                    }
                    System.IO.File.Delete(from);
                }

                return RedirectToAction(nameof(Index), new { id = document.ClientId });
            }
            ViewData["Client"] = await _context.Client.FirstOrDefaultAsync(c => c.Id == document.ClientId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentType, "Id", "Name", document.DocumentTypeId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Set<Client>(), "Id", "Name", document.ClientId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentType, "Id", "Name", document.DocumentTypeId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClientId,DocumentTypeId,Name")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = document.ClientId });
            }
            ViewData["Client"] = await _context.Client.FirstOrDefaultAsync(c => c.Id == document.ClientId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentType, "Id", "Name", document.DocumentTypeId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Client)
                .Include(d => d.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            string file = this._hostingEnvironment.WebRootPath + "\\documents\\" + document.FileFullName;
            string thumbnail = this._hostingEnvironment.WebRootPath + "\\documents\\thumbnails\\" + document.FileFullName;
            System.IO.File.Delete(file);
            System.IO.File.Delete(thumbnail);
            return RedirectToAction(nameof(Index), new { id = document.ClientId });
        }

        public async Task<JsonResult> Upload(IFormFile file)
        {
            string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string ext = Path.GetExtension(filename);
            string name = Guid.NewGuid().ToString();
            string path = this.GetPathAndFilename(name, ext);
            using (FileStream output = System.IO.File.Create(path))
                await file.CopyToAsync(output);

            return Json(new { FileName = name + ext });
        }

        public async Task<ActionResult> DownloadDocument(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Client)
                .Include(d => d.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }
            string name = document.Client.FullName.Replace(" ", "_") + "_" + document.DocumentType.Name.Replace(" ", "_") + "_" + DateTime.Now.Ticks + Path.GetExtension(document.FileFullName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(this._hostingEnvironment.WebRootPath + "\\documents\\" + document.FileFullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }

        public IActionResult Back()
        {
            Guid clientId;
            if (Guid.TryParse(HttpContext.Session.GetString("clientId"), out clientId))
                return RedirectToAction(nameof(Index), new { id = clientId });
            else
                return RedirectToAction("Index", "Clients");
        }

        private bool DocumentExists(Guid id)
        {
            return _context.Document.Any(e => e.Id == id);
        }

        private string GetPathAndFilename(string name, string ext)
        {
            return this._hostingEnvironment.WebRootPath /*Environment.CurrentDirectory*/ + "\\uploads\\" + name + ext;
        }

        private string ResolveExtension(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
