using Microsoft.AspNetCore.Mvc;
using HotelMS.Models;
using HotelMS.Data;
using HotelMS.ViewModel;
using Microsoft.EntityFrameworkCore;
using HotelMS.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelMS.Controllers;

public class AdminController : Controller
{
    const string SessionKeyCat = "category";
    const string SessionKeyRoom = "room";
    const string SessionKeyUser = "user";
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Rooms Controller
    [HttpGet]
    public async Task<IActionResult> Rooms()
    {
        var adminUserVM = new AdminUserVM();
        try
        {
            var roomsList = await _context.RoomTb.ToListAsync();
            var catList = await _context.CategoryTb.ToListAsync();
            adminUserVM.RoomList = roomsList;
            adminUserVM.CatListItem = catList.ConvertAll(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.CatName
            });

            HttpContext.Session.Set<List<Room>>(SessionKeyRoom, roomsList);

            return View(adminUserVM);
        }
        catch (Exception e)

        {
            ModelState.AddModelError(string.Empty, "Failure to load data");
            var roomsList = HttpContext.Session.Get<List<Category>>(SessionKeyRoom);
            // deal with normal scenarios
            if (roomsList == null)
            {
                adminUserVM.RoomList = new List<Room>();
            }
            return View(adminUserVM);
        }
    }
    [HttpPost]
    public async Task<IActionResult> SaveRoom(Room room)
    {
        Status st = (Status)Int64.Parse(room.Status);
        room.Status = st.ToString();
        var adminUserVM = new AdminUserVM();
        var roomsList = HttpContext.Session.Get<List<Room>>(SessionKeyRoom);

        adminUserVM.RoomList = roomsList ?? new List<Room>();

        if (ModelState.IsValid)
        {
            try
            {

                await _context.RoomTb.AddAsync(room);
                await _context.SaveChangesAsync();
                TempData["success"] = "saved";
                return RedirectToAction("Rooms");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "Failure to register room please try again");

                //Todo: log the Error to a Logger 
                return View("Rooms", adminUserVM);
            }
        }
        else
        {
            return View("Rooms", adminUserVM);
        }
    }
    [HttpPost]
    public IActionResult EditRoom(Room room)
    {
        var adminUserVM = new AdminUserVM();
        var roomsList = HttpContext.Session.Get<List<Room>>(SessionKeyRoom);

        adminUserVM.RoomList = roomsList ?? new List<Room>();

        if (ModelState.IsValid)
        {
            try
            {
                var Entityroom = _context.RoomTb.Attach(room);
                Entityroom.State = EntityState.Modified;
                _context.SaveChanges();
                TempData["success"] = "edited";
                return RedirectToAction("Rooms");
            }

            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Failure to edit category please try again");

                //Todo: log the Error to a Logger 
                return View("Rooms", adminUserVM);
            }
        }
        else
        {
            return View("Rooms", adminUserVM);
        }
    }
    [HttpPost]
    public IActionResult DeleteRoom(Room room)
    {
        var adminUserVM = new AdminUserVM();
        var roomList = HttpContext.Session.Get<List<Room>>(SessionKeyRoom);

        adminUserVM.RoomList = roomList ?? new List<Room>();

        if (ModelState.IsValid)
        {
            try
            {
                var Entityroom = _context.RoomTb.Find(room.Id);
                if (Entityroom != null)
                {
                    _context.RoomTb.Remove(Entityroom);
                    _context.SaveChanges();
                    TempData["success"] = "deleted";
                    return RedirectToAction("Rooms");
                }
                else
                {
                    return View("Rooms", adminUserVM);
                }

            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, "Failure to edit category please try again");

                //Todo: log the Error to a Logger 
                return View("Rooms", adminUserVM);
            }
        }
        else
        {
            return View("Rooms", adminUserVM);
        }
    }


    //Categories Actions

    public async Task<IActionResult> Categories()
    {
        var adminUserVM = new AdminUserVM();
        try
        {
            var catList = await _context.CategoryTb.ToListAsync();
            adminUserVM.CatList = catList;

            HttpContext.Session.Set<List<Category>>(SessionKeyCat, catList);

            return View(adminUserVM);
        }
        catch (Exception e)

        {
            ModelState.AddModelError(string.Empty, "Failure to load data");
            var catList = HttpContext.Session.Get<List<Category>>(SessionKeyCat);
            // deal with normal scenarios
            if (catList == null)
            {
                adminUserVM.CatList = new List<Category>();
            }
            return View(adminUserVM);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveCategories(Category category)
    {
        var adminUserVM = new AdminUserVM();
        var catList = HttpContext.Session.Get<List<Category>>(SessionKeyCat);

        adminUserVM.CatList = catList ?? new List<Category>();

        if (ModelState.IsValid)
        {
            try
            {
                await _context.CategoryTb.AddAsync(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "saved";
                return RedirectToAction("Categories");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "Failure to register category please try again");

                //Todo: log the Error to a Logger 
                return View("Categories", adminUserVM);
            }
        }
        else
        {
            return View("Categories", adminUserVM);
        }
    }
    [HttpPost]
    public IActionResult EditCategories(Category category)
    {
        var adminUserVM = new AdminUserVM();
        var catList = HttpContext.Session.Get<List<Category>>(SessionKeyCat);

        adminUserVM.CatList = catList ?? new List<Category>();

        if (ModelState.IsValid)
        {
            try
            {
                var cat = _context.CategoryTb.Attach(category);
                cat.State = EntityState.Modified;
                _context.SaveChanges();
                TempData["success"] = "edited";
                return RedirectToAction("Categories");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);

                ModelState.AddModelError(string.Empty, "Failure to edit category please try again");

                //Todo: log the Error to a Logger 
                return View("Categories", adminUserVM);
            }
        }
        else
        {
            return View("Categories", adminUserVM);
        }
    }

    [HttpPost]
    public IActionResult DeleteCategories(Category category)
    {
        var adminUserVM = new AdminUserVM();
        var catList = HttpContext.Session.Get<List<Category>>(SessionKeyCat);

        adminUserVM.CatList = catList ?? new List<Category>();

        if (ModelState.IsValid)
        {
            try
            {
                var cat = _context.CategoryTb.Find(category.Id);
                if (cat != null)
                {
                    _context.CategoryTb.Remove(cat);
                    _context.SaveChanges();
                    TempData["success"] = "deleted";
                    return RedirectToAction("Categories");
                }
                else
                {
                    return View("Categories", adminUserVM);
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Failure to edit category please try again");
                //Todo: log the Error to a Logger 
                return View("Categories", adminUserVM);
            }
        }
        else
        {
            return View("Categories", adminUserVM);
        }
    }

    //Users Controller

    [HttpGet]
    [ImportModelState]
    public async Task<IActionResult> Users()
    {
        var adminUserVM = new AdminUserVM();
        try
        {
            var usersList = await _context.UserTb.ToListAsync();
            adminUserVM.UserList = usersList;
            HttpContext.Session.Set<List<User>>(SessionKeyUser, usersList);
            return View(adminUserVM);
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, "Failure to load data");
            var rList = HttpContext.Session.Get<List<User>>(SessionKeyUser);
            if (rList == null)
            {
                adminUserVM.UserList = new List<User>();
            }
            return View(adminUserVM);
        }
    }

    [HttpPost]
    [ExportModelState]
    public async Task<IActionResult> SaveUser(User user)
    {
        Console.WriteLine("user", user);
        var adminUserVM = new AdminUserVM();
        var usersList = HttpContext.Session.Get<List<User>>(SessionKeyUser);
        adminUserVM.UserList = usersList ?? new List<User>();

        if (ModelState.IsValid)
        {
            Gender st = (Gender)long.Parse(user.UGender);
            user.UGender = st.ToString();
            try
            {
                Console.WriteLine("triggered");
                await _context.UserTb.AddAsync(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "saved";
                return RedirectToAction("Users");
            }
            catch (Exception e)
            {
                //Todo: log the Error to a Logger 
                if (e.Message.Contains("duplicate key value violates unique constraint"))
                {
                    ModelState.AddModelError(string.Empty, "Hit the edit button to edit the form");
                }
                ModelState.AddModelError(string.Empty, "Failure to register room please try again");

                //Todo: log the Error to a Logger 
                return RedirectToAction("Users", adminUserVM);
            }
        }
        else
        {
            return RedirectToAction("Users", adminUserVM);
        }
    }

    [HttpPost]
    public IActionResult EditUser(User user)
    {

        var adminUserVM = new AdminUserVM();
        var usersList = HttpContext.Session.Get<List<User>>(SessionKeyUser);

        adminUserVM.UserList = usersList ?? new List<User>();

        if (ModelState.IsValid)
        {
            Status st = (Status)Int64.Parse(user.UGender);
            user.UGender = st.ToString();
            try
            {
                var Entityroom = _context.UserTb.Attach(user);
                Entityroom.State = EntityState.Modified;
                _context.SaveChanges();
                TempData["success"] = "edited";
                return RedirectToAction("Users");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "Failure to edit category please try again");
                //Todo: log the Error to a Logger 
                return View("Users", adminUserVM);
            }
        }
        else
        {
            return View("Users", adminUserVM);
        }
    }

    [HttpPost]
    public IActionResult DeleteUser(User user)
    {
        Gender st = (Gender)Int64.Parse(user.UGender);
        user.UGender = st.ToString();

        var adminUserVM = new AdminUserVM();
        var usersList = HttpContext.Session.Get<List<User>>(SessionKeyUser);

        adminUserVM.UserList = usersList ?? new List<User>();

        if (ModelState.IsValid)
        {
            try
            {
                var Entityroom = _context.UserTb.Find(user.Id);
                if (Entityroom != null)
                {
                    _context.UserTb.Remove(Entityroom);
                    _context.SaveChanges();
                    TempData["success"] = "deleted";
                    return RedirectToAction("Users");
                }
                else
                {
                    return View("Users", adminUserVM);
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Failure to edit category please try again");
                //Todo: log the Error to a Logger 
                return View("Users", adminUserVM);
            }
        }
        else
        {
            return View("Users", adminUserVM);
        }
    }
}