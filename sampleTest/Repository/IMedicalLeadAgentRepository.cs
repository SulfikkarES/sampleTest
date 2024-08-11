using sampleTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sampleTest.Repository
{
    interface IMedicalLeadAgentRepository
    {
        List<MedicalLeadAgent> GetMedicalLeadAgentList();
        void NewMedicalLeadAgent(MedicalLeadAgent model);
        void UpdateMedicalLeadAgent(MedicalLeadAgent model);
        void DeleteMedicalLeadAgent(MedicalLeadAgent model);
        MedicalLeadAgent GetUserByMedicalLeadAgentId(int id);
    }
}
