using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CargaDadosNBA
{
    public class PaginaClassificacao
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;

        public PaginaClassificacao(SeleniumConfigurations configurations)
        {
            _configurations = configurations;

            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--headless");

            _driver = new FirefoxDriver(
                _configurations.CaminhoDriverFirefox,
                options);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(_configurations.Timeout);
            _driver.Navigate().GoToUrl(
                _configurations.UrlPaginaClassificacaoNBA);
        }

        public List<Conferencia> ObterClassificacao()
        {
            DateTime dataCarga = DateTime.Now;
            List<Conferencia> conferencias = new List<Conferencia>();

            string temporada = _driver
                .FindElement(By.ClassName("automated-header"))
                .FindElement(By.TagName("h1"))
                .Text.Split(new char[] { ' ' }).Last();

            var dadosConferencias = _driver
                .FindElements(By.ClassName("responsive-table-wrap"));
            var captions = _driver
                .FindElements(By.ClassName("table-caption"));

            for (int i = 0; i < captions.Count; i++)
            {
                var caption = captions[i];
                Conferencia conferencia = new Conferencia();
                conferencia.Temporada = temporada;
                conferencia.DataCarga = dataCarga;
                conferencia.Nome =
                    caption.FindElement(By.ClassName("long-caption")).Text;
                conferencias.Add(conferencia);

                int posicao = 0;
                var conf = dadosConferencias[i];
                var dadosEquipes = conf.FindElement(By.TagName("tbody"))
                    .FindElements(By.TagName("tr"));
                foreach (var dadosEquipe in dadosEquipes)
                {
                    var estatisticasEquipe =
                        dadosEquipe.FindElements(By.TagName("td"));

                    posicao++;
                    Equipe equipe = new Equipe();
                    equipe.Posicao = posicao;
                    equipe.Nome =
                        estatisticasEquipe[0].FindElement(
                            By.ClassName("team-names")).GetAttribute("innerHTML");
                    equipe.Vitorias = Convert.ToInt32(
                        estatisticasEquipe[1].Text);
                    equipe.Derrotas = Convert.ToInt32(
                        estatisticasEquipe[2].Text);
                    equipe.PercentualVitorias =
                        estatisticasEquipe[3].Text;

                    conferencia.Equipes.Add(equipe);
                }
            }

            return conferencias;
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}