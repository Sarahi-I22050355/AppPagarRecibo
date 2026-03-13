using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("TipoOportunidad")]
    public class TipoOportunidad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, Unique]
        public int Clave { get; set; }
        [NotNull, MaxLength(30)]
        public string Descripcion { get; set; }
        public bool GeneraCostoExtra { get; set; } = false;
        public bool EsEspecial { get; set; } = false;
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
