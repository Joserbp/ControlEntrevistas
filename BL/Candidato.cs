using DL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Candidato
    {
        private readonly string _connectionString;
        public Candidato()
        {

        }
        public Candidato(string connectionString)
        {
            _connectionString = connectionString;
        }

        public  ML.Result Add(ML.Candidato candidato)
        {
            ML.Result result = new ML.Result();
            try
            {   
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"CandidatoAdd '{candidato.IdCandidato}','{candidato.Nombre}', '{candidato.ApellidoPaterno}' , '{candidato.ApellidoMaterno}', '{candidato.Correo}', '{candidato.Celular}', {candidato.Vacante.IdVacante}");
                    if (rowAffected > 0)
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
        public  ML.Result Update(ML.Candidato candidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"CandidatoUpdate {candidato.IdCandidato} ,'{candidato.Nombre}', '{candidato.ApellidoPaterno}' , '{candidato.ApellidoMaterno}', '{candidato.Correo}', '{candidato.Celular}', {candidato.Vacante.IdVacante}");
                    if (rowAffected > 0)
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
        public  ML.Result Delete(string IdCandidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"CandidatoUpdate '{IdCandidato}'");
                    if (rowAffected > 0)
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
        public  ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    var candidatos = context.Candidatos.FromSqlRaw("CandidatoGetAll").ToList();
                    if (candidatos.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var objCandidato in candidatos)
                        {
                            ML.Candidato candidato = new ML.Candidato
                            {
                                IdCandidato = objCandidato.IdCandidato,
                                Nombre = objCandidato.Nombre,
                                ApellidoPaterno = objCandidato.ApellidoPaterno,
                                ApellidoMaterno = objCandidato.ApellidoMaterno,
                                Celular = objCandidato.Celular,
                                Correo = objCandidato.Correo,
                                Vacante = new ML.Vacante
                                {
                                    IdVacante = objCandidato.IdVacante.Value,
                                    Nombre = objCandidato.NombreVacante
                                }
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
        public  ML.Result GetById(string idCandidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.ControlEntrevistaContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext(optionsBuilder.Options))
                {
                    var objCandidato = context.Candidatos.FromSqlRaw($"CandidatoGetById '{idCandidato}'").AsEnumerable().FirstOrDefault();
                    if (objCandidato != null)
                    {
                        ML.Candidato candidato = new ML.Candidato
                        {
                            IdCandidato = objCandidato.IdCandidato,
                            Nombre = objCandidato.Nombre,
                            ApellidoPaterno = objCandidato.ApellidoPaterno,
                            ApellidoMaterno = objCandidato.ApellidoMaterno,
                            Celular = objCandidato.Celular,
                            Correo = objCandidato.Correo,
                            Vacante = new ML.Vacante
                            {
                                IdVacante = objCandidato.IdVacante.Value
                            }
                        };
                        result.Object = candidato;

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
