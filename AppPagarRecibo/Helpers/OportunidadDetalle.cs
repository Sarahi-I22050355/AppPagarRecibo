namespace AppPagarRecibo.Helpers
{
    public class OportunidadDetalle
    {
        public string ClaveAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public int Semestre { get; set; }
        public string TipoOportunidad { get; set; }
        public int ClaveOportunidad { get; set; }
        public bool EsEspecial { get; set; }
        public decimal CostoExtra { get; set; }
    }
}
