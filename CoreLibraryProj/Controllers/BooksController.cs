#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLibraryProj;
using Microsoft.EntityFrameworkCore;
using CoreLibraryProj.Models;





namespace CoreLibraryProj.Controllers
{
    public class BooksController : Controller
    {
        private readonly CoreLibraryContext _context;

        public BooksController(CoreLibraryContext context)
        {
            _context = context;
            this.db = context;

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
                .Include(b=>b.DocumentFullTexts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public VirtualFileResult GetVirtualFile(string book_name_parameter,string file_name_select)
        {

            
                string file_path_db = "";

                foreach (DocumentFullText text in db.DocumentFullTexts.ToList())
                {
                    if (text.DocumentId == int.Parse(book_name_parameter) && text.LanguageId == int.Parse(file_name_select))
                    {
                        file_path_db = text.FullDocumentText;
                    }
                }

                var filepath = Path.Combine("~/lib/texts", file_path_db);
            
           
            return File(filepath, "text/plain", file_path_db);
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
