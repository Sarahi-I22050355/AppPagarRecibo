using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("Asignatura")]
    public class Asignatura
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(10)]
        public string Clave { get; set; }
        [NotNull, MaxLength(50)]
        public string Descripcion { get; set; }
        [NotNull]
        public int Semestre { get; set; }
        [NotNull]
        public DateTime FechaCreacion { get; set; }
        [NotNull]
        public DateTime FechaModificacion { get; set; }
        [NotNull]
        public int IdUsuarioCrea { get; set; }
        public int? IdUsuarioModifica { get; set; }
        public bool Estatus { get; set; } = true;
    }
}
