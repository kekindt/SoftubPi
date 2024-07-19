using Microsoft.AspNetCore.Mvc;
using softubWeb.Models;
using softubWeb.Models.Views;

namespace softubWeb.Controllers;

public class ControlController : Controller
{
    //ConfigContext config = null;

    //public ControlController()
    //{
    //    if(config == null)
    //        config = new ConfigContext();
    //}

    public IActionResult Index()
    {
        ControlIndex model = GetCurrentValues();
        return View(model);
    }

    public IActionResult SaveValues(ControlIndex controlIndex)
    {
        using (var db = new ConfigContext())
        {
            var dbValues = db.ConfigValues.ToList();
            var dbValue = dbValues.FirstOrDefault();

            dbValue.TargetTemp = controlIndex.TargetTemp;
            dbValue.LightsOn = controlIndex.LightsOn == true ? 1 : 0;
            dbValue.JetsOn = controlIndex.JetsOn == true ? 1 : 0;

            db.ConfigValues.Update(dbValue);
            db.SaveChanges();

            //ControlIndex model = GetCurrentValues();
            return RedirectToAction("Index");
        }
    }

    private ControlIndex GetCurrentValues()
    {
        using (var db = new ConfigContext())
        {
            var dbConfig = db.ConfigValues.ToList();
            ControlIndex model = new ControlIndex()
            {
                TargetTemp = dbConfig.First().TargetTemp.Value,
                LastTemp = dbConfig.First().LastTemp.Value,
                LightsOn = dbConfig.First().LightsOn == 1 ? true : false,
                JetsOn = dbConfig.First().JetsOn == 1 ? true : false,
            };
            return model;
        }
            
    }

}