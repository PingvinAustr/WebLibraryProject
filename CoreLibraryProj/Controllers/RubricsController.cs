#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLibraryProj;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;

namespace CoreLibraryProj.Controllers
{
    [Authorize(Roles ="admin,editor")]
    public class RubricsController : Controller
    {
        private readonly CoreLibraryContext _context;

        public RubricsController(CoreLibraryContext context)
        {
            _context = context;
        }

        // GET: Rubrics
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated == false) { return Redirect("~/Home"); }
            return View(await _context.Rubrics.ToListAsync());
        }

        // GET: Rubrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubric = await _context.Rubrics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rubric == null)
            {
                return NotFound();
            }

            return View(rubric);
        }

        // GET: Rubrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rubrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RubricName")] Rubric rubric)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rubric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rubric);
        }

        // GET: Rubrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubric = await _context.Rubrics.FindAsync(id);
            if (rubric == null)
            {
                return NotFound();
            }
            return View(rubric);
        }

        // POST: Rubrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RubricName")] Rubric rubric)
        {
            if (id != rubric.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rubric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RubricExists(rubric.Id))
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
            return View(rubric);
        }

        // GET: Rubrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubric = await _context.Rubrics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rubric == null)
            {
                return NotFound();
            }

            return View(rubric);
        }

        // POST: Rubrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rubric = await _context.Rubrics.FindAsync(id);
            _context.Rubrics.Remove(rubric);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RubricExists(int id)
        {
            return _context.Rubrics.Any(e => e.Id == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Rubric newcat;
                                var c = (from cat in _context.Rubrics
                                         where cat.RubricName.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Rubric();
                                    newcat.RubricName = worksheet.Name;

                                    //додати в контекст
                                    _context.Rubrics.Add(newcat);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Book book = new Book();
                                        book.BookName = row.Cell(1).Value.ToString();
                                        book.BookDescription = row.Cell(6).Value.ToString();
                                        book.BookRubric = newcat;
                                        _context.Books.Add(book);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Author author;

                                                var a = (from aut in _context.Authors
                                                         where aut.AuthorName.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    author = a[0];
                                                }
                                                else
                                                {
                                                    author = new Author();
                                                    author.AuthorName = row.Cell(i).Value.ToString();

                                                    //додати в контекст
                                                    _context.Add(author);
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            CoreLibraryContext db = new CoreLibraryContext();
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var categories = _context.Rubrics.Include("Books").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проєктах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in categories)
                { 
                    var worksheet = workbook.Worksheets.Add(c.RubricName);

                    worksheet.Cell("A1").Value = "Назва";
                    worksheet.Cell("B1").Value = "Короткий опис";                  
                    worksheet.Cell("C1").Value = "Автор";                  
                    worksheet.Cell("D1").Value = "Файл з повним текстом твору";                  
                    worksheet.Row(1).Style.Font.Bold = true;
                    var books = c.Books.ToList();

                    worksheet.Columns("A:A").Width = 30;
                    worksheet.Columns("B:B").Width = 115;
                    worksheet.Columns("C:C").Width = 30;
                    worksheet.Columns("D:D").Width = 43;

                   

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < books.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = books[i].BookName;
                        
                        
                        worksheet.Cell(i + 2, 2).Value = books[i].BookDescription;
                        worksheet.Cell(i + 2, 2).Style.Alignment.WrapText = true;


                        foreach (Author author in db.Authors.ToList())
                        {
                            if (author.Id==books[i].BookAuthorId) worksheet.Cell(i + 2, 3).Value = author.AuthorName;
                        }

                        string all_files = "";
                        foreach (DocumentFullText dft in db.DocumentFullTexts.ToList())
                        {
                            if (dft.DocumentId == books[i].Id)
                            {
                                all_files += dft.FullDocumentText + ", ";
                            }
                        }
                        worksheet.Cell(i + 2, 4).Value = all_files;






                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        //змініть назву файла відповідно до тематики Вашого проєкту

                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }





    }
}
