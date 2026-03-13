using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("UsuarioPeriodoSemestral")]
    public class UsuarioPeriodoSemestral
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int IdUsuario { get; set; }
        [NotNull]
        public int IdPeriodoSemestral { get; set; }
        [NotNull]
        public int IdTransaccion { get; set; }
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
