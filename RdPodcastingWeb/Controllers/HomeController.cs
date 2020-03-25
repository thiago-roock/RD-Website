using CsvHelper;
using RdPodcastingWeb.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RdPodcastingWeb.Controllers
{
    [OutputCache(CacheProfile = "CacheLong")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }
        public ActionResult Episodios()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult teste()
        {
            return View();
        }

        [Route("{dataBusca:string}")]
        public async Task<ActionResult> Covid19(string dataBusca)
        {
            string _dia = null;
            string _mes = null;
            string _ano = null;

            if (dataBusca != "start")
            {

                _dia = dataBusca.Split('-')[0];
                _mes = dataBusca.Split('-')[1];
                _ano = dataBusca.Split('-')[2];

            }
            else
            {
                _dia = AdicionarZero((DateTime.Now.Day - 1).ToString());
                _mes = AdicionarZero(DateTime.Now.Month.ToString());
                _ano = DateTime.Now.Year.ToString();
            }



            ViewBag.CovidData = await ConstruirObjetoDeHoje(_dia,_mes,_ano);

            return View();
        }
        public async Task<DataCovid19> ConstruirObjetoDeHoje(string dia, string mes,string ano)
        {

            var client = new HttpClient();
            string queryString = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + mes + "-" + dia + "-" + ano + ".csv";

            var response = await client.GetAsync(queryString);
            if(response.IsSuccessStatusCode)
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    return EncontrarPais(new CsvReader(reader, CultureInfo.InvariantCulture)
                        .GetRecords<DataCovid19>()
                        .ToList());
                }
            return null;
        }

        public async Task<DataCovid19> ConstruirObjetoDeData(string dia, string mes, string ano)
        {
            var client = new HttpClient();
            string queryString = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + mes + "-" + dia + "-" + ano + ".csv";

            var response = await client.GetAsync(queryString);
            if (response.IsSuccessStatusCode)
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    return EncontrarPais(new CsvReader(reader, CultureInfo.InvariantCulture)
                        .GetRecords<DataCovid19>()
                        .ToList());
                }
            return null;
        }

        public DataCovid19 EncontrarPais(List<DataCovid19> listCovid, string NomeDoPais = "Brazil")
        {
            if (listCovid != null)
                foreach (DataCovid19 pais in listCovid)
                    if (pais.Country_Region.Equals(NomeDoPais))
                        return pais;

            return null;
        }

        public string AdicionarZero(string numero)
        {
            if (int.Parse(numero) < 9)
                numero = "0" + numero;

            return numero;
        }

    }
}