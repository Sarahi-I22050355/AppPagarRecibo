using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("Banco")]
    public class Banco
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(30)]
        public string Descripcion { get; set; }
        [NotNull, MaxLength(20)]
        public string CuentaDeposito { get; set; }
        [NotNull, MaxLength(18)]
        public string CLABE { get; set; }
        [NotNull, MaxLength(30)]
        public string BancoReceptor { get; set; }
        [NotNull, MaxLength(500)]
        public string InstruccionesSPEI { get; set; }
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
