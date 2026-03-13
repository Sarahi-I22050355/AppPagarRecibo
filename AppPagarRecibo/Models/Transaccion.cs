using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("Transaccion")]
    public class Transaccion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int IdUsuario { get; set; }
        [NotNull, Unique, MaxLength(20)]
        public string Referencia { get; set; }
        [MaxLength(20)]
        public string Codigo { get; set; }
        [NotNull, MaxLength(255)]
        public string Concepto { get; set; }
        [NotNull, MaxLength(10)]
        public string TipoPago { get; set; }
        [MaxLength(150)]
        public string ConceptoSPEI { get; set; }
        [MaxLength(4)]
        public string UltimosDigitos { get; set; }
        [MaxLength(30)]
        public string BancoEmisor { get; set; }
        [NotNull]
        public decimal Monto { get; set; }
        public decimal MontoDescuento { get; set; } = 0;
        [NotNull]
        public decimal MontoTotal { get; set; }
        [NotNull]
        public int IdBancoReceptor { get; set; }
        [NotNull]
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public bool Pagado { get; set; } = false;
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
