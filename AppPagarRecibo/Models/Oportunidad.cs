using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("Oportunidad")]
    public class Oportunidad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int IdUsuario { get; set; }
        [NotNull]
        public int IdAsignatura { get; set; }
        [NotNull]
        public int IdTipoOportunidad { get; set; }
        [NotNull]
        public decimal CostoExtra { get; set; } = 0;
        public bool? Aprobado { get; set; } = null;
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
