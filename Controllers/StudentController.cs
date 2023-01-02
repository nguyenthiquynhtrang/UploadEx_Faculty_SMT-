using NguyenThiQuynhTrangBTH2.Data;
using NguyenThiQuynhTrangBTH2.Models;
using NguyenThiQuynhTrangBTH2.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NguyenThiQuynhTrangBTH2.Controllers
{
    public class StudentController : Controller
    {
        //khai bao DBcontext de lam viec voi Database
        private readonly ApplicationDbContext _context;
        private StringProcess strPro = new StringProcess();
        private ExcelProcess _excelProcess = new ExcelProcess();
        public StudentController (ApplicationDbContext context)
        {
            _context = context;
        }
        // Action tra ve View hien thi danh sach sinh vien
        public async Task<IActionResult> Index()
        {
            var model = await _context.Students.ToListAsync();
            return View(model);
        }
        //Action tra ve View them moi danh sach sinh vien
        public IActionResult Create()
        {
            var newStudentID ="STD0001";
            var countStudent = _context.Students.Count();
            if(countStudent>0)
            {
                var studentID = _context.Students.OrderByDescending(m => m.StudentID).First().StudentID;
                //sinh ma tu dong
                newStudentID = strPro.AutoGenerateCode(studentID);
            }
            ViewBag.newID = newStudentID;
            ViewData["FacultyID"] = new SelectList(_context.Faculty, "FacultyID", "FacultyID");
            return View();
        }
        //Action xu ly du lieu sinh vien gui len tu view va luu vao Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,StudentName,FacultyID")] Student std)
        {
            if(ModelState.IsValid)
            {
                _context.Add(std);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
           ViewData["FacultyID"] = new SelectList(_context.Faculty, "FacultyID", "FacultyID", std.FacultyID);
            return View(std);
        }
         public IActionResult Upload()
         {
            return View();
         }
       //POST :Student/Upload
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
                        //create a new Student object 
                        var std = new Student();
                        //set values for attributes
                        std.StudentID = dt.Rows[i][0].ToString();
                        std.StudentName = dt.Rows[i][1].ToString();
                        //add object to Context
                        _context.Students.Add(std);
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
       //GET: Student/Edit/5
     public async Task<IActionResult> Edit(string id)
     {
        if(id == null)
        {
            return View("NotFound");
        }
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return View("NotFound");
        }
        return View(student);
     }
       //POST :Student/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("StudentID,StudentName")] Student std)
    {
        if (id !=std.StudentID)
        {
            return View("NotFound");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(std);
                await _context.SaveChangesAsync();
            }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(std.StudentID))
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
        return View(std);
    }
    //GET: Student/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var std = await _context.Students
         .FirstOrDefaultAsync(m => m.StudentID == id);
        if (std == null)
        {
            return View("NotFound");
        }
        return View(std);
    }
    //POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var std = await _context.Students.FindAsync(id);
        _context.Students.Remove(std!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool StudentExists(string id)
    {
        return _context.Students.Any(e => e.StudentID == id);
    }
    }
}