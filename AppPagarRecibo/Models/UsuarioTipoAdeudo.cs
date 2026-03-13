using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("UsuarioTipoAdeudo")]
    public class UsuarioTipoAdeudo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int IdUsuario { get; set; }
        [NotNull]
        public int IdTipoAdeudo { get; set; }
        [MaxLength(255)]
        public string Observacion { get; set; }
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
