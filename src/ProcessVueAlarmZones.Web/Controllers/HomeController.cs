using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProcessVueAlarmZones.Application.Interface;
using ProcessVueAlarmZones.Web.Models;

namespace ProcessVueAlarmZones.Web.Controllers;

public class HomeController(IEemuaZoneClassifier classifier) : Controller
{
    private readonly IEemuaZoneClassifier _classifier = classifier;

    [HttpGet]
    public IActionResult Index() => View(new IndexViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(IndexViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        vm.Result = _classifier.Classify(vm.Input.AverageAlarmRate, vm.Input.PercentOutsideTarget);
        return View(vm);
    }
}
