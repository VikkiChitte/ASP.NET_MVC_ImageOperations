using DAL.Entities;
using Lab_ImageOperations.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Lab_ImageOperations.Controllers
{
    public class EmployeeController : Controller
    {
        AppDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EmployeeController(AppDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db =  db;
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<EmployeeViewModel> EmpDeptViewModellist = new List<EmployeeViewModel>();
            var empList = (from e in _db.Employees
                           select new
                           {
                               e.EmployeeId,
                               e.Name,
                               e.Address,
                               e.ImagePath
                           }).ToList();
            foreach(var item in empList)
            {
                EmployeeViewModel objedvm = new EmployeeViewModel();
                
                objedvm.Name = item.Name;
                objedvm.Address = item.Address;
                objedvm.EmployeeId = item.EmployeeId;
                objedvm.ImagePath = item.ImagePath;
                EmpDeptViewModellist.Add(objedvm);
            }

            return View(EmpDeptViewModellist);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel model)
        {
            ModelState.Remove("EmployeeId");
            if(ModelState.IsValid)
            {
                string filePath = UploadedFile(model);
                Employee employee = new Employee
                {
                    Name = model.Name,
                    Address = model.Address,
                    ImagePath = filePath
                };
                _db.Add(employee);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        private string UploadedFile(EmployeeViewModel model) 
        {
            string filePath = null; 
            if (model.Image != null) 
            { 
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                filePath = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string path = Path.Combine(uploadsFolder, filePath);
                using (var fileStream = new FileStream(path, FileMode.Create))
                { 
                    model.Image.CopyTo(fileStream);
                }
            }
            return filePath;
        }

        public IActionResult Edit(int id)
        {
            Employee data = _db.Employees.Find(id);
            EmployeeViewModel model = new EmployeeViewModel();
            if(model != null)
            {
                model.Name = data.Name;
                model.Address = data.Address;
                model.EmployeeId = data.EmployeeId;
                model.ImagePath = data.ImagePath;
            }
            return View("Create", model);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    EmployeeId = model.EmployeeId,
                    Name = model.Name,
                    Address = model.Address
                };
                if(model.Image != null)
                {
                    string filePath = UploadedFile(model);
                    employee.ImagePath = filePath;
                }
                _db.Employees.Update(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

        public IActionResult Delete(int id)
        {
            Employee model = _db.Employees.Find(id);
            if(model != null)
            {
                _db.Employees.Remove(model);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
