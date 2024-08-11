using sampleTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sampleTest.Repository
{
    internal interface IAdminRepository
    {
        void UpdatePassword(AdminLogin model);
        AdminLogin AdminUserDetails(int id);
        List<AdminLogin> GetAdminModelList();
        AdminLogin GetUserByUserName(string name);
        void SaveErrorLogs(string ModuleName, string ProcedureName, string LClassName, string LogParameter, string LogRemarks, int LUserId, string ECode);

    }
}
