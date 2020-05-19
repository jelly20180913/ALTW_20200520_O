using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataModel.CustomModel.SAP.Mapping;
namespace TestWebApi.Common
{

    public class Mapping
    {
        public static List<CustomerType> CustomerTypeList = new List<CustomerType>
        {
            new CustomerType {CGrp="01",Name = "安費諾集團", OfficialName = "APH" },
            new CustomerType {CGrp="02",Name = "一般客戶",  OfficialName = "CUS" },
            new CustomerType { CGrp="03", Name = "經銷商",    OfficialName = "DIS"},
            new CustomerType {CGrp="04",Name = "DIS-END-CUS", OfficialName = "CUS" },
            new CustomerType {CGrp="05",Name = "EMS",  OfficialName = "CUS" },
            new CustomerType { CGrp="06", Name = "EMS-END-CUS",    OfficialName = "CUS" },
            new CustomerType {CGrp="07",Name = "PL", OfficialName = "CUS" },
            new CustomerType {CGrp="08",Name = "APH-END-CUS",  OfficialName = "CUS" },
            new CustomerType { CGrp="09", Name = "B2B-CUS",    OfficialName = "CUS" }
        };
        public static List<CountryRegion> CountryRegionList = new List<CountryRegion>
        {
            new CountryRegion {Ctr="TR" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="CN" , OfficialRegion ="China" },
            new CountryRegion {Ctr="DK" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="BR" , OfficialRegion ="South America" },
            new CountryRegion {Ctr="PK" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="JP" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="BE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="IL" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="CA" , OfficialRegion ="North America" },
            new CountryRegion {Ctr="TW" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="BY" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="LT" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="IQ" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="HU" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="ID" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="IN" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="ES" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="HR" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="GR" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="LV" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="FR" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="PL" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="KE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="FI" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="AE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="AR" , OfficialRegion ="South America" },
            new CountryRegion {Ctr="RU" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="BG" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="ZA" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="KR" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="JO" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="US" , OfficialRegion ="USA" },
            new CountryRegion {Ctr="GB" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="HK" , OfficialRegion ="China" },
            new CountryRegion {Ctr="EG" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="NO" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="TH" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="UA" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="NZ" , OfficialRegion ="Oceania" },
            new CountryRegion {Ctr="MY" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="MT" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="CZ" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="NL" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="LK" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="SK" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="SI" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="CL" , OfficialRegion ="South America" },
            new CountryRegion {Ctr="PH" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="VN" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="RS" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="AT" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="EE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="IE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="SG" , OfficialRegion ="APAC" },
            new CountryRegion {Ctr="SL" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="CH" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="SE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="IT" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="PT" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="MX" , OfficialRegion ="North America" },
            new CountryRegion {Ctr="DE" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="AU" , OfficialRegion ="Oceania" },
            new CountryRegion {Ctr="MO" , OfficialRegion ="China" },
            new CountryRegion {Ctr="LU" , OfficialRegion ="EMEA" },
            new CountryRegion {Ctr="RO" , OfficialRegion ="EMEA" } 
        };
    }
}
