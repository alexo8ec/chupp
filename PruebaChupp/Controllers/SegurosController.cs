using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PruebaChupp.Data;
using PruebaChupp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace PruebaChupp.Controllers
{
    public class SegurosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("seguros/consultar")]
        public ActionResult consultar()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                using (var db = new ChuppContext())
                {
                    var customerData = (from s in db.seguros
                                        join t in db.tipoSeguros on s.id_tipo_seguro equals t.id_tipo_seguro
                                        select new
                                        {
                                            id_seguro = s.id_seguro,
                                            codigo_seguro = s.codigo_seguro,
                                            nombre_seguro = s.nombre_seguro,
                                            tipo_seguro = t.tipo_seguro,
                                            fecha_creacion_seguro = s.fecha_creacion_seguro,
                                            id_tipo_seguro = s.id_tipo_seguro,
                                            valor_seguro = s.valor_seguro,
                                            estado_seguro = s.estado_seguro
                                        });
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var likeSearch = $"%{searchValue}%";
                        customerData = (from s in db.seguros
                                        join t in db.tipoSeguros on s.id_tipo_seguro equals t.id_tipo_seguro
                                        where s.codigo_seguro.Contains(searchValue)
                                        || s.nombre_seguro.Contains(searchValue)
                                        select new
                                        {
                                            id_seguro = s.id_seguro,
                                            codigo_seguro = s.codigo_seguro,
                                            nombre_seguro = s.nombre_seguro,
                                            tipo_seguro = t.tipo_seguro,
                                            fecha_creacion_seguro = s.fecha_creacion_seguro,
                                            id_tipo_seguro = s.id_tipo_seguro,
                                            valor_seguro = s.valor_seguro,
                                            estado_seguro = s.estado_seguro
                                        });

                    }
                    recordsTotal = customerData.Count();
                    var data = customerData.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [Route("seguros/tipos")]
        public ActionResult tipos()
        {
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from c in db.tipoSeguros
                                 select c);
                    return Json(new { result = JsonConvert.SerializeObject(query) });
                }
            }
            catch (Exception ex) { return Json(new { }); }
        }
        [HttpPost]
        [Route("seguros/guardar")]
        public ActionResult guardar(string datos)
        {
            bool esta = false;
            string mns = "";
            try
            {
                using (var db = new ChuppContext())
                {
                    JArray jsonArray = JArray.Parse(datos);
                    string id_seguro = "";
                    string codigo_seguro = "";
                    string nombre = "";
                    string tipo_seguro = "";
                    string estado = "";
                    string fecha_creacion = "";
                    string valor_seguro = "";
                    foreach (var e in jsonArray)
                    {
                        if (e.First.First.ToString() == "id_seguro")
                        {
                            id_seguro = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "codigo_seguro")
                        {
                            codigo_seguro = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "nombre")
                        {
                            nombre = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "tipo_seguro")
                        {
                            tipo_seguro = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "fecha_creacion")
                        {
                            fecha_creacion = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "valor")
                        {
                            valor_seguro = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "cheestado")
                        {
                            if (e.Last.Last.ToString() == "on")
                                estado = "1";
                            else
                                estado = "0";
                        }
                    }
                    if (id_seguro == "")
                    {
                        using (var db_ = new ChuppContext())
                        {
                            var query = db_.seguros.Where(p => p.nombre_seguro == nombre).ToList();
                            if (query.Count == 0)
                            {
                                int maxCod = 0;
                                try
                                {
                                    maxCod = db.seguros.Max(p => p.id_seguro) + 1 + Convert.ToInt32(tipo_seguro);
                                }
                                catch (Exception ex)
                                {
                                    maxCod++;
                                    maxCod = maxCod + Convert.ToInt32(tipo_seguro);
                                }
                                int decimalLength = maxCod.ToString("D").Length + 6;
                                var tipoS = "";
                                try
                                {
                                    tipoS = db.tipoSeguros.Where(x => x.id_tipo_seguro == Convert.ToInt32(tipo_seguro)).Select(p => p.tipo_seguro).Single();
                                    tipoS = quitarAcentos(tipoS);
                                }
                                catch (Exception ex) { }
                                var per = new Seguros
                                {
                                    nombre_seguro = nombre,
                                    codigo_seguro = tipoS.Substring(0, 2).ToUpper() + maxCod.ToString("D6"),
                                    id_tipo_seguro = Convert.ToInt32(tipo_seguro),
                                    fecha_creacion_seguro = DateTime.Now,
                                    fecha_modificacion_seguro = DateTime.Now,
                                    valor_seguro = Convert.ToDecimal(valor_seguro),
                                    estado_seguro = 1
                                };
                                db.seguros.Add(per);
                                if (db.SaveChanges() == 1)
                                {
                                    esta = true;
                                    mns = "Datos guardados correctamente.";
                                }
                                else
                                    mns = "Ocurrio un error al guardar los datos";
                            }
                            else
                            {
                                esta = false;
                                mns = "El nombre del seguro que intenta ingresar ya se encuentra en la base";
                            }
                        }
                    }
                    else
                    {
                        using (var db_ = new ChuppContext())
                        {
                            var per = new Seguros
                            {
                                id_seguro = Convert.ToInt32(id_seguro),
                                nombre_seguro = nombre,
                                codigo_seguro = codigo_seguro,
                                fecha_creacion_seguro = Convert.ToDateTime(fecha_creacion),
                                fecha_modificacion_seguro = DateTime.Now,
                                id_tipo_seguro = Convert.ToInt32(tipo_seguro),
                                valor_seguro = Convert.ToDecimal(valor_seguro),
                                estado_seguro = Convert.ToInt32(estado == "" ? 0 : estado)
                            };
                            db.seguros.Attach(per);
                            db.Entry(per).State = EntityState.Modified;
                            if (db.SaveChanges() == 1)
                            {
                                esta = true;
                                mns = "Datos guardados correctamente.";
                            }
                            else
                                mns = "Ocurrio un error al guardar los datos";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esta = false;
                mns = "Ocurrio un error al guardar los datos";
            }
            return Json(new
            {
                result = esta,
                mns = mns
            });
        }
        [HttpPost]
        [Route("seguros/eliminar")]
        public ActionResult eliminar(int id)
        {
            bool estado = false;
            string mns = "";
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from p in db.seguros
                                 where p.id_seguro == id
                                 select p).Single();

                    db.Remove(query);
                    if (db.SaveChanges() == 1)
                    {
                        estado = true;
                        mns = "Datos borrados correctamente";
                    }
                    else
                        mns = "Ocurrio un error al eliminar los datos";
                }
            }
            catch (Exception ex)
            {
                estado = false;
                mns = "Ocurrio un error al eliminar los datos";
            }
            return Json(new
            {
                result = estado,
                mns = mns
            });
        }
        [HttpPost]
        [Route("seguros/listar")]
        public ActionResult listar()
        {
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from c in db.seguros
                                 select c);
                    return Json(new { result = JsonConvert.SerializeObject(query) });
                }
            }
            catch (Exception ex) { return Json(new { }); }
        }
        [HttpPost]
        [Route("seguros/porcentajes")]
        public ActionResult porcentajes()
        {
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from c in db.porcentajes
                                 select c);
                    return Json(new { result = JsonConvert.SerializeObject(query) });
                }
            }
            catch (Exception ex) { return Json(new { }); }
        }
        [HttpPost]
        [Route("seguros/llenardatos")]
        public ActionResult llenardatos(int id_persona, int id_seguro)
        {
            try
            {
                using (var db = new ChuppContext())
                {
                    var persona = (from p in db.personas
                                   where p.id_persona == id_persona
                                   select p).Single();
                    int edad = Convert.ToInt32(CalcEdad(persona.fecha_nacimiento_persona.ToString()));

                    var seguro = (from s in db.seguros
                                  where s.id_seguro == id_seguro
                                  select new
                                  {
                                      id_seguro = s.id_seguro,
                                      codigo_seguro = s.codigo_seguro,
                                      nombre_seguro = s.nombre_seguro,
                                      fecha_creacion_seguro = s.fecha_creacion_seguro,
                                      id_tipo_seguro = s.id_tipo_seguro,
                                      valor_seguro = s.valor_seguro,
                                      estado_seguro = s.estado_seguro,
                                  }).Single();

                    var rango = (from r in db.rangos
                                 select r);
                    var rn = "";
                    var por = "";
                    var valorSeguro = "";
                    var prima = "";
                    var id_rango = "";
                    foreach (var rc in rango)
                    {
                        string[] newr = rc.rango.Split("-");
                        if (edad >= Convert.ToInt32(newr[0]) && edad <= Convert.ToInt32(newr[1]))
                        {
                            rn = newr[0] + " - " + newr[1];
                            por = rc.porcentaje.ToString();
                            valorSeguro = Convert.ToString(((seguro.valor_seguro * Convert.ToDecimal(por)) / 100) + seguro.valor_seguro);
                            prima = (((Convert.ToDecimal(valorSeguro) * 12) * 10 / 100)).ToString();
                            id_rango = Convert.ToString(rc.id_rango);
                        }
                    }

                    object[] datoretornado;

                    datoretornado = new object[] {
                        new{
                            id_seguro=seguro.id_seguro,
                            codigo_seguro = seguro.codigo_seguro,
                            nombre_seguro = seguro.nombre_seguro,
                            fecha_creacion_seguro = seguro.fecha_creacion_seguro,
                            id_tipo_seguro = seguro.id_tipo_seguro,
                            valor_base = seguro.valor_seguro,
                            estado_seguro = seguro.estado_seguro,
                            rango=rn,
                            edad=edad,
                            valor_seguro=valorSeguro,
                            prima=prima,
                            id_rango=id_rango,
                            porcentaje=por
                        }
                    };

                    return Json(new { result = JsonConvert.SerializeObject(datoretornado) });
                }
            }
            catch (Exception ex) { return Json(new { }); }
        }
        private static string CalcEdad(string fnac)
        {
            DateTime dat = Convert.ToDateTime(fnac);
            DateTime nacimiento = new DateTime(dat.Year, dat.Month, dat.Day);
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            return edad.ToString();
        }
        public string quitarAcentos(String pala)
        {
            return Regex.Replace(pala.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
