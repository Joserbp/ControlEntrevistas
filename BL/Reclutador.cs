using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Reclutador
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    var reclutadores = context.Reclutadors.FromSqlRaw("ReclutadorGetAll").ToList();
                    if (reclutadores.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var objReclutador in reclutadores)
                        {
                            ML.Reclutador candidato = new ML.Reclutador
                            {
                                IdReclutador = objReclutador.IdReclutador,
                                Nombre = objReclutador.Nombre,
                                ApellidoPaterno = objReclutador.ApellidoPaterno,
                                ApellidoMaterno = objReclutador.ApellidoMaterno,
                            };
                            result.Objects.Add(candidato);
                        }
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
