using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("Beca")]
    public class Beca
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(30)]
        public string Descripcion { get; set; }
        [NotNull]
        public decimal Porcentaje { get; set; }
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
