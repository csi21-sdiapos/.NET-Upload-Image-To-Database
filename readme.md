# Subir desde la vista una imagen a la base de datos

# 1. Instalar los paquetes NuGets

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Relational
- Microsoft.EntityFrameworkCore.Tools
- Npgsql.EntityFrameworkCore.PostgreSQL

# 2. Crear la entidad de prueba y el contexto

## 2.1. Entities --> Alumno.cs

```csharp
namespace UploadImageToDatabase.Entities
{
    [Table("alumno", Schema = "public")]
    public class Alumno
    {
        [Column("Md_uuid")]
        [Display(Name = "Md_uuid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Md_uuid { get; set; } = Guid.NewGuid();

        [Column("Md_date")]
        [Display(Name = "Md_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Md_date { get; set; } = DateTime.Now;

        [Key]
        [Column("Alumno_id")]
        [Display(Name = "Alumno_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Alumno_id { get; set; }

        [Required]
        [Column("Alumno_nombre")]
        [Display(Name = "Alumno_nombre")]
        [StringLength(10, ErrorMessage = "El nombre del alumno no puede exceder los 10 caracteres")]
        public string Alumno_nombre { get; set; }

        [Required]
        [Column("Alumno_imagen")]
        [Display(Name = "Alumno_imagen")]
        public string Alumno_imagen { get; set; }

        [NotMapped]
        public IFormFile Archivo_imagen { get; set; }
    }
}
```

## 2.2. DbContexts --> PostgreSqlContext.cs

```csharp
namespace UploadImageToDatabase.DbContexts
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions options) : base(options) 
        { }

        public virtual DbSet<Alumno> DbSetAlumnos { get; set; }
    }
}
```

## 2.3. appsettings.json

```json
"ConnectionStrings": {
    "PostgreSqlConnection": "Host=localhost;Port=5432;Pooling=true;Database=upload-image-example;UserId=postgres;Password=12345;",
}
```

## 2.4. Program.cs

```csharp
// Añadimos nuestra conexión a la BBDD de PostgreSQL
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<PostgreSqlContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
    });

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
```

## 2.5. Hacemos la migración a la BBDD

`Add-Migration migracion-1 -Context PostgreSqlContext`

`Update-Database -Context PostgreSqlContext`

# 3. Creamos el Index (lista de alumnos) y el Create

## 3.1. Pages --> Alumnos --> Create.cshtml.cs

```csharp
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
```

## 3.2. Pages --> Alumnos --> Create.cshtml

```html
<h1>Imagen de Perfil</h1>

<p>
    <a asp-page="Index">Back To List</a>
</p>

<div class="row">
    <div class="col-md-4">
        <form method="post" enctype="multipart/form-data">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>

            <div class="form-group">
                <label asp-for="Alumno.Alumno_nombre" class="control-label"></label>
                <input asp-for="Alumno.Alumno_nombre" class="form-control" />
                <span asp-validation-for="Alumno.Alumno_nombre" class="text-danger"></span>
            </div>

            <div class="form-group">
                <label asp-for="Alumno.Archivo_imagen" class="control-label"></label>
                <input type="file" asp-for="Alumno.Archivo_imagen" class="form-control" />
                <span asp-validation-for="Alumno.Archivo_imagen" class="text-danger"></span>
            </div>

            <br />

            <div class="form-group">
                <input type="submit" value="Create" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>
```

## 3.3. Pages --> Alumnos --> Index.cshtml.cs

```csharp
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
```

## 3.4. Pages --> Alumnos --> Index.cshtml

```html
<h1>Lista de Alumnos</h1>

<p>
    <a asp-page="Create">Subir una imagen de perfil del alumno</a>
</p>

<br />

<table class="table">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(model => model.listaAlumnos[0].Md_uuid)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.listaAlumnos[1].Md_date)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.listaAlumnos[2].Alumno_id)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.listaAlumnos[3].Alumno_nombre)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.listaAlumnos[4].Alumno_imagen)
            </th>
        </tr>
    </thead>

    <tbody>
        @foreach (var item in Model.listaAlumnos) {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => item.Md_uuid)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Md_date)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Alumno_id)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Alumno_nombre)
                </td>
                <td>
                    <img src="data:image/jpg;base64,@item.Alumno_imagen" width="120px" height="120px" />
                </td>
            </tr>
        }
    </tbody>
</table>
```

# 4. Routing en el navbar del Pages --> Shared --> _Layout.cshtml

```html
<nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" asp-area="" asp-page="/Index">UploadImageToDatabase</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Alumnos/Index">Alumnos</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Privacy">Privacy</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
```

# 5. Prueba de Ejecución 1 - Crear un alumno subiendo una imagen de perfil

[Prueba de Ejecución 1](https://user-images.githubusercontent.com/91122596/217535591-f8028e55-5575-49d4-97f3-f05dc6e4441a.mp4)
