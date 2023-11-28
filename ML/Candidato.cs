namespace ML
{
    public class Candidato
    {
        public int IdCandidato { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public ML.Vacante Vacante { get; set; }

    }
}