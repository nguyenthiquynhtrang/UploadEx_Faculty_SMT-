using NguyenThiQuynhTrangBTH2.Data;
using NguyenThiQuynhTrangBTH2.Models;
using NguyenThiQuynhTrangBTH2.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NguyenThiQuynhTrangBTH2.Controllers
{
    public class FacultyController : Controller
    {
        //khai bao DBcontext de lam viec voi Database
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
        public FacultyController (ApplicationDbContext context)
        {
            _context = context;
        }
        // Action tra ve View hien thi danh sach sinh vien
        public async Task<IActionResult> Index()
        {
            var model = await _context.Faculty.ToListAsync();
            return View(model);
        }
        //Action tra ve View them moi danh sach sinh vien
        public IActionResult Create()
        {
            return View();
        }
        //Action xu ly du lieu sinh vien gui len tu view va luu vao Database
        [HttpPost]
        public async Task<IActionResult> Create(Faculty fac)
        {
            if(ModelState.IsValid)
            {
                _context.Add(fac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(fac);
        }
         public IActionResult Upload()
         {
            return View();
         }
       //POST :Faculty/Upload
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
                        //create a new Faculty object 
                        var fac = new Faculty();
                        //set values for attributes
                        fac.FacultyID = dt.Rows[i][0].ToString();
                        fac.FacultyName = dt.Rows[i][1].ToString();
                        //add object to Context
                        _context.Faculty.Add(fac);
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
       //GET: Faculty/Edit/5
     public async Task<IActionResult> Edit(string id)
     {
        if(id == null)
        {
            return View("NotFound");
        }
        var faculty = await _context.Faculty.FindAsync(id);
        if (faculty == null)
        {
            return View("NotFound");
        }
        return View (faculty);
     }
       //POST :Faculty/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("FacultyID,FacultyName")] Faculty fac)
    {
        if (id !=fac.FacultyID)
        {
            return View("NotFound");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(fac);
                await _context.SaveChangesAsync();
            }
        catch (DbUpdateConcurrencyException)
        {
            if (!FacultyExists(fac.FacultyID))
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
        return View(fac);
    }
    //GET:Faculty/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var fac = await _context.Faculty
         .FirstOrDefaultAsync(m => m.FacultyID == id);
        if (fac == null)
        {
            return View("NotFound");
        }
        return View(fac);
    }
    //POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var fac = await _context.Faculty.FindAsync(id);
        _context.Faculty.Remove(fac!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool FacultyExists(string id)
    {
        return _context.Faculty.Any(e => e.FacultyID == id);
    }
    }
} 