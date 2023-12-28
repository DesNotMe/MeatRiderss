using Microsoft.AspNetCore.Mvc;
using MeatRiderss.Models;
using MeatRiderss.DataAccess.Repository.IRepository;
using MeatRiderss.DataAccess.Repository;


namespace MeatRiderss.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();//which object we're working on
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
                _unitOfWork.Category.Add(obj);//add category
                _unitOfWork.Save(); // redirect to categoryindex view
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id); //find the category using id
            //other ways to retreive edit data using id
            //Category? categoryFromDb1 = _categoryRepo.FirstOrDefault(u => u.Id == id); //find the category using id
            //Category? categoryFromDb2 = _categoryRepo.Where(u => u.Id == id).FirstOrDefault() ;//find the category using id
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
                _unitOfWork.Category.Update(obj);//add category
                _unitOfWork.Save(); // redirect to categoryindex view
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id); //find the category using id
            //other ways to retreive edit data using id
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id); //find the category using id
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();//find the category using id
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? id)//parameters will be the same for get and post action hence need to be different name
        {

            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save(); // redirect to categoryindex view
            TempData["success"] = "Category deleted successfully!";

            return RedirectToAction("Index");


        }
    }
}
