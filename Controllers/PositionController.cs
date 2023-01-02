using NguyenThiQuynhTrangBTH2.Data;
using NguyenThiQuynhTrangBTH2.Models;
using NguyenThiQuynhTrangBTH2.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NguyenThiQuynhTrangBTH2.Controllers
{
    public class PositionController : Controller
    {
        //khai bao DBcontext de lam viec voi Database
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
        public PositionController (ApplicationDbContext context)
        {
            _context = context;
        }
        // Action tra ve View hien thi danh sach sinh vien
        public async Task<IActionResult> Index()
        {
            var model = await _context.Position.ToListAsync();
            return View(model);
        }
        //Action tra ve View them moi danh sach sinh vien
        public IActionResult Create()
        {
            return View();
        }
        //Action xu ly du lieu sinh vien gui len tu view va luu vao Database
        [HttpPost]
        public async Task<IActionResult> Create(Position pst)
        {
            if(ModelState.IsValid)
            {
                _context.Add(pst);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(pst);
        }
         public IActionResult Upload()
         {
            return View();
         }
       //POST :Position/Upload
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
                        //create a new Position object 
                        var pst = new Position();
                        //set values for attributes
                        pst.PositionID = dt.Rows[i][0].ToString();
                        pst.PositionName = dt.Rows[i][1].ToString();
                        //add object to Context
                        _context.Position.Add(pst);
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
       //GET: Position/Edit/5
     public async Task<IActionResult> Edit(string id)
     {
        if(id == null)
        {
            return View("NotFound");
        }
        var position = await _context.Position.FindAsync(id);
        if (position == null)
        {
            return View("NotFound");
        }
        return View (position);
     }
       //POST :Position/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("PositionID,PositionName")] Position pst)
    {
        if (id !=pst.PositionID)
        {
            return View("NotFound");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pst);
                await _context.SaveChangesAsync();
            }
        catch (DbUpdateConcurrencyException)
        {
            if (!PositionExists(pst.PositionID))
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
        return View(pst);
    }
    //GET:Position/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var pst = await _context.Position
         .FirstOrDefaultAsync(m => m.PositionID == id);
        if (pst == null)
        {
            return View("NotFound");
        }
        return View(pst);
    }
    //POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var pst = await _context.Position.FindAsync(id);
        _context.Position.Remove(pst!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool PositionExists(string id)
    {
        return _context.Position.Any(e => e.PositionID == id);
    }
    }
}