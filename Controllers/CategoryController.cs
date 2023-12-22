using MeatRiderss.Data;
using MeatRiderss.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeatRiderss.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category>  objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()//by default is get action
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString()) 
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
           
            //custom validation
            if (ModelState.IsValid)//validation in caterogry.cs
            {
                _db.Categories.Add(obj);//add category
                _db.SaveChanges(); // redirect to categoryindex view
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            return View();
           
        }
        public IActionResult Edit(int? id)// id is nullable
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id); //find the category using id
            //other ways to retreive edit data using id
            Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id); //find the category using id
            Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault() ;//find the category using id
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        //how is it updated? based on the id that is populdated in the obj parameter, it will automatically update it in db
        public IActionResult Edit(Category obj)
        {
            
            if (ModelState.IsValid)//validation in category.cs
            {
                _db.Categories.Update(obj);//add category
                _db.SaveChanges(); // redirect to categoryindex view
                TempData["success"] = "Category edited successfully!";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)// id is nullable
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id); //find the category using id
            //other ways to retreive edit data using id
            Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id); //find the category using id
            Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();//find the category using id
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        
        public IActionResult DeletePOST(int? id)//parameters will be the same for get and post action hence need to be different name
        {

            Category? obj = _db.Categories.Find(id);
            if ( obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges(); // redirect to categoryindex view
            TempData["success"] = "Category deleted successfully!";

            return RedirectToAction("Index");
           

        }
    }
}
