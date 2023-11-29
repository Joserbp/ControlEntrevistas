namespace ML
{
    public class Vacante
    {
        public int IdVacante { get; set; }
        public string Nombre { get; set; }
        public ML.Empresa Empresa { get; set; }
        public List<object> Vacantes { get; set; }
    }
}