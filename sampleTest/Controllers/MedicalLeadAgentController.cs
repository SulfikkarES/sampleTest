using sampleTest.Models;
using sampleTest.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sampleTest.Controllers
{
    public class MedicalLeadAgentController : Controller
    {
        private readonly  IMedicalLeadAgentRepository _MedicalLeadAgentRepository;
        private readonly IAdminRepository _AdminRepository;
        ModelEntity _dbContext = new ModelEntity();


        public MedicalLeadAgentController()
        {
            _MedicalLeadAgentRepository = new MedicalLeadAgentRepository(new ModelEntity());
            _AdminRepository = new AdminRepository(new ModelEntity());
        }

        // GET: Save MedicalLeadAgent
        [HttpGet]
        [Authorize]
        public ActionResult MedicalLeadAgentFormSave()
        {
            try
            {
                if (TempData["StatusMess"] == null)
                {
                    TempData["StatusMess"] = string.Empty;
                    Session.Clear();
                }

            }
            catch (Exception ex)
            {
                _AdminRepository.SaveErrorLogs("Admin", "MedicalLeadAgentFormSave", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }

            return View(new MedicalLeadAgent());
        }


        // POST: Save MedicalLeadAgent
        [HttpPost]
        [Authorize]
        public ActionResult MedicalLeadAgentFormSave(MedicalLeadAgent model)
        {
            Response ObjRes = new Response();
            int LUserid = Convert.ToInt32(Session["AdminID"]);

            try
            {
                if (model != null && !string.IsNullOrEmpty(model.MedicalLeadAgentName_VC) && !string.IsNullOrEmpty(model.MedicalLeadAgentEmail_VC) && !Regex.IsMatch(model.MedicalLeadAgentMobilePhone_VC, @"[a-zA-Z]"))
                {
                    MedicalLeadAgent CheckLeadAgent = _MedicalLeadAgentRepository.GetMedicalLeadAgentList().FirstOrDefault(x => x.MedicalLeadAgentMobilePhone_VC == model.MedicalLeadAgentMobilePhone_VC);

                    if (CheckLeadAgent == null)
                    {
                        MedicalLeadAgent Lmodel = new MedicalLeadAgent
                        {
                            MedicalLeadAgentName_VC = model.MedicalLeadAgentName_VC,
                            MedicalLeadAgentMobilePhone_VC = model.MedicalLeadAgentMobilePhone_VC,
                            MedicalLeadAgentEmail_VC = model.MedicalLeadAgentEmail_VC,
                            MedicalLeadAgentRemarks_VC = model.MedicalLeadAgentRemarks_VC,
                            RecordStatus_NB = 0,
                            CreatedDate_DT = DateTime.Now,
                            CreatedUser_NB = LUserid,
                            MedicalLeadAgentLocation_VC = model.MedicalLeadAgentLocation_VC,
                            MedicalLeadAgentStatus_VC = "Open",
                            ImageTitle_VC = ImageUplode(model.ImageFile),
                            ImageTitle2_VC = ImageUplode(model.ImageFile2)
                        };

                        _MedicalLeadAgentRepository.NewMedicalLeadAgent(Lmodel);
                        ObjRes.Success = true;
                        ObjRes.Message = "Saved successfully";
                    }
                    else
                    {
                        ObjRes.Message = CheckLeadAgent != null ? "Already exists" : "Please fill the values...";
                    }
                }
                else
                {
                    ObjRes.Message = "Please check the parameter";
                }
            }
            catch (Exception ex)
            {
                ObjRes.Message = ex.Message;
                _AdminRepository.SaveErrorLogs("Admin", "MedicalLeadAgentFormSave", "Error", Convert.ToString(model.MedicalLeadAgentMobilePhone_VC), ex.Message + ex.StackTrace, 0, "");
            }

            TempData["StatusMess"] = ObjRes.Message;

            if (ObjRes.Success == true)
            {
                return RedirectToAction("MedicalLeadAgentFormSave", "MedicalLeadAgent");
            }
            else
            {
                return View(model);
            }

        }

        // GET: Get MedicalLeadAgent
        [Authorize]
        [HttpGet]
        public ActionResult MedicalLeadAgentForm()
        {
            MedicalLeadAgent model = new MedicalLeadAgent();
            int LUserid = Convert.ToInt32(Session["AdminID"]);

            ViewBag.StatusListlist = GetStatusDD("ML");
            try
            {

                if (@Session["LeadList"] != null && Session["LeadList"].ToString() != string.Empty)
                {
                    if (model != null)
                    {

                        model.MedicalLeadAgentList = @Session["LeadList"] as List<MedicalLeadAgent>;
                        ViewBag.fromDate = @Session["FromDataValue"] as DateTime?;
                        ViewBag.toDate = @Session["toDataValue"] as DateTime?;

                    }
                }


                if (TempData["StatusMess"] == null)
                {
                    TempData["StatusMess"] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                _AdminRepository.SaveErrorLogs("Admin", "MedicalLeadAgentForm", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult MedicalLeadAgentForm(DateTime? FromDate, DateTime? ToDate, string MedicalLeadAgentStatus_VC)
        {
            List<MedicalLeadAgent> Modellist = new List<MedicalLeadAgent>();
            int LUserid = Convert.ToInt32(Session["AdminID"]);
            MedicalLeadAgent model = new MedicalLeadAgent();
            ViewBag.StatusListlist = GetStatusDD("ML");

            try
            {
                TempData["StatusMess"] = string.Empty;

                

                if (FromDate != null && ToDate != null)
                {

                        Modellist = GetMedicalLeadAgentDetails();


                        if (!string.IsNullOrEmpty(MedicalLeadAgentStatus_VC))
                        {
                            Modellist = Modellist.Where(s => s.MedicalLeadAgentStatus_VC == MedicalLeadAgentStatus_VC).ToList();
                        }

                     
                        if (FromDate.HasValue && ToDate.HasValue)
                        {
                            Modellist = Modellist.Where(s => s.CreatedDate_DT.Date >= FromDate.Value.Date && s.CreatedDate_DT.Date <= ToDate.Value.Date).ToList();
                        }

                        TempData["MedicalLeadListT"] = Modellist;
                        Session["LeadList"] = Modellist;
                        Session["FromDataValue"] = FromDate;
                        Session["toDataValue"] = ToDate;
                    
                    model.MedicalLeadAgentList = Modellist;
                }
                else if (FromDate.HasValue)
                {
                   
                    TempData["StatusMess"] = "Please Fill From Date and To Date";
                }
                else if (ToDate.HasValue)
                {
                   
                    TempData["StatusMess"] = "Please Fill From Date and To Date";
                }
                else
                {
                   
                    TempData["StatusMess"] = "Please Fill From Date and To Date";
                }


            }
            catch (Exception ex)
            {
                _AdminRepository.SaveErrorLogs("Admin", "MedicalLeadAgentForm", "Error", "", ex.Message + ex.StackTrace, 0, "");
                return RedirectToAction("MedicalLeadAgentForm");
            }
            return View(model);

        }

        [Authorize]
        [HttpGet]
        public ActionResult UpdateMedicalLeadAgent(int? id)
        {

            Response ObjRes = new Response();

            MedicalLeadAgent objLeadAgentModel = new MedicalLeadAgent();
            try
            {

                ViewBag.StatusListlist = GetStatusDD("ML");
                

                if (id != null)
                {
                    objLeadAgentModel = _MedicalLeadAgentRepository.GetUserByMedicalLeadAgentId((int)id);

                    TempData["StatusMess"] = ObjRes.Message;


                }
                else
                {
                    ObjRes.Message = "Invalid";
                    TempData["StatusMess"] = ObjRes.Message;
                }

                if (TempData["StatusMess"] == null)
                {
                    TempData["StatusMess"] = string.Empty;
                }
            }
            catch (Exception ex)
            {
               _AdminRepository.SaveErrorLogs("Admin", "UpdateMedicalLeadAgent", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }


            return View(objLeadAgentModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateMedicalLeadAgent([Bind(Include = "MedicalLeadAgentID,MedicalLeadAgentName_VC,MedicalLeadAgentMobilePhone_VC,MedicalLeadAgentEmail_VC,MedicalLeadAgentStatus_VC,MedicalLeadAgentRemarks_VC,MedicalLeadAgentLocation_VC")] MedicalLeadAgent ObjLeadAgent)
        {

            Response ObjRes = new Response();
            MedicalLeadAgent CheckLeadAgent = new MedicalLeadAgent();
            int LUserid = Convert.ToInt32(Session["AdminID"]);

            ViewBag.StatusListlist = GetStatusDD("ML");
            try
            {
                CheckLeadAgent = _MedicalLeadAgentRepository.GetUserByMedicalLeadAgentId(ObjLeadAgent.MedicalLeadAgentID);
                if (CheckLeadAgent != null)
                {

                    if (ModelState.IsValid)
                    {

                        CheckLeadAgent.MedicalLeadAgentID = ObjLeadAgent.MedicalLeadAgentID;
                        CheckLeadAgent.MedicalLeadAgentName_VC = ObjLeadAgent.MedicalLeadAgentName_VC;
                        CheckLeadAgent.MedicalLeadAgentMobilePhone_VC = ObjLeadAgent.MedicalLeadAgentMobilePhone_VC;
                        CheckLeadAgent.MedicalLeadAgentEmail_VC = ObjLeadAgent.MedicalLeadAgentEmail_VC;
                        CheckLeadAgent.MedicalLeadAgentStatus_VC = ObjLeadAgent.MedicalLeadAgentStatus_VC;
                        CheckLeadAgent.MedicalLeadAgentRemarks_VC = ObjLeadAgent.MedicalLeadAgentRemarks_VC;
                        CheckLeadAgent.MedicalLeadAgentLocation_VC = ObjLeadAgent.MedicalLeadAgentLocation_VC;
                        CheckLeadAgent.ModifiedDate_DT = DateTime.Now;
                        CheckLeadAgent.ModifiedUser_NB = LUserid;

                        if (!Regex.IsMatch(ObjLeadAgent.MedicalLeadAgentMobilePhone_VC, ("[a-zA-Z]")))
                        {
                            if (ObjLeadAgent.MedicalLeadAgentMobilePhone_VC.Length >= 6)
                            {

                                _MedicalLeadAgentRepository.UpdateMedicalLeadAgent(CheckLeadAgent);

                                ObjRes.Message = "Updated successfully";
                            }
                            else
                            {
                                ObjRes.Message = "Invalid Mobile Number";
                            }
                        }
                        else
                        {
                            ObjRes.Message = "Invalid Mobile Number";
                        }


                    }
                    else
                    {
                        ObjRes.Message = "Invalid Mobile Number";
                    }

                }
                else
                {
                    ObjRes.Message = "Invalid user";
                }
                TempData["StatusMess"] = ObjRes.Message;

            }
            catch (Exception ex)
            {
                 _AdminRepository.SaveErrorLogs("Admin", "UpdateMedicalLeadAgent", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }

            return View(ObjLeadAgent);

        }


        // GET: MedicalLeadAgent
        public ActionResult Index()
        {
            return View();
        }

        public string ImageUplode(HttpPostedFileBase ImageFile)
        {
            string imageName = "";
            if (ImageFile != null)
            {
                string fileName2 = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extention2 = Path.GetExtension(ImageFile.FileName);
                fileName2 = fileName2 + DateTime.Now.ToString("yymmssfff") + extention2;
                imageName = fileName2;
                fileName2 = Path.Combine(Server.MapPath("~/AttachmentImage/"), fileName2);
                ImageFile.SaveAs(fileName2);
            }
            else
            {
                imageName = "ErrorDataFound.png";
            }


            return imageName;
        }

        public List<MedicalLeadAgent> GetMedicalLeadAgentDetails()
        {
            List<MedicalLeadAgent> Modelist = new List<MedicalLeadAgent>();
           

            try
            {
                var Record = _MedicalLeadAgentRepository
                                .GetMedicalLeadAgentList()
                                .Where(x => x.RecordStatus_NB == 0)
                                .Select(ML => new MedicalLeadAgent
                                {
                                  MedicalLeadAgentID = ML.MedicalLeadAgentID,
                                  MedicalLeadAgentName_VC = ML.MedicalLeadAgentName_VC,
                                  MedicalLeadAgentMobilePhone_VC = ML.MedicalLeadAgentMobilePhone_VC,
                                  MedicalLeadAgentEmail_VC = ML.MedicalLeadAgentEmail_VC,                                  
                                  CreatedDate_DT = ML.CreatedDate_DT,                                 
                                  MedicalLeadAgentStatus_VC = ML.MedicalLeadAgentStatus_VC,
                                  MedicalLeadAgentRemarks_VC = ML.MedicalLeadAgentRemarks_VC,
                                  MedicalLeadAgentLocation_VC = ML.MedicalLeadAgentLocation_VC,
                                  ImageTitle_VC = BindFileName(ML.ImageTitle_VC),
                                  ImageTitle2_VC = BindFileName(ML.ImageTitle2_VC)
                                
                              }).OrderByDescending(x => x.MedicalLeadAgentID).Distinct().ToList();

                Modelist = Record.ToList();

                TempData["MedicalLeadListT"] = Modelist;

            }
            catch (Exception ex)
            {
              
                _AdminRepository.SaveErrorLogs("Admin", "GetMedicalLeadAgentDetails", "Error", Modelist + "," + Modelist, ex.Message + ex.StackTrace, 0, "");

            }


            return Modelist;
        }
        public string BindFileName(string yourPath)
        {
            string AttImgPath = null;
            string TokenUrl = ConfigurationManager.AppSettings["TokenUrl"];
            if (yourPath != null)
            {

                // using the method
                string filename = Path.GetFileName(yourPath);
                //string filename = yourPath;
                if (filename != null && filename != string.Empty)
                {

                    string LocalPathFile = System.Web.Hosting.HostingEnvironment.MapPath("~/AttachmentImage/" + filename);
                    if (LocalPathFile !=null)
                    {
                        AttImgPath = TokenUrl + "AttachmentImage/" + filename;
                    }
                    else
                    {
                        AttImgPath = TokenUrl + "AttachmentImage/" + "ErrorDataFound.png";
                    }

                }
            }
            else
            {
                AttImgPath = TokenUrl + "AttachmentImage/" + "ErrorDataFound.png";
            }
            return AttImgPath;
        }
        public List<SelectListItem> GetStatusDD(string StatusType)
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();
            var StatusListquery = _dbContext.StatusMasters.Where(c => c.StatusType == StatusType).Select(d => new SelectListItem
            {
                Value = d.ID.ToString(),
                Text = d.Status,
            });
            StatusList = StatusListquery.ToList();
            return StatusList;
        }

        public ActionResult GetMedicalLeadAgentFileDownload(int? id)
        {
            MedicalLeadAgent objLeadAgentModel = new MedicalLeadAgent();

            objLeadAgentModel = _MedicalLeadAgentRepository.GetUserByMedicalLeadAgentId((int)id);

            string image1 = Server.MapPath("~/AttachmentImage/" + objLeadAgentModel.ImageTitle_VC);
            string image2 = Server.MapPath("~/AttachmentImage/" + objLeadAgentModel.ImageTitle2_VC);

            try
            {

                //Two images download as zip format

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Create a zip archive
                    using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        // Add the first image to the zip archive
                        AddFileToZip(zipArchive, image1, objLeadAgentModel.ImageTitle_VC);

                        // Add the second image to the zip archive
                        AddFileToZip(zipArchive, image2, objLeadAgentModel.ImageTitle2_VC);
                    }

                    // Set the content type
                    Response.ContentType = "application/zip";

                    // Set the content-disposition header to force download
                    Response.AddHeader("content-disposition", "attachment; filename=" + objLeadAgentModel.MedicalLeadAgentID + "Attachment.zip");

                    // Write the zip file bytes to the response stream
                    Response.BinaryWrite(memoryStream.ToArray());
                }

                // End the response
                Response.End();



            }
            catch (Exception ex)
            {
                _AdminRepository.SaveErrorLogs("Admin", "GetMedicalLeadAgentFileDownload", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }
            return RedirectToAction("MedicalLeadAgentForm", "MedicalLeadAgent");
        }

        private void AddFileToZip(ZipArchive zipArchive, string filePath, string entryName)
        {
            // Create a new entry in the zip archive
            ZipArchiveEntry entry = zipArchive.CreateEntry(entryName);

            // Open the file to be added to the zip archive
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Get a stream for the entry in the zip archive
                using (Stream entryStream = entry.Open())
                {
                    // Copy the file content to the zip archive entry stream
                    fileStream.CopyTo(entryStream);
                }
            }
        }

        public ActionResult DeleteLeadeAgentAdminDashBoard(int? id)
        {
            //string pop = "Are you sure you want to delete this Record?";
            Response ObjRes = new Response();

            MedicalLeadAgent model = new MedicalLeadAgent();
            try
            {
                if (id != 0)
                {
                    model = _MedicalLeadAgentRepository.GetUserByMedicalLeadAgentId(Convert.ToInt32(id));
                    if (model != null)
                    {
                        if (model.MedicalLeadAgentMobilePhone_VC.Length >= 6)
                        {
                            model.RecordStatus_NB = 1;
                            _MedicalLeadAgentRepository.UpdateMedicalLeadAgent(model);
                            ObjRes.Message = "Deleted successfully";
                        }
                        else
                        {
                            ObjRes.Message = "An error occure while deleting...";
                        }

                        model.MedicalLeadAgentList = Session["LeadList"] as List<MedicalLeadAgent>;
                        var itemToRemove = model.MedicalLeadAgentList.FirstOrDefault(i => i.MedicalLeadAgentID == id);
                        if (itemToRemove != null)
                        {
                            model.MedicalLeadAgentList.Remove(itemToRemove);
                       
                            Session["LeadList"] = model.MedicalLeadAgentList; // Save the updated list back to TempData

                        }

                    }
                }
                TempData["StatusMess"] = ObjRes.Message;
            }
            catch (Exception ex)
            {
                _AdminRepository.SaveErrorLogs("Admin", "DeleteLeadeAgentAdminDashBoard", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }

            //  return RedirectToAction("LeadAgentForm");
            return RedirectToAction("MedicalLeadAgentForm", "MedicalLeadAgent");

        }

        public ActionResult GetLeadAgentReportList()
        {
            try
            {
                List<MedicalLeadAgent> Modellist1 = new List<MedicalLeadAgent>();
                //IEnumerable<LeadAgentModel> LeadAgentLIST = _dbSale.LeadAgentDB.ToList();

                if (TempData["StatusMess"] == null)
                {
                    TempData["StatusMess"] = string.Empty;
                }



                if (@TempData["MedicalLeadListT"] != null && TempData["MedicalLeadListT"].ToString() != string.Empty)
                {
                    List<MedicalLeadAgent> Modellist = @TempData["MedicalLeadListT"] as List<MedicalLeadAgent>;
                    if (Modellist.Count() > 0)
                    {
                        List<MedicalLeadAgentReport> reportList = new List<MedicalLeadAgentReport>();
                        foreach (var item in Modellist)
                        {
                            MedicalLeadAgentReport objRE = new MedicalLeadAgentReport();

                            objRE.MedicalLeadAgentName_VC = item.MedicalLeadAgentName_VC;
                            objRE.MedicalLeadAgentMobilePhone_VC = item.MedicalLeadAgentMobilePhone_VC;
                            objRE.MedicalLeadAgentEmail_VC = item.MedicalLeadAgentEmail_VC;
                            objRE.MedicalLeadAgentLocation_VC = item.MedicalLeadAgentLocation_VC;
                            objRE.MedicalLeadAgentStatus_VC = item.MedicalLeadAgentStatus_VC;
                            objRE.MedicalLeadAgentRemarks_VC = item.MedicalLeadAgentRemarks_VC;
                            reportList.Add(objRE);
                        }
                        DownloadExcelReport(reportList, "MedicalLeadAgentReport");
                    }
                    else
                    {
                        TempData["StatusMess"] = "No Data Found";
                        return RedirectToAction("MedicalLeadAgentForm", "MedicalLeadAgent");
                    }
                }
            }
            catch (Exception ex)
            {
                  _AdminRepository.SaveErrorLogs("Admin", "GetLeadAgentReportList", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }
            return RedirectToAction("MedicalLeadAgentForm", "MedicalLeadAgent");
        }

        public void DownloadExcelReport(object TempReport, string Filename)
        {
            try
            {

                var gv = new GridView();
                gv.DataSource = TempReport;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string date_str = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string filNa = Filename + date_str;
                Response.AddHeader("content-disposition", "attachment; filename=" + filNa + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                gv.RenderControl(objHtmlTextWriter);

                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
               _AdminRepository.SaveErrorLogs("Admin", "DownloadExcelReport", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }
        }
    }
}