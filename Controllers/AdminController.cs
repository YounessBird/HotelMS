using System.Diagnostics;
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
    public IActionResult Users()
    {
        var adminUserVM = fakegendata();


        return View(adminUserVM);
    }

    [HttpPost]
    public async Task<IActionResult> Users(User user)
    {
        var adminUserVM = fakegendata();
        adminUserVM.User = user;

        if (ModelState.IsValid)
        {
            try
            {
                await _context.UserTb.AddAsync(user);
                await _context.SaveChangesAsync();
                return View(adminUserVM);
            }
            catch (Exception e)
            {
                //Todo: log the Error to a Logger 
                ModelState.AddModelError(string.Empty, e.Message);
                return View(adminUserVM);
            }
        }
        else
        {
            return View(adminUserVM);
        }
    }

    public AdminUserVM fakegendata()
    {
        List<Room> rooomList = new List<Room>()
        {
           new Room{Id=1,RName="Lux",RCategory= new Category{Id=1,CatName="LUX"},RLocation="London",RCost=100},
           new Room{Id=2,RName="Lux2",RCategory= new Category{Id=2,CatName="LUX2"},RLocation="London",RCost=100},
    };


        AdminUserVM AdminUserVM = new AdminUserVM
        {
            Category = null,
            RoomList = rooomList,
        };
        return AdminUserVM;
    }
}