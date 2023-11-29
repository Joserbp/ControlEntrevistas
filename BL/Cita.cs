using Microsoft.EntityFrameworkCore;
using ML;

namespace BL
{
    public class Cita
    {
        public static ML.Result Add(ML.Cita cita)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    int rowsAffected = context.Database.ExecuteSql($"CitaAdd '{cita.Candidato.IdCandidato}', {cita.Reclutador.IdReclutador}, {cita.Status.IdStatus}, '{cita.Candidato.Nombre}','{cita.Reclutador.Nombre}'");
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