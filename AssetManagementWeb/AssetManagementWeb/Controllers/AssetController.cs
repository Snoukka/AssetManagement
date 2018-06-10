﻿using AssetManagementWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using AssetManagementWeb.Models;
using AssetManagementWeb.Database;
using System.Globalization;

namespace AssetManagementWeb.Controllers
{
    public class AssetController : Controller
    {
        // GET: Asset
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }


        public ActionResult List()
        {
            List<LocatedAssetsViewModel> model = new List<LocatedAssetsViewModel>();

            AssetManagementEntities entities = new AssetManagementEntities();
            try
            {
                List<AssetLocation1> assets = entities.AssetLocations1.ToList();

                //muodostetaan näkymämalli tietokannan rivien pohjalta
                CultureInfo fiFI = new CultureInfo("fi-FI");
                foreach (AssetLocation1 asset in assets)
                {
                    LocatedAssetsViewModel view = new LocatedAssetsViewModel();
                    view.id = asset.Id;
                    view.LocationCode = asset.AssetLocation.Code;
                    view.LocationName = asset.AssetLocation.Name;
                    view.AssetCode = asset.Asset.Code;
                    view.AssetName = asset.Asset.Type + ": " +asset.Asset.Model;
                    view.LastSeen = asset.LastSeen.Value.ToString(fiFI);

                    model.Add(view);
                }
            }
            finally
            {
                entities.Dispose();
            }
                return View(model);
        }

        public ActionResult ListJson()
        {
            List<LocatedAssetsViewModel> model = new List<LocatedAssetsViewModel>();

            AssetManagementEntities entities = new AssetManagementEntities();
            try
            {
                List<AssetLocation1> assets = entities.AssetLocations1.ToList();

                //muodostetaan näkymämalli tietokannan rivien pohjalta
                CultureInfo fiFI = new CultureInfo("fi-FI");
                foreach (AssetLocation1 asset in assets)
                {
                    LocatedAssetsViewModel view = new LocatedAssetsViewModel();
                    view.id = asset.Id;
                    view.LocationCode = asset.AssetLocation.Code;
                    view.LocationName = asset.AssetLocation.Name;
                    view.AssetCode = asset.Asset.Code;
                    view.AssetName = asset.Asset.Type + ": " + asset.Asset.Model;
                    view.LastSeen = asset.LastSeen.Value.ToString(fiFI);

                    model.Add(view);
                }
            }
            finally
            {
                entities.Dispose();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult  AssignLocation()
        {

            string json = Request.InputStream.ReadToEnd();
            AssignLocationModel imputData = 
                JsonConvert.DeserializeObject< AssignLocationModel>(json);

            bool success = false;
            string error = "";
            AssetManagementEntities entities = new AssetManagementEntities();
            try
            {
                //Haetaan ensin paikan id-numerokoodin perusteella
                int locationId = (from l in entities.AssetLocations
                              where l.Code == imputData.LocationCode
                              select l.Id).FirstOrDefault();

                //Haetaan ensin paikan id-numerokoodin perusteella
                int assetId = (from a in entities.Assets
                                  where a.Code == imputData.AssetCode
                                  select a.Id).FirstOrDefault();
                if ((locationId > 0) && (assetId > 0))
                {
                    //tallennetaan uusi rivi aikaleiman kanssa kantaan
                    AssetLocation1 newEntry = new AssetLocation1();
                    newEntry.LocationId = locationId;
                    newEntry.AssetId = assetId;
                    newEntry.LastSeen = DateTime.Now;

                    entities.AssetLocations1.Add(newEntry);
                    entities.SaveChanges();

                    success = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.GetType().Name + ": " + ex.Message;
            }
            finally
            {
                entities.Dispose();
            }

            // palautetaan JSON-muotoinen tulos kutsujalle
            var result = new { success = success, error = error };
            return Json(result);
        }


    }
}
