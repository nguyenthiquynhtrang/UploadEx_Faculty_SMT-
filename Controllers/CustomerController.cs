using NguyenThiQuynhTrangBTH2.Data;
using NguyenThiQuynhTrangBTH2.Models;
using NguyenThiQuynhTrangBTH2.Models.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NguyenThiQuynhTrangBTH2.Controllers
{
    public class CustomerController : Controller
    {
        //khai bao DBcontext de lam viec voi Database
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
        public CustomerController (ApplicationDbContext context)
        {
            _context = context;
        }
        // Action tra ve View hien thi danh sach 
        public async Task<IActionResult> Index()
        {
            var model = await _context.Customers.ToListAsync();
            return View(model);
        }
        //Action tra ve View them moi danh sach
        public IActionResult Create()
        {
            return View();
        }
        //Action xu ly du lieu  gui len tu view va luu vao Database
        [HttpPost]
        public async Task<IActionResult> Create(Customer cus)
        {
            if(ModelState.IsValid)
            {
                _context.Add(cus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(cus);
        }
         public IActionResult Upload()
         {
            return View();
         }
       //POST :Customer/Upload
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
                        //create a new Customer object 
                        var cus = new Customer();
                        //set values for attributes
                        cus.CustomerID = dt.Rows[i][0].ToString();
                        cus.CustomerName = dt.Rows[i][1].ToString();
                        //add object to Context
                        _context.Customers.Add(cus);
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
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return View("NotFound");
        }
        return View(customer);
     }
       //POST :Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("CustomerID,CustomerName")] Customer cus)
    {
        if (id !=cus.CustomerID)
        {
            return View("NotFound");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cus);
                await _context.SaveChangesAsync();
            }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(cus.CustomerID))
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
        return View(cus);
            }
    //GET: Customer/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var cus = await _context.Customers
         .FirstOrDefaultAsync(m => m.CustomerID == id);
        if (cus == null)
        {
            return View("NotFound");
        }
        return View(cus);
    }
    //POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var cus = await _context.Customers.FindAsync(id);
        _context.Customers.Remove(cus!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool CustomerExists(string id)
    {
        return _context.Customers.Any(e => e.CustomerID == id);
    }
    }
}