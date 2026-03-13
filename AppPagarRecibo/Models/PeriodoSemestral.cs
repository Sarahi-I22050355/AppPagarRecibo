using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("PeriodoSemestral")]
    public class PeriodoSemestral
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(50)]
        public string Descripcion { get; set; }
        [NotNull]
        public DateTime FechaInicia { get; set; }
        [NotNull]
        public DateTime FechaTermina { get; set; }
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
