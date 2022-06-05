using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PruebaChupp.Data;
using PruebaChupp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaChupp.Controllers
{
    public class VentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("ventas/consultar")]
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
                    var customerData = (from v in db.ventas
                                        join s in db.seguros on v.id_seguro_venta equals s.id_seguro
                                        join p in db.personas on v.id_persona_venta equals p.id_persona
                                        join r in db.rangos on v.id_rango_venta equals r.id_rango
                                        join po in db.porcentajes on v.id_porcentaje_venta equals po.id_porcentaje
                                        select new
                                        {
                                            id_persona = p.id_persona,
                                            id_venta = v.id_venta,
                                            codigo_seguro = s.codigo_seguro,
                                            nombre_seguro = s.nombre_seguro,
                                            nombre_persona = p.nombre_persona,
                                            apellido_persona = p.apellido_persona,
                                            rango = r.rango,
                                            porcentaje = r.porcentaje,
                                            fecha_creacion = v.fecha_creacion_venta,
                                            prima = v.prima.ToString(),
                                            valor_venta = v.valor_venta,
                                            id_seguro = s.id_seguro,
                                            id_porcentaje = po.id_porcentaje,
                                            fecha_nacimiento = p.fecha_nacimiento_persona,
                                            valor_base = s.valor_seguro

                                        });
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var likeSearch = $"%{searchValue}%";
                        customerData = (from v in db.ventas
                                        join s in db.seguros on v.id_seguro_venta equals s.id_seguro
                                        join p in db.personas on v.id_persona_venta equals p.id_persona
                                        join r in db.rangos on v.id_rango_venta equals r.id_rango
                                        join po in db.porcentajes on v.id_porcentaje_venta equals po.id_porcentaje
                                        select new
                                        {
                                            id_persona = p.id_persona,
                                            id_venta = v.id_venta,
                                            codigo_seguro = s.codigo_seguro,
                                            nombre_seguro = s.nombre_seguro,
                                            nombre_persona = p.nombre_persona,
                                            apellido_persona = p.apellido_persona,
                                            rango = r.rango,
                                            porcentaje = r.porcentaje,
                                            fecha_creacion = v.fecha_creacion_venta,
                                            prima = v.prima.ToString(),
                                            valor_venta = v.valor_venta,
                                            id_seguro = s.id_seguro,
                                            id_porcentaje = po.id_porcentaje,
                                            fecha_nacimiento = p.fecha_nacimiento_persona,
                                            valor_base = s.valor_seguro
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
        [Route("ventas/guardar")]
        public ActionResult guardar(string datos)
        {
            bool esta = false;
            string mns = "";
            try
            {
                using (var db = new ChuppContext())
                {
                    JArray jsonArray = JArray.Parse(datos);
                    string id_venta = "";
                    string id_seguro = "";
                    string id_persona = "";
                    string fecha_creacion = "";
                    string id_rango = "";
                    string valor = "";
                    string id_porcentaje = "";
                    string prima = "";
                    foreach (var e in jsonArray)
                    {
                        if (e.First.First.ToString() == "id_venta")
                        {
                            id_venta = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "id_seguro")
                        {
                            id_seguro = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "id_persona")
                        {
                            id_persona = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "valor_seguro")
                        {
                            valor = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "fecha_creacion")
                        {
                            fecha_creacion = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "id_rango")
                        {
                            id_rango = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "id_porcentaje")
                        {
                            id_porcentaje = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "prima")
                        {
                            prima = e.Last.Last.ToString();
                        }
                    }
                    if (id_venta == "")
                    {
                        using (var db_ = new ChuppContext())
                        {
                            var query = db_.ventas.Where(p => p.id_seguro_venta == Convert.ToInt32(id_seguro)).ToList();
                            if (query.Count == 0)
                            {
                                var per = new Ventas
                                {
                                    id_persona_venta = Convert.ToInt32(id_persona),
                                    id_seguro_venta = Convert.ToInt32(id_seguro),
                                    id_porcentaje_venta = Convert.ToInt32(id_porcentaje),
                                    id_rango_venta = Convert.ToInt32(id_rango),
                                    valor_venta = Convert.ToDecimal(valor),
                                    fecha_creacion_venta = DateTime.Now,
                                    prima = Convert.ToDecimal(prima)
                                };
                                db.ventas.Add(per);
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
                                mns = "La persona ya tiene un seguro de estas caracteristicas";
                            }
                        }
                    }
                    else
                    {
                        using (var db_ = new ChuppContext())
                        {
                            var per = new Ventas
                            {
                                id_venta = Convert.ToInt32(id_venta),
                                id_persona_venta = Convert.ToInt32(id_persona),
                                id_seguro_venta = Convert.ToInt32(id_seguro),
                                id_porcentaje_venta = Convert.ToInt32(id_porcentaje),
                                id_rango_venta = Convert.ToInt32(id_rango),
                                valor_venta = Convert.ToDecimal(valor),
                                prima = Convert.ToDecimal(prima)
                            };
                            db.ventas.Attach(per);
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
        [Route("ventas/eliminar")]
        public ActionResult eliminar(int id)
        {
            bool estado = false;
            string mns = "";
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from p in db.ventas
                                 where p.id_venta == id
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
