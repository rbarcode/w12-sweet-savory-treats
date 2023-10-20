using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Bakery.Models;

namespace Bakery.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, BakeryContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    // [AllowAnonymous]
    // public ActionResult Index()
    // {
    //   return View(_db.Treats.ToList());
    // }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
            .Include(treat => treat.JoinEntity)
            .ThenInclude(join => join.Flavor)
            .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult Create()
    {
      Treat treat = new();
      ViewBag.Type = new SelectList(treat.availableTypes);
      return View();
    }

    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index", "Home");
    }

    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.Flavors = _db.Flavors.ToList();
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      #nullable disable
      if (joinEntity == null && flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = flavorId, TreatId = treat.TreatId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId});
    }

    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      ViewBag.Type = new SelectList(thisTreat.availableTypes);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == treat.TreatId);
      return RedirectToAction("Details", new { id = treat.TreatId});
    }

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
      _db.FlavorTreats.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index", "Home");
    }
  }
}