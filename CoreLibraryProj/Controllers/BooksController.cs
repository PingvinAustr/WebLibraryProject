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
            ViewBag.AuthorList=db.Authors.ToList();
            ViewData["Books"] = db.Books.ToList();
        }



        CoreLibraryContext db = new CoreLibraryContext();
        [HttpPost]
        public ActionResult Index(string parameter1,string droppar_rubrics,string droppar_authors)
        {
            //Initialize needed variables to avoid errors after POST request
            InitializeViewBag();


           

            //Saving previously chosen items & text from the search form
            ViewBag.SaveValSearch = parameter1;
            ViewBag.SaveValRub = droppar_rubrics;
            ViewBag.SaveValAuth = droppar_authors;

            //Checking different options after form submitting

            //1... If dropdowns were not used
           
            var books = db.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).Where(a => a.BookName.Contains(parameter1)).ToList();


            //2... If all fields are empty - show all results
            if ((parameter1 == null) && (droppar_rubrics == null) && (droppar_authors == null))
            {
                var coreLibraryContext = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric);
                books = coreLibraryContext.ToList();
                return View(books);
            }

            //3... All combinations of empty/filled search bar & 2 dropdowns (to avoid null exception)
            if (parameter1 != null)
            {
                if ((droppar_authors != null) && (droppar_rubrics != null))
                {
                    books=_context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).Where(b => b.BookName.Contains(parameter1)).ToList().Where(b => b.BookRubricId == int.Parse(droppar_rubrics)).ToList().Where(b=>b.BookAuthorId==int.Parse(droppar_authors)).ToList();                 
                }
                else if ((droppar_authors != null) && (droppar_rubrics == null))
                {
                    books = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).Where(b => b.BookName.Contains(parameter1)).ToList().Where(b => b.BookAuthorId == int.Parse(droppar_authors)).ToList();
                }
                else if ((droppar_authors == null) && (droppar_rubrics != null))
                {
                    books = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).Where(b => b.BookName.Contains(parameter1)).ToList().Where(b => b.BookRubricId == int.Parse(droppar_rubrics)).ToList();
                }
            }
            else if (parameter1 == null)
            {
                if ((droppar_authors != null) && (droppar_rubrics != null))
                {
                    books = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).ToList().Where(b => b.BookRubricId == int.Parse(droppar_rubrics)).ToList().Where(b => b.BookAuthorId == int.Parse(droppar_authors)).ToList();
                }
                else if ((droppar_authors != null) && (droppar_rubrics == null))
                {
                    books = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).ToList().Where(b => b.BookAuthorId == int.Parse(droppar_authors)).ToList();
                }
                else if ((droppar_authors == null) && (droppar_rubrics != null))
                {
                    books = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).ToList().Where(b => b.BookRubricId == int.Parse(droppar_rubrics)).ToList();
                }
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



        /*
       [HttpPost]
        public ActionResult ApplyFilters(string droppar_rubrics)
        {
            ViewBag.droppar_rubrics = droppar_rubrics;   
            InitializeViewBag();
            if (droppar_rubrics == null) return View("~/Views/Books/Index.cshtml", _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric));
            return View("~/Views/Books/Index.cshtml", _context.Books.Include(b => b.BookAuthor).Include(b => b.BookRubric).Where(b=>b.BookRubricId==int.Parse(droppar_rubrics)));
        }
        */



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
