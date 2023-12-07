using Microsoft.EntityFrameworkCore;
using ML;

namespace BL
{
    public class Cita
    {
        private readonly string _connectionString;
        public Cita()
        {

        }
        public Cita(string connectionString)
        {
            _connectionString = connectionString;
        }
        public  ML.Result Add(ML.Cita cita)
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    int rowsAffected = context.Database.ExecuteSql($"CitaAdd {cita.Candidato.IdCandidato},{cita.Reclutador.IdReclutador}, {cita.Status.IdStatus},{cita.Candidato.Nombre},{cita.Reclutador.Nombre}");
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false; 
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}