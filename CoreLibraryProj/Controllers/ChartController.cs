using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibraryProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {

        private readonly CoreLibraryContext _context;
        public ChartController(CoreLibraryContext context)
        {
            _context = context;
        }


        //Pie chart rubrics
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var categories = _context.Rubrics.ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Рубрика", "Кількість творів" });
            foreach (var c in categories)
            {
                int k = 0;
                foreach (var b in _context.Books.ToList())
                {
                    if (b.BookRubricId == c.Id) k++;
                }
                catBook.Add(new object[] { c.RubricName, k });
            }
            return new JsonResult(catBook);
        }


        //Pie chart authors
        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var authors = _context.Authors.ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Автор", "Кількість творів" });
            foreach (var a in authors)
            {
                int k = 0;
                foreach (var b in _context.Books.ToList())
                {
                    if (b.BookAuthorId == a.Id) k++;
                }
                catBook.Add(new object[] { a.AuthorName, k });
            }
            return new JsonResult(catBook);
        }



        public static string NameCountryByLanguage(string language)
        {
            string country="";

            switch (language)
            {
                case "Російська": { country = "Russia"; break; }
                case "Українська": { country = "Ukraine"; break; }
                case "Німецька": { country = "Germany"; break; }
                case "Англійська": { country = "United States"; break; }
                case "Іспанська": { country = "Spain"; break; }
                case "Французька": { country = "France"; break; }



                default: { break; }
            }



            return country;
        }


        //Pie chart countries
        [HttpGet("JsonData3")]
        public JsonResult JsonData3()
        {
            var languages = _context.Languages.ToList();
            List<object> catBook = new List<object>();
            catBook.Add(new[] { "Країна", "Кількість творів" });
            foreach (var l in languages)
            {
                int k = 0;
                foreach (var b in _context.DocumentFullTexts.ToList())
                {
                    if (b.LanguageId == l.Id) k++;
                }



                catBook.Add(new object[] { NameCountryByLanguage(l.LanguageName), k });
            }


            


            return new JsonResult(catBook);
        }


    }
}
