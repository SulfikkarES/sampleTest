using sampleTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace sampleTest.Repository
{
    public class AdminRepository :IAdminRepository
    {
        private readonly ModelEntity _dbContext;


        public AdminRepository()
        {
        
            _dbContext = new ModelEntity();
        }

        public AdminRepository(ModelEntity context)
        {
            _dbContext = context;

        }

        public void UpdatePassword(AdminLogin model)
        {
            try
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
               // _AppService.SaveErrorLogs("ADMIN", "UpdatePassword", "Error", Convert.ToString(model.AID), ex.Message + ex.StackTrace, 0, "");
            }
        }

        public AdminLogin AdminUserDetails(int id)
        {
            AdminLogin model = new AdminLogin();
            try
            {
                if (id != null && id != 0)
                {
                    model = _dbContext.AdminLogins.FirstOrDefault(c => c.ID == id && c.RecordStatus_NB == 0);
                }
            }
            catch (Exception ex)
            {
                //SaveErrorLogs("Admin", "AdminUserDetails", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }

            return model;
        }

        public List<AdminLogin> GetAdminModelList()
        {
            List<AdminLogin> Modellist = _dbContext.AdminLogins.Where(x => x.RecordStatus_NB == 0).ToList();
            return Modellist;
        }

        public AdminLogin GetUserByUserName(string name)
        {
            return _dbContext.AdminLogins.FirstOrDefault(x => x.Username == name);
        }

        public void SaveErrorLogs(string ModuleName, string ProcedureName, string LClassName, string LogParameter, string LogRemarks, int LUserId, string ECode)
        {
            ErrorLog model = new ErrorLog();
            try
            {
                model.Log_ModuleName_VC = ModuleName;
                model.Log_ProcedureName_VC = ProcedureName;
                model.Log_ClassName_VC = LClassName;
                model.Log_Parameter_VC = LogParameter;
                model.Log_Remarks_VC = LogRemarks;
                model.Log_LogDate_DT = DateTime.Now;
                model.Log_UserID_NB = LUserId;
                model.Log_ErrorCode_VC = ECode;
                _dbContext.ErrorLogs.Add(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                model.Log_Remarks_VC = ex.Message;
                model.Log_LogDate_DT = DateTime.Now;
                model.Log_ClassName_VC = "ErrorLogModel";
                model.Log_UserID_NB = 0;
                model.Log_ModuleName_VC = "ErrorLog";
                _dbContext.ErrorLogs.Add(model);
                _dbContext.SaveChanges();
            }
        }

    }
}