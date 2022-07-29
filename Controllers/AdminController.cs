using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using HotelMS.Models;
using HotelMS.Data;
using HotelMS.ViewModel;
using Microsoft.EntityFrameworkCore;
using HotelMS.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HotelMS.Controllers;


public class AdminController : Controller
{
    const string SessionKeyCat = "category";
    const string SessionKeyRoom = "room";
    const string SessionKeyUser = "user";
    const string SessionKeyRole = "role";

    private readonly ApplicationDbContext _context;

    private RoleManager<AppRole> _roleManager;
    private UserManager<AppUser> _userManager;
    public AdminController(ApplicationDbContext context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    //Rooms Controller
    [HttpGet]
    [ImportModelState]
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
    [ExportModelState]

    public async Task<IActionResult> SaveRoom(Room room)
    {

        var adminUserVM = new AdminUserVM();
        var roomsList = HttpContext.Session.Get<List<Room>>(SessionKeyRoom);
        adminUserVM.RoomList = roomsList ?? new List<Room>();

        if (ModelState.IsValid)
        {
            Status st = (Status)long.Parse(room.Status);
            room.Status = st.ToString();
            try
            {

                await _context.RoomTb.AddAsync(room);
                await _context.SaveChangesAsync();
                TempData["success"] = "saved";
                return RedirectToAction("Rooms");
            }

            catch (Exception e)
            {
                if (e.Message.Contains("duplicate key value violates unique constraint"))
                {
                    ModelState.AddModelError(string.Empty, "Hit the edit button to edit the form");
                }
                ModelState.AddModelError(string.Empty, "Failure to register room please try again");

                //Todo: log the Error to a Logger 
                return RedirectToAction("Rooms", adminUserVM);
            }
        }
        else
        {
            return RedirectToAction("Rooms", adminUserVM);
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
            Status st = (Status)Int64.Parse(room.Status);
            room.Status = st.ToString();
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
        Status st = (Status)Int64.Parse(room.Status);
        room.Status = st.ToString();

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
        // get all roles per users 
        // display them in table with checkbox 
        // use viewmodel property RolesInUsers Dictionary<string, List<IdentityRole>> the key is user ID List is roles of the user
        // use _userManager to get roles per user 
        // _userManager.isInRoleAsync(user, role.name) ans then set the key isSelected to bool 

        var adminUserVM = new AdminUserVM();


        try
        {
            adminUserVM.UserList = _userManager.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role).AsNoTracking().Select(o => new UserDetailsDto()
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Email = o.Email,
                            Gender = o.Gender,
                            Phone = o.PhoneNumber,
                            Password = o.GenericPassword,
                            RoleNameList = o.UserRoles.Select(o => o.Role.Name).ToList(),
                        }).ToList();
            HttpContext.Session.Set<List<UserDetailsDto>>(SessionKeyUser, adminUserVM.UserList);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ModelState.AddModelError(string.Empty, "Failure to load data");
            var rList = HttpContext.Session.Get<List<UserDetailsDto>>(SessionKeyUser);
            if (rList == null)
            {
                adminUserVM.UserList = new List<UserDetailsDto>();
            }
        }
        return View(adminUserVM);
    }

    [HttpPost]
    [ExportModelState]
    public async Task<IActionResult> SaveUser(User user)
    {

        // we have to to implement adding user to a role 
        // get the role name and role ID from select option 
        // add users with their passord to db 

        var adminUserVM = new AdminUserVM();
        Gender st = (Gender)long.Parse(user.UGender);
        user.UGender = st.ToString();

        var appuser = new AppUser()
        {
            UserName = user.UEmail,
            Name = user.UName,
            PhoneNumber = user.UPhone,
            Gender = user.UGender,
            Email = user.UEmail,
            GenericPassword = user.UPassword, // Note this may have to change to conceal password as at the moment it is used to store a one time password for users 
        };
        var usersList = HttpContext.Session.Get<List<UserDetailsDto>>(SessionKeyUser);
        adminUserVM.UserList = usersList ?? new List<UserDetailsDto>();

        if (ModelState.IsValid)
        {
            Console.WriteLine("Passed");

            try
            {
                // await _context.UserTb.AddAsync(user);
                // await _context.SaveChangesAsync();
                var result = await _userManager.CreateAsync(appuser, user.UPassword);
                // Console.WriteLine($"---------Role {user.Role.Name}");
                // if (String.IsNullOrEmpty(user.Role.Name))
                // {
                //     Console.WriteLine("should not trigg if null");
                //     await _userManager.AddToRoleAsync(appuser, user.Role.Name);
                // }
                if (result.Succeeded)
                {
                    TempData["success"] = "saved";
                    return RedirectToAction("Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return RedirectToAction("Users");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($" ---- Exception {e}");
                //Todo: log the Error to a Logger 
                if (e.Message.Contains("duplicate key value violates unique constraint"))
                {
                    ModelState.AddModelError(string.Empty, "Hit the edit button to edit the form");
                }
                ModelState.AddModelError(string.Empty, "Failure to register user please try again");

                //Todo: log the Error to a Logger 
                return RedirectToAction("Users", adminUserVM);
            }
        }
        else
        {
            Console.WriteLine("Error");
            return RedirectToAction("Users", adminUserVM);
        }
        return View("Users");
    }

    [HttpPost]
    [ExportModelState]
    public async Task<IActionResult> EditUser(EditUser EditedUser)
    {

        var adminUserVM = new AdminUserVM();

        // this might be unnecessary 
        var usersList = HttpContext.Session.Get<List<AppUser>>(SessionKeyUser);

        adminUserVM.UserList = usersList ?? new List<AppUser>();

        if (ModelState.IsValid)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(EditedUser.Id);
                if (user == null)
                {
                    ModelState.AddModelError("", "User can not be found ");
                    return View("Users", adminUserVM);
                }
                else
                {


                    Status st = (Status)Int64.Parse(EditedUser.UGender);
                    EditedUser.UGender = st.ToString();
                    user.Name = EditedUser.UName;
                    user.Email = EditedUser.UEmail;
                    user.Gender = EditedUser.UGender;
                    user.PhoneNumber = EditedUser.UPhone;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "edited";
                        return RedirectToAction("Users");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return RedirectToAction("Users");
                    }
                }
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
        return View("Users");
    }

    // [HttpPost]
    // public IActionResult DeleteUser(User user)
    // {
    //     Gender st = (Gender)Int64.Parse(user.UGender);
    //     user.UGender = st.ToString();

    //     var adminUserVM = new AdminUserVM();
    //     var usersList = HttpContext.Session.Get<List<User>>(SessionKeyUser);

    //     adminUserVM.UserList = usersList ?? new List<User>();

    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             var Entityroom = _context.UserTb.Find(user.Id);
    //             if (Entityroom != null)
    //             {
    //                 _context.UserTb.Remove(Entityroom);
    //                 _context.SaveChanges();
    //                 TempData["success"] = "deleted";
    //                 return RedirectToAction("Users");
    //             }
    //             else
    //             {
    //                 return View("Users", adminUserVM);
    //             }

    //         }
    //         catch (Exception e)
    //         {
    //             ModelState.AddModelError(string.Empty, "Failure to edit category please try again");
    //             //Todo: log the Error to a Logger 
    //             return View("Users", adminUserVM);
    //         }
    //     }
    //     else
    //     {
    //         return View("Users", adminUserVM);
    //     }
    // }
    //[HttpPost]
    // public IActionResult UpdateUserRole(UserRoles userRoles)
    // {



    //     Gender st = (Gender)Int64.Parse(user.UGender);
    //     user.UGender = st.ToString();

    //     var adminUserVM = new AdminUserVM();
    //     var usersList = HttpContext.Session.Get<List<User>>(SessionKeyUser);

    //     adminUserVM.UserList = usersList ?? new List<User>();

    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             var Entityroom = _context.UserTb.Find(user.Id);
    //             if (Entityroom != null)
    //             {
    //                 _context.UserTb.Remove(Entityroom);
    //                 _context.SaveChanges();
    //                 TempData["success"] = "deleted";
    //                 return RedirectToAction("Users");
    //             }
    //             else
    //             {
    //                 return View("Users", adminUserVM);
    //             }

    //         }
    //         catch (Exception e)
    //         {
    //             ModelState.AddModelError(string.Empty, "Failure to edit category please try again");
    //             //Todo: log the Error to a Logger 
    //             return View("Users", adminUserVM);
    //         }
    //     }
    //     else
    //     {
    //         return View("Users", adminUserVM);
    //     }
    // }

    [HttpGet]
    [ImportModelState]
    public async Task<IActionResult> Roles()
    {
        AccountVM accountVM = new();
        try
        {
            var roles = _roleManager.Roles.ToList();

            if (roles.Count > 0)
            {

                foreach (var roleObj in roles)
                {
                    var usersList = await _userManager.GetUsersInRoleAsync(roleObj.Name);
                    var UserNameList = usersList.Select(o => o.UserName).ToList();
                    accountVM.UserInRole.Add(roleObj.Id, UserNameList);
                }
                accountVM.RolesTbList = roles;
                HttpContext.Session.Set<List<AppRole>>(SessionKeyRole, roles);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return View(accountVM);
    }


    [ExportModelState]
    [HttpPost]
    public async Task<IActionResult> CreateRole(RoleVm Role)
    {
        var accountVM = new AccountVM();
        var rolesList = HttpContext.Session.Get<List<AppRole>>(SessionKeyRole);
        accountVM.RolesTbList = rolesList ?? new List<AppRole>();

        if (ModelState.IsValid)
        {
            AppRole appRole = new()
            {
                Name = Role.RName
            };
            try
            {
                IdentityResult result = await _roleManager.CreateAsync(appRole);
                if (result.Succeeded)
                {
                    TempData["success"] = "saved";
                    return RedirectToAction("Roles");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Failed to create Role");
                //Todo log errors
                Console.WriteLine(e);

                return RedirectToAction("Roles");
            }
        }
        return View("Roles", accountVM);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(RoleVm Role)
    {
        var accountVM = new AccountVM();
        var rolesList = HttpContext.Session.Get<List<AppRole>>(SessionKeyRole);
        accountVM.RolesTbList = rolesList ?? new List<AppRole>();

        if (ModelState.IsValid)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(Role.Id);
                if (role == null)
                {
                    ModelState.AddModelError("", $"Role with Id {Role.Id} can not be found");
                    return View("Roles", accountVM);
                }

                role.Name = Role.RName;

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["success"] = "edited";
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("Roles", accountVM);
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "Failure to edit Role please try again");
                //Todo: log the Error to a Logger 
                return View("Roles", accountVM);
            }
        }
        else
        {
            return View("Roles", accountVM);
        }
    }
}
