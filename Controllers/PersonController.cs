using NguyenThiQuynhTrangBTH2.Data;
using NguyenThiQuynhTrangBTH2.Models;
using NguyenThiQuynhTrangBTH2.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NguyenThiQuynhTrangBTH2.Controllers
{
    public class PersonController : Controller
    {
        //khai bao DBcontext de lam viec voi Database
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
        public PersonController (ApplicationDbContext context)
        {
            _context = context;
        }
        // Action tra ve View hien thi danh sach 
        public async Task<IActionResult> Index()
        {
            var model = await _context.Persons.ToListAsync();
            return View(model);
        }
        //Action tra ve View them moi danh sach
        public IActionResult Create()
        {
            return View();
        }
        //Action xu ly du lieu  gui len tu view va luu vao Database
        [HttpPost]
        public async Task<IActionResult> Create(Person per)
        {
            if(ModelState.IsValid)
            {
                _context.Add(per);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(per);
        }
         public IActionResult Upload()
         {
            return View();
         }
       //POST :Person/Upload
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
                        //create a new Person object 
                        var per = new Person();
                        //set values for attributes
                        per.PersonID = dt.Rows[i][0].ToString();
                       per.PersonName = dt.Rows[i][1].ToString();
                        //add object to Context
                        _context.Persons.Add(per);
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
       //GET: Person/Edit/5
     public async Task<IActionResult> Edit(string id)
     {
        if(id == null)
        {
            return View("NotFound");
        }
        var person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            return View("NotFound");
        }
        return View(person);
     }
       //POST :Person/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("PersonID,PersonName")] Person per)
    {
        if (id !=per.PersonID)
        {
            return View("NotFound");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(per);
                await _context.SaveChangesAsync();
            }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonExists(per.PersonID))
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
        return View(per);
    }
    //GET: Person/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var per = await _context.Persons
         .FirstOrDefaultAsync(m => m.PersonID == id);
        if (per == null)
        {
            return View("NotFound");
        }
        return View(per);
    }
    //POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var per = await _context.Persons.FindAsync(id);
        _context.Persons.Remove(per!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool PersonExists(string id)
    {
        return _context.Persons.Any(e => e.PersonID == id);
    }
    }
}