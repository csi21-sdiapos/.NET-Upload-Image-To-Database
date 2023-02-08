using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UploadImageToDatabase.DbContexts;
using UploadImageToDatabase.Entities;

namespace UploadImageToDatabase.Pages.Alumnos
{
    public class CreateModel : PageModel
    {
        private readonly PostgreSqlContext _postgreSqlContext;

        [BindProperty]
        public Alumno Alumno { get; set; }

        public CreateModel(PostgreSqlContext postgreSqlContext)
        {
            _postgreSqlContext = postgreSqlContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            byte[] bytesArray = null;

            if (Alumno.Archivo_imagen != null)
            {
                using (Stream fileStream = Alumno.Archivo_imagen.OpenReadStream())
                {
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        bytesArray = binaryReader.ReadBytes((Int32)fileStream.Length);
                    }
                }

                Alumno.Alumno_imagen = Convert.ToBase64String(bytesArray, 0, bytesArray.Length);
            }

            _postgreSqlContext.DbSetAlumnos.Add(Alumno);
            _postgreSqlContext.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
