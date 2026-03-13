using SQLite;
using System;

namespace AppPagarRecibo.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public string Nombre { get; set; }
        [NotNull, Unique, MaxLength(10)]
        public string Matricula { get; set; }
        [NotNull, MaxLength(256)]
        public string Clave { get; set; }
        [NotNull]
        public int IdTipoUsuario { get; set; }
        public int? IdBeca { get; set; }
        [NotNull]
        public int IdCarrera { get; set; }
        [NotNull]
        public int SemestreActual { get; set; }
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
