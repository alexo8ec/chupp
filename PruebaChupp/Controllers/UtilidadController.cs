using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using NPOI.HSSF.UserModel;
using PruebaChupp.Data;
using PruebaChupp.Models;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PruebaChupp.Controllers
{
    public class UtilidadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public UtilidadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        [Route("utilidad/subirarchivo")]
        public ActionResult subirarchivo(IFormCollection form)
        {
            try
            {
                IFormFile file = form.Files[0];
                string folderName = "/Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        string nombre = "";
                        string apellido = "";
                        string cedula = "";
                        string telefono = "";
                        string celular = "";
                        string email = "";
                        string direccion = "";
                        string fecha_nacimiento = "";
                        int cont = 0;
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    if (cont == 0)
                                        nombre = row.GetCell(j).ToString();
                                    else if (cont == 1)
                                        apellido = row.GetCell(j).ToString();
                                    else if (cont == 2)
                                    {
                                        cedula = row.GetCell(j).ToString();
                                        if (cedula.Length != 10)
                                        {
                                            return Json(new { result = false, mns = "El número de cedula debe contener 10 digitos" });
                                        }
                                        else if (!esNumero(cedula))
                                        {
                                            return Json(new { result = false, mns = "El número de cedula deben ser solo números" });
                                        }
                                    }
                                    else if (cont == 3)
                                    {
                                        telefono = row.GetCell(j).ToString();
                                        if (telefono.Length != 9)
                                        {
                                            return Json(new { result = false, mns = "El número de teléfono debe contener 9 digitos" });
                                        }
                                        else if (!esNumero(telefono))
                                        {
                                            return Json(new { result = false, mns = "El número de teléfono deben ser solo números" });
                                        }
                                    }
                                    else if (cont == 4)
                                    {
                                        celular = row.GetCell(j).ToString();
                                        if (celular.Length != 10)
                                        {
                                            return Json(new { result = false, mns = "El número de celular debe contener 10 digitos" });
                                        }
                                        else if (!esNumero(celular))
                                        {
                                            return Json(new { result = false, mns = "El número de celular deben ser solo números" });
                                        }
                                    }
                                    else if (cont == 5)
                                    {
                                        email = row.GetCell(j).ToString();
                                        if (!esEmail(email))
                                        {
                                            return Json(new { result = false, mns = "Formato de E-mail incorrecto Ej: corre@dominio.com" });
                                        }
                                    }
                                    else if (cont == 6)
                                        direccion = row.GetCell(j).ToString();
                                    else if (cont == 7)
                                    {
                                        fecha_nacimiento = row.GetCell(j).ToString();
                                        if (!esFecha(fecha_nacimiento))
                                        {
                                            return Json(new { result = false, mns = "Formato de fecha incorrecto Ej: 10-09-1983 | 10/09/1983 | 1983-09-10" });
                                        }
                                    }
                                    cont++;
                                }
                            }
                            try
                            {
                                using (var db = new ChuppContext())
                                {
                                    var query = db.personas.Where(p => p.cedula_persona == cedula).ToList();
                                    if (query.Count == 0)
                                    {
                                        var per = new Personas
                                        {
                                            nombre_persona = nombre,
                                            apellido_persona = apellido,
                                            cedula_persona = cedula,
                                            telefono_persona = telefono,
                                            celular_persona = celular,
                                            email_persona = email,
                                            direccion_persona = direccion,
                                            fecha_nacimiento_persona = Convert.ToDateTime(fecha_nacimiento),
                                            estado_persona = 1
                                        };
                                        db.personas.Add(per);
                                        if (db.SaveChanges() == 1)
                                        {
                                            cont = 0;
                                            /*esta = true;
                                            mns = "Datos guardados correctamente.";*/
                                        }
                                        else
                                        {
                                            //mns = "Ocurrio un error al guardar los datos";
                                        }
                                    }
                                    else
                                    { return Json(new { result = false, mns = "Un número de cedula ya esta ingresada en la base" }); }
                                }
                            }
                            catch (Exception ex)
                            { return Json(new { result = false, mns = ex }); }
                        }
                    }
                    return Json(new { result = true, mns = "Importacion Correcta" });
                }
                return this.Content(sb.ToString());
            }
            catch (Exception ex) { return Json(new { result = false, mns = ex }); }
        }
        [HttpGet]
        [Route("utilidad/descargarxls")]
        public FileResult descargarxls()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Uploads/plantilla_ejemplo.xls";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(sFileName);
            var memory = new MemoryStream();
            using (var fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("nombre");
                row.CreateCell(1).SetCellValue("apellido");
                row.CreateCell(2).SetCellValue("cedula");
                row.CreateCell(3).SetCellValue("telefono");
                row.CreateCell(4).SetCellValue("celular");
                row.CreateCell(5).SetCellValue("email");
                row.CreateCell(6).SetCellValue("direccion");
                row.CreateCell(7).SetCellValue("fecha_nacimiento");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Ejemplo Juan Alberto");
                row.CreateCell(1).SetCellValue("Ejemplo Perez Toala");
                row.CreateCell(2).SetCellValue("Ejemplo 0985458962");
                row.CreateCell(3).SetCellValue("Ejemplo 042569878");
                row.CreateCell(4).SetCellValue("Ejemplo 0987854523");
                row.CreateCell(5).SetCellValue("Ejemplo ejemplo@gmail.com");
                row.CreateCell(6).SetCellValue("Ejemplo direccion");
                row.CreateCell(7).SetCellValue("Ejemplo 10-09-1983");

                row = excelSheet.CreateRow(2);
                row.CreateCell(0).SetCellValue("Ejemplo Juan Alberto");
                row.CreateCell(1).SetCellValue("Ejemplo Perez Toala");
                row.CreateCell(2).SetCellValue("Ejemplo 0985458962");
                row.CreateCell(3).SetCellValue("Ejemplo 042569878");
                row.CreateCell(4).SetCellValue("Ejemplo 0987854523");
                row.CreateCell(5).SetCellValue("Ejemplo ejemplo@gmail.com");
                row.CreateCell(6).SetCellValue("Ejemplo direccion");
                row.CreateCell(7).SetCellValue("Ejemplo 10-09-1983");

                row = excelSheet.CreateRow(3);
                row.CreateCell(0).SetCellValue("Ejemplo Juan Alberto");
                row.CreateCell(1).SetCellValue("Ejemplo Perez Toala");
                row.CreateCell(2).SetCellValue("Ejemplo 0985458962");
                row.CreateCell(3).SetCellValue("Ejemplo 042569878");
                row.CreateCell(4).SetCellValue("Ejemplo 0987854523");
                row.CreateCell(5).SetCellValue("Ejemplo ejemplo@gmail.com");
                row.CreateCell(6).SetCellValue("Ejemplo direccion");
                row.CreateCell(7).SetCellValue("Ejemplo 10-09-1983");

                workbook.Write(fs);
            }
            var dir = "";
            using (var stream = new FileStream(Path.Combine(dir, sFileName), FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"Uploads/plantilla_ejemplo.xls");
            string fileName = "plantilla_ejemplo.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        private bool esNumero(string n)
        {
            int valor;
            bool esNumero;
            return int.TryParse(n, out valor);
        }
        private static Boolean esFecha(String fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool esEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
