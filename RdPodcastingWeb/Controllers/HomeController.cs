<<<<<<< Updated upstream
﻿using CsvHelper;
using RdPodcastingWeb.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
=======
﻿using System.Web.Mvc;
>>>>>>> Stashed changes

namespace RdPodcastingWeb.Controllers
{
 
    public class HomeController : Controller
    {
        [OutputCache(CacheProfile = "CacheLong")]
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
        public ActionResult Contato()
        {
            return View();
        }
<<<<<<< Updated upstream
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
                _dia = AdicionarZero((DateTime.Now.Day).ToString());
                _mes = AdicionarZero(DateTime.Now.Month.ToString());
                _ano = DateTime.Now.Year.ToString();
            }
            DataCovid19 covidData;
            int subtrair = 0;
            int diaLoop = 0;
            bool mesanterior = false;
            do
            {
                if (!mesanterior)
                    diaLoop = string.IsNullOrEmpty(_dia) ? 0 : int.Parse(_dia) - subtrair;

                covidData = Task.Run(() => ConstruirObjetoDeHoje(AdicionarZero(diaLoop.ToString()), _mes, _ano)).Result;
                subtrair += 1;

                if (diaLoop == 0)
                {
                    _mes = AdicionarZero((int.Parse(_mes) - 1).ToString());
                    diaLoop = DateTime.DaysInMonth(int.Parse(_ano), int.Parse(_mes));
                    mesanterior = true;
                }
                else if (covidData.Last_Update != null)
                    break;
            } while (covidData.Last_Update == null);


            covidData.Last_Update = Convert.ToDateTime(covidData.Last_Update).ToString("D",
            CultureInfo.CreateSpecificCulture("pt-BR"));
                      
            ViewBag.CovidData = covidData;

            var casosDoMesAtual = await CasosDoMesAtual();
            casosDoMesAtual.CasosConfirmados = Convert.ToDouble(covidData.Confirmed);
            ViewBag.CovidMesAtual = casosDoMesAtual;
            int mesAnterior = DateTime.Now.Month - 1;

            var casosDoMes =await CasosDoMes(mesAnterior, DateTime.Now.Year);

            ViewBag.CovidMesAnterior = casosDoMes;
            return View();
        }
        public async Task<Covid19Mensal> CasosDoMesAtual()
        {
            Covid19Mensal obj = new Covid19Mensal() 
            { 
                Mes = ReplacePorNomeDoMes(DateTime.Now.Month)
            };

            var quantidadeDeDias = DateTime.Now.Day == 1 ? DateTime.Now.Day : DateTime.Now.Day - 1;

            for (int qtdeDeAnalises = 0; qtdeDeAnalises < 3; qtdeDeAnalises++)
            {
                double primeiroCaso = 0;
                double ultimoCaso = 0;
                for (int i = 1; i <= quantidadeDeDias; i++)
                {
                    var objResultado = await ConstruirObjetoDeHoje(
                        AdicionarZero(i.ToString()),
                        AdicionarZero(DateTime.Now.Month.ToString()),
                        DateTime.Now.Year.ToString());

                    var InfectadosCurados = string.IsNullOrEmpty(objResultado.Recovered) ? 0.0 : double.Parse(objResultado.Recovered);
                    var Mortos = string.IsNullOrEmpty(objResultado.Deaths) ? 0.0 : double.Parse(objResultado.Deaths);
                    var InfectadosEmTratamento = string.IsNullOrEmpty(objResultado.Active) ? 0.0 : double.Parse(objResultado.Active);


                    switch (qtdeDeAnalises) 
                    {
                        case 0:
                            {
                                if (InfectadosCurados > primeiroCaso && primeiroCaso == 0)
                                    primeiroCaso = InfectadosCurados;
                                else if(InfectadosCurados > ultimoCaso && primeiroCaso > 0)
                                        ultimoCaso = InfectadosCurados;

                                
                                obj.InfectadosCurados = ultimoCaso <= primeiroCaso? primeiroCaso:Total(primeiroCaso, ultimoCaso);
                                continue;
                            }
                        case 1:
                            {
                                if (Mortos > primeiroCaso && primeiroCaso == 0)
                                    primeiroCaso = Mortos;
                                else if (Mortos > ultimoCaso && primeiroCaso > 0)
                                    ultimoCaso = Mortos;


                                obj.Mortos = ultimoCaso <= primeiroCaso ? primeiroCaso : Total(primeiroCaso, ultimoCaso);
                                continue;
                            }
                        case 2:
                            {
                                if (InfectadosEmTratamento > primeiroCaso && primeiroCaso == 0)
                                    primeiroCaso = InfectadosEmTratamento;
                                else if (InfectadosEmTratamento > ultimoCaso && primeiroCaso > 0)
                                    ultimoCaso = InfectadosEmTratamento;


                                obj.InfectadosEmTratamento = ultimoCaso <= primeiroCaso ? primeiroCaso : Total(primeiroCaso, ultimoCaso);
                                continue;
                            }
                    }
                }
                
            }
            return obj;
        }
        public async Task<Covid19Mensal> CasosDoMes(int mes,int ano = 2020)
        {
            
            Covid19Mensal obj = new Covid19Mensal()
            {
                Mes = ReplacePorNomeDoMes(mes)
            };

            var quantidadeDeDias = DateTime.DaysInMonth(ano,mes);

            for (int qtdeDeAnalises = 0; qtdeDeAnalises < 3; qtdeDeAnalises++)
            {
                double primeiroCaso = 0;
                double ultimoCaso = 0;
                for (int i = 1; i <= quantidadeDeDias; i++)
                {
                    var objResultado = await ConstruirObjetoDeHoje(
                        AdicionarZero(i.ToString()),
                        AdicionarZero(mes.ToString()),
                        ano.ToString());

                    var InfectadosCurados = string.IsNullOrEmpty(objResultado.Recovered) ? 0.0 : double.Parse(objResultado.Recovered);
                    var Mortos = string.IsNullOrEmpty(objResultado.Deaths) ? 0.0 : double.Parse(objResultado.Deaths);
                    var InfectadosEmTratamento = string.IsNullOrEmpty(objResultado.Active) ? 0.0 : double.Parse(objResultado.Active);


                    switch (qtdeDeAnalises)
                    {
                        case 0:
                            {
                                if (InfectadosCurados > primeiroCaso && primeiroCaso == 0)
                                    primeiroCaso = InfectadosCurados;
                                else if (InfectadosCurados > ultimoCaso && primeiroCaso > 0)
                                    ultimoCaso = InfectadosCurados;


                                obj.InfectadosCurados = ultimoCaso <= primeiroCaso ? primeiroCaso : Total(primeiroCaso, ultimoCaso);
                                continue;
                            }
                        case 1:
                            {
                                if (Mortos > primeiroCaso && primeiroCaso == 0)
                                    primeiroCaso = Mortos;
                                else if (Mortos > ultimoCaso && primeiroCaso > 0)
                                    ultimoCaso = Mortos;


                                obj.Mortos = ultimoCaso <= primeiroCaso ? primeiroCaso : Total(primeiroCaso, ultimoCaso);
                                continue;
                            }
                        case 2:
                            {
                                if (InfectadosEmTratamento > primeiroCaso && primeiroCaso == 0)
                                    primeiroCaso = InfectadosEmTratamento;
                                else if (InfectadosEmTratamento > ultimoCaso && primeiroCaso > 0)
                                    ultimoCaso = InfectadosEmTratamento;


                                obj.InfectadosEmTratamento = ultimoCaso <= primeiroCaso ? primeiroCaso : Total(primeiroCaso, ultimoCaso);
                                continue;
                            }
                    }
                }

            }
            return obj;
        }
        public double Total(double primeiro,double ultimo) 
        {
            return ultimo - primeiro;
        }
        public async Task<JsonResult> GetDataGraficoDias()
        {
            var lista = await ConstruirListaParaGraficos();
            return Json(lista,JsonRequestBehavior.AllowGet);
        }
        public async Task<Covid19Data> ConstruirListaParaGraficos()
        {
            string _dia = null;
            string _mes = null;
            string _ano = null;

            Covid19Data CovidData = new Covid19Data()
            {
                ListaMes = new List<Covid19Mensal>(),
                ListaDias = new List<Covid19Dia>()
            };

            for (int ano = 2020; ano <= DateTime.Now.Year; ano++)
            {
                _ano = ano.ToString();
                for (int mes = 1; mes <= DateTime.Now.Month; mes++)
                {
                    var quantidadeDeDias = 0;
                    if (DateTime.Now.Hour >= 21) 
                        quantidadeDeDias = mes == DateTime.Now.Month? DateTime.Now.Day:DateTime.DaysInMonth(ano, mes);
                    else
                        quantidadeDeDias = mes == DateTime.Now.Month ? DateTime.Now.Day - 1 : DateTime.DaysInMonth(ano, mes);

                    _mes = AdicionarZero(mes.ToString());
                    string MesNome = ReplacePorNomeDoMes(mes);

                    for (int dia = 1; dia <= quantidadeDeDias; dia++)
                    {
                        _dia = AdicionarZero(dia.ToString());
                        var objHj = await ConstruirObjetoDeHoje(_dia, _mes, _ano);

                        if (dia + 1 > quantidadeDeDias)
                        {
                            CovidData.ListaMes.Add(new Covid19Mensal()
                            {
                                Mes = MesNome,
                                Mortos = double.Parse(objHj.Deaths),
                                InfectadosEmTratamento = double.Parse(objHj.Active),
                                InfectadosCurados = double.Parse(objHj.Recovered),
                                CasosConfirmados = double.Parse(objHj.Confirmed)
                            });
                        }

                        CovidData.ListaDias.Add(new Covid19Dia()
                        {
                            Mes = MesNome,
                            Dia = dia,
                            Mortos = double.Parse(objHj.Deaths),
                            InfectadosEmTratamento = double.Parse(objHj.Active),
                            InfectadosCurados = double.Parse(objHj.Recovered),
                            CasosConfirmados = double.Parse(objHj.Confirmed)
                        });
                    }
                }
            }

            return CovidData;
        }
        private DataCovid19 montarObj(string dia, string mes, string ano)
        {
            var pathString = Server.MapPath("~/files/" +mes + "-" + dia + "-" + ano + ".csv");
  
            if (System.IO.File.Exists(pathString)) 
            { 
                var reader = encontrarArquivo(pathString);

                if (reader != null)
                {
                    var resultado = popularListaDataCovid19(reader);
                    return EncontrarPais(resultado);
                }
            }
            return null;
        }
        public async Task<DataCovid19> ConstruirObjetoDeHoje(string dia, string mes,string ano)
        {
            var objDataCovid19 = montarObj(dia, mes, ano);
            if (objDataCovid19 != null)
                return objDataCovid19;

            var client = new HttpClient();
            string queryString = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + mes + "-" + dia + "-" + ano + ".csv";

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var rc = await response.Content.ReadAsStreamAsync();
                    using (var reader = new StreamReader(rc))
                    {
                        var resultado = popularListaDataCovid19(reader);
                        var pathString = Server.MapPath("~/files/" + mes + "-" + dia + "-" + ano + ".csv");
                        using (var sr = new StreamWriter(pathString, false, Encoding.UTF8))
                        {
                            using (var csv = new CsvWriter(sr, CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(resultado);
                            }
                        }

                        return EncontrarPais(resultado);
                    }
                }
            }
            catch(Exception ex)
            {
                return new DataCovid19() { Deaths = "0", Active = "0", Confirmed = "0", Recovered = "0" };
            }

            return new DataCovid19() { Deaths = "0", Active = "0", Confirmed = "0", Recovered = "0" };
        }
        public List<DataCovid19> popularListaDataCovid19(StreamReader reader)
        {
            return new CsvReader(reader, CultureInfo.InvariantCulture)
                            .GetRecords<DataCovid19>()
                            .ToList();
        }
        public StreamReader encontrarArquivo(string NomeArquivo)
        {
            return new StreamReader(NomeArquivo);
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
            if (int.Parse(numero) <= 9)
                numero = "0" + numero;

            return numero;
        }
        public string ReplacePorNomeDoMes(int mes)
        {
            switch (mes)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
                default:
                    return "SwitchError";


            }
        }

=======
>>>>>>> Stashed changes
    }
}