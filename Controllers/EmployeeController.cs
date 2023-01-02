using NguyenThiQuynhTrangBTH2.Data;
using NguyenThiQuynhTrangBTH2.Models;
using NguyenThiQuynhTrangBTH2.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NguyenThiQuynhTrangBTH2.Controllers
{
    public class EmployeeController : Controller
    {
        //khai bao DBcontext de lam viec voi Database
        private readonly ApplicationDbContext _context;
        private StringProcess strPro = new StringProcess();
        private ExcelProcess _excelProcess = new ExcelProcess();
        public EmployeeController (ApplicationDbContext context)
        {
            _context = context;
        }
        // Action tra ve View hien thi danh sach 
        public async Task<IActionResult> Index()
        {
            var model = await _context.Employees.ToListAsync();
            return View(model);
        }
        //Action tra ve View them moi danh sach 
        public IActionResult Create()
        {
            var newEmployeeID ="EMP0001";
            var countEmployee = _context.Employees.Count();
            if(countEmployee>0)
            {
                var employeeID = _context.Employees.OrderByDescending(m => m.EmployeeID).First().EmployeeID;
                //sinh ma tu dong
                newEmployeeID = strPro.AutoGenerateCode(employeeID);
            }
            ViewBag.newID = newEmployeeID;
            ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionID");
            return View();
        }
        //Action xu ly du lieu sinh vien gui len tu view va luu vao Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,EmployeeName,PositionID")] Employee emp)
        {
            if(ModelState.IsValid)
            {
                _context.Add(emp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
           ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionID", emp.PositionID);
            return View(emp);
        }
         public IActionResult Upload()
         {
            return View();
         }
       //POST :Employee/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upload(IFormFile file)
    {
        if (file!=null)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsl")
            {
                //rename file when upload to server 
                var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                var filePath= Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                var fileLocation = new FileInfo(filePath).ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    //save file to sever
                    await file.CopyToAsync(stream);
                    //read data from file and write to database 
                    var dt = _excelProcess.ExcelToDataTable(fileLocation);
                    //using for loop to read data from dt
                    for (int i =0;i < dt.Rows.Count; i++)
                    {
                        //create a new Employee object 
                        var emp = new Employee();
                        //set values for attributes
                        emp.EmployeeID = dt.Rows[i][0].ToString();
                        emp.EmployeeName = dt.Rows[i][1].ToString();
                        //add object to Context
                        _context.Employees.Add(emp);
                    }
                    //save to database
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        else
            {
                ModelState.AddModelError("","Please choose excel file to upload!");
            }
         return View();
    }
       //GET: Employee/Edit/5
     public async Task<IActionResult> Edit(string id)
     {
        if(id == null)
        {
            return View("NotFound");
        }
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return View("NotFound");
        }
        return View(employee);
     }
       //POST :Employee/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("EmployeeID,EmployeeName")] Employee emp)
    {
        if (id !=emp.EmployeeID)
        {
            return View("NotFound");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(emp);
                await _context.SaveChangesAsync();
            }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(emp.EmployeeID))
            {
                return View("NotFound");
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
        }
        return View(emp);
    }
    //GET: Employee/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var emp = await _context.Employees
         .FirstOrDefaultAsync(m => m.EmployeeID == id);
        if (emp == null)
        {
            return View("NotFound");
        }
        return View(emp);
    }
    //POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var emp = await _context.Employees.FindAsync(id);
        _context.Employees.Remove(emp!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool EmployeeExists(string id)
    {
        return _context.Employees.Any(e => e.EmployeeID == id);
    }
    }
}