using sampleTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace sampleTest.Repository
{
    public class MedicalLeadAgentRepository :IMedicalLeadAgentRepository
    {
        private readonly ModelEntity _dbContext;


        public MedicalLeadAgentRepository()
        {

            _dbContext = new ModelEntity();
        }

        public MedicalLeadAgentRepository(ModelEntity context)
        {
            _dbContext = context;

        }

        public void NewMedicalLeadAgent(MedicalLeadAgent model)
        {
            try
            {
                _dbContext.MedicalLeadAgents.Add(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
              //  _AppService.SaveErrorLogs("API/ADMIN", "NewLeadAgent", "Error", model.LeadAgentMobilePhone_VC + "," + model.LeadAgentID, ex.Message + ex.StackTrace, 0, "");
            }
        }

        public void UpdateMedicalLeadAgent(MedicalLeadAgent model)
        {
            try
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
              //  _AppService.SaveErrorLogs("API/ADMIN", "UpdateLeadAgent", "Error", Convert.ToString(model.LeadAgentID), ex.Message + ex.StackTrace, 0, "");
            }
        }

        public void DeleteMedicalLeadAgent(MedicalLeadAgent model)
        {
            try
            {
                _dbContext.Entry(model).State = EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
               // _AppService.SaveErrorLogs("API/ADMIN", "DeleteLeadAgent", "Error", Convert.ToString(model.LeadAgentID), ex.Message + ex.StackTrace, 0, "");
            }
        }

        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
               // _AppService.SaveErrorLogs("API/ADMIN", "Save", "Error", "", ex.Message + ex.StackTrace, 0, "");
            }
        }

        public List<MedicalLeadAgent> GetMedicalLeadAgentList()
        {
            List<MedicalLeadAgent> Modellist = _dbContext.MedicalLeadAgents.Where(x => x.RecordStatus_NB == 0).ToList();
            return Modellist;
        }

        public MedicalLeadAgent GetUserByMedicalLeadAgentId(int id)
        {
            return _dbContext.MedicalLeadAgents.FirstOrDefault(x => x.MedicalLeadAgentID == id);
        }
    }
}