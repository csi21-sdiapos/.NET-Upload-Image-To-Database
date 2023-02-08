using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

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
