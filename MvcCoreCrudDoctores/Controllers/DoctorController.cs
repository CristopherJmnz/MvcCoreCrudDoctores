using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudDoctores.Models;
using MvcCoreCrudDoctores.Repositories;

namespace MvcCoreCrudDoctores.Controllers
{
    public class DoctorController : Controller
    {
        private DoctorRepository drepo;
        public DoctorController()
        {
            this.drepo= new DoctorRepository();
        }
        public async Task<IActionResult> Index()
        {
            return View(await drepo.GetAllDoctorsAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await this.drepo.FindDocotrAsync(id));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await this.drepo.FindDocotrAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult>Edit(string apellido,string especialidad, int salario,int cod_hos,int iddoc)
        {
            await this.drepo.UpdateDoctorAsync(apellido, salario, especialidad, cod_hos, iddoc);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.drepo.DeleteDoctorAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doctor doc)
        {
            await this.drepo.InsertDoctorAsync(doc.Apellido,doc.Salario,doc.Especialidad,doc.HospitalCod);
            return RedirectToAction("Index");
        }
    }
}
