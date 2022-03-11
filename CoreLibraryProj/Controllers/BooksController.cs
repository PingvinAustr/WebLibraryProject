#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLibraryProj;

namespace CoreLibraryProj.Controllers
{
    public class BooksController : Controller
    {
        private readonly CoreLibraryContext _context;

        public BooksController(CoreLibraryContext context)
        {
            _context = context;
        }

       public  void InitializeViewBag()
        {
            ViewBag.RubricsDropDown= new SelectList(db.Rubrics.ToList(), "Id", "RubricName");
            ViewBag.RubricsList=db.Rubrics.ToList();
            ViewData["Books"] = db.Books.ToList();
        }



        CoreLibraryContext db = new CoreLibraryContext();
        [HttpPost]
        public ActionResult Index(string parameter1)
        {
            InitializeViewBag();
            var books = db.Books.Where(a => a.BookName.Contains(parameter1)).ToList();
            var coreLibraryContext1=_context.Books.Include(b=>b.BookAuthor).Include(b=>b.BookRubric).Where(b=>b.BookName.Contains(parameter1)).ToList();
            books = coreLibraryContext1.ToList();

            if (parameter1 ==null)
            {
               
                var coreLibraryContext = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric); 
                books = coreLibraryContext.ToList();
                return View(books);
            }
           
            return View("~/Views/Books/Index.cshtml",books);
           

        }
       
        // GET: Books
        [HttpGet]
        public async Task<IActionResult> Index()
        {       
            InitializeViewBag();
            var coreLibraryContext = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric);
            return View(await coreLibraryContext.ToListAsync());
        }



       [HttpPost]
        public ActionResult ApplyFilters(string droppar)
        {
            ViewBag.droppar = droppar;   
            InitializeViewBag();
            return View("~/Views/Books/Index.cshtml", _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric));
        }




        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookRubric)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["BookAuthorId"] = new SelectList(_context.Authors, "Id", "Id");
            ViewData["BookRubricId"] = new SelectList(_context.Rubrics, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookName,BookDescription,BookRubricId,BookAuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookAuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.BookAuthorId);
            ViewData["BookRubricId"] = new SelectList(_context.Rubrics, "Id", "Id", book.BookRubricId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["BookAuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.BookAuthorId);
            ViewData["BookRubricId"] = new SelectList(_context.Rubrics, "Id", "Id", book.BookRubricId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookName,BookDescription,BookRubricId,BookAuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["BookAuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.BookAuthorId);
            ViewData["BookRubricId"] = new SelectList(_context.Rubrics, "Id", "Id", book.BookRubricId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookRubric)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
