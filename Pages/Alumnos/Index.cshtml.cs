using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UploadImageToDatabase.DbContexts;
using UploadImageToDatabase.Entities;

namespace UploadImageToDatabase.Pages.Alumnos
{
    public class IndexModel : PageModel
    {
        private readonly PostgreSqlContext _postgreSqlContext;

        public IndexModel(PostgreSqlContext postgreSqlContext)
        {
            _postgreSqlContext = postgreSqlContext;
        }

        public IList<Alumno> listaAlumnos { get; set; }

        public void OnGet()
        {
            if (_postgreSqlContext.DbSetAlumnos != null)
            {
                listaAlumnos = _postgreSqlContext.DbSetAlumnos.ToList();
            }
        }
    }
}
