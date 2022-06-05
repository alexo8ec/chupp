using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PruebaChupp.Data;
using PruebaChupp.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace PruebaChupp.Controllers
{
    public class PersonasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("personas/consultar")]
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
                    var customerData = (from tempcustomer in db.personas
                                        select tempcustomer);
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var likeSearch = $"%{searchValue}%";
                        customerData = (from c in db.personas
                                        where c.cedula_persona.Contains(searchValue)
                                         || c.nombre_persona.Contains(searchValue)
                                         || c.apellido_persona.Contains(searchValue)
                                        select c);
                    }
                    recordsTotal = customerData.Count();
                    var data = customerData.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [Route("personas/guardar")]
        public ActionResult guardar(string datos)
        {
            bool esta = false;
            string mns = "";
            try
            {
                using (var db = new ChuppContext())
                {
                    JArray jsonArray = JArray.Parse(datos);
                    string id_persona = "";
                    string nombre = "";
                    string apellido = "";
                    string cedula = "";
                    string direccion = "";
                    string email = "";
                    string telefono = "";
                    string celular = "";
                    string estado = "";
                    string fecha_nacimiento = "";
                    foreach (var e in jsonArray)
                    {
                        if (e.First.First.ToString() == "id_persona")
                        {
                            id_persona = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "nombre")
                        {
                            nombre = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "apellido")
                        {
                            apellido = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "cedula")
                        {
                            cedula = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "direccion")
                        {
                            direccion = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "email")
                        {
                            email = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "telefono")
                        {
                            telefono = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "celular")
                        {
                            celular = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "fecha_nacimiento")
                        {
                            fecha_nacimiento = e.Last.Last.ToString();
                        }
                        else if (e.First.First.ToString() == "cheestado")
                        {
                            if (e.Last.Last.ToString() == "on")
                                estado = "1";
                            else
                                estado = "0";
                        }
                    }
                    if (id_persona == "")
                    {
                        using (var db_ = new ChuppContext())
                        {
                            var query = db_.personas.Where(p => p.cedula_persona == cedula).ToList();
                            if (query.Count == 0)
                            {
                                var per = new Personas
                                {
                                    nombre_persona = nombre,
                                    apellido_persona = apellido,
                                    cedula_persona = cedula,
                                    direccion_persona = direccion,
                                    email_persona = email,
                                    telefono_persona = telefono,
                                    celular_persona = celular,
                                    estado_persona = Convert.ToInt32(estado == "" ? 0 : estado),
                                    fecha_nacimiento_persona = Convert.ToDateTime(fecha_nacimiento)
                                };
                                db.personas.Add(per);
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
                                mns = "El número de cedula que intenta ingresar ya se encuentra en la base";
                            }
                        }
                    }
                    else
                    {
                        using (var db_ = new ChuppContext())
                        {
                            var per = new Personas
                            {
                                id_persona = Convert.ToInt32(id_persona),
                                nombre_persona = nombre,
                                apellido_persona = apellido,
                                cedula_persona = cedula,
                                direccion_persona = direccion,
                                email_persona = email,
                                telefono_persona = telefono,
                                celular_persona = celular,
                                estado_persona = Convert.ToInt32(estado == "" ? 0 : estado),
                                fecha_nacimiento_persona = Convert.ToDateTime(fecha_nacimiento)
                            };
                            db.personas.Attach(per);
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
            return Json(new { result = esta, mns = mns });
        }
        [HttpPost]
        [Route("personas/eliminar")]
        public ActionResult eliminar(int id)
        {
            bool estado = false;
            string mns = "";
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from p in db.personas
                                 where p.id_persona == id
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
        [Route("personas/listar")]
        public ActionResult listar()
        {
            try
            {
                using (var db = new ChuppContext())
                {
                    var query = (from c in db.personas
                                 select c);
                    return Json(new { result = JsonConvert.SerializeObject(query) });
                }
            }
            catch (Exception ex) { return Json(new { }); }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
