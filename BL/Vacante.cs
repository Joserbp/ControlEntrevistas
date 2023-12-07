using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Vacante
    {
        private readonly string _connectionString;
        public Vacante()
        {

        }
        public Vacante(string connectionString)
        {
            _connectionString = connectionString;
        }
        public  ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    var vacantes = (from a in context.Vacantes 
                                   select a).ToList();
                    if (vacantes.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var objVacante in vacantes)
                        {
                            ML.Vacante vacante = new ML.Vacante
                            {
                                IdVacante = objVacante.IdVacante,
                                Nombre = objVacante.Nombre,
                            };
                            result.Objects.Add(vacante);
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
