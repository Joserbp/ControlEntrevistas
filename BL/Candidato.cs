using DL;
using Microsoft.EntityFrameworkCore;
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
        public static ML.Result Add(ML.Candidato candidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    int rowAffected = context.Database.ExecuteSql($"CandidatoAdd '{candidato.IdCandidato}','{candidato.Nombre}', '{candidato.ApellidoPaterno}' , '{candidato.ApellidoMaterno}', '{candidato.Correo}', '{candidato.Celular}', {candidato.Vacante.IdVacante}");
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
        public static ML.Result Update(ML.Candidato candidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    int rowAffected = context.Database.ExecuteSql($"CandidatoUpdate {candidato.IdCandidato} ,'{candidato.Nombre}', '{candidato.ApellidoPaterno}' , '{candidato.ApellidoMaterno}', '{candidato.Correo}', '{candidato.Celular}', {candidato.Vacante.IdVacante}");
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
        public static ML.Result Delete(string IdCandidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    int rowAffected = context.Database.ExecuteSql($"CandidatoUpdate '{IdCandidato}'");
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
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
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
        public static ML.Result GetById(string idCandidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ControlEntrevistaContext context = new DL.ControlEntrevistaContext())
                {
                    var objCandidato = context.Candidatos.FromSqlRaw($"CandidatoGetById '{idCandidato}'").FirstOrDefault();
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
