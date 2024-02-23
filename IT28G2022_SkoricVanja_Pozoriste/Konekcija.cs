using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IT28G2022_SkoricVanja_Pozoriste
{
    internal class Konekcija
    {
        public SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder();
            ccnSb.DataSource = @"LAPTOP-GVJLU57Q\SQLEXPRESS";
            ccnSb.InitialCatalog = "Pozoriste2023";
            ccnSb.IntegratedSecurity = true;
            string con = ccnSb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;


        }
    }
}
