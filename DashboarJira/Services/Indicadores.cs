using DashboarJira.Controller;
using DashboarJira.Model;
using MQTT.Infrastructure.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DashboarJira.Services
{
    public class Indicadores
    {
        public List<IndicadoresEntity> indicadores(string fechaInicio, string fechaFin) {
            JiraAccess jira = new JiraAccess();
            IAIOController iaio = new IAIOController(jira);
            IANOController iano = new IANOController(jira);
            ICPMController icpm = new ICPMController(jira);
            IEPMController iepm = new IEPMController(jira);
            RAIOController raio = new RAIOController(jira);
            RANOController rano = new RANOController(jira);
            IRFController IRF = new IRFController(jira);
            IndicadoresEntity indicadorAux = new IndicadoresEntity();

            // Realizar operaciones y guardar resultados en un objeto
            List<IndicadoresEntity> indicadores = new List<IndicadoresEntity>();

            //IAIO GENERAL
            IAIOEntity IAIOGeneral = iaio.IAIOGeneral(fechaInicio, fechaFin);

            indicadorAux.nombre = "IAIO GENERAL";
            indicadorAux.calculo = IAIOGeneral.CalcularIndicadorIAIO();
            indicadorAux.descripcion = IAIOGeneral.ToString();
            indicadores.Add(indicadorAux);

            //IAIO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IAIOEntity IAIOContratista = iaio.IAIOContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "IAIO CONTRATISTA";
            indicadorAux.calculo = IAIOContratista.CalcularIndicadorIAIO();
            indicadorAux.descripcion = IAIOContratista.ToString();
            indicadores.Add(indicadorAux);

            //IAIO NO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IAIOEntity IAIONoContratista = iaio.IAIONoContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "IAIO NO CONTRATISTA";
            indicadorAux.calculo = IAIONoContratista.CalcularIndicadorIAIO();
            indicadorAux.descripcion = IAIONoContratista.ToString();
            indicadores.Add(indicadorAux);

            //IANO GENERAL
            indicadorAux = new IndicadoresEntity();
            IANOEntity IANOGeneral = iano.IANOGeneral(fechaInicio, fechaFin);
            indicadorAux.nombre = "IANO GENERAL";
            indicadorAux.calculo = IANOGeneral.CalcularIndicadorIANO();
            indicadorAux.descripcion = IANOGeneral.ToString();
            indicadores.Add(indicadorAux);

            //IANO Contratista
            indicadorAux = new IndicadoresEntity();
            IANOEntity IANOContratista = iano.IANOContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "IANO CONTRATISTA";
            indicadorAux.calculo = IANOContratista.CalcularIndicadorIANO();
            indicadorAux.descripcion = IANOContratista.ToString();
            indicadores.Add(indicadorAux);

            //IANO NO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IANOEntity IANONoContratista = iano.IANO_NO_Contratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "IANO NO CONTRATISTA";
            indicadorAux.calculo = IANONoContratista.CalcularIndicadorIANO();
            indicadorAux.descripcion = IANONoContratista.ToString();
            indicadores.Add(indicadorAux);

            //ICPM ITTS
            indicadorAux = new IndicadoresEntity();
            ICPMEntity ICPMITTS = icpm.ICPM_ITTS(fechaInicio, fechaFin);
            indicadorAux.nombre = "ICPM ITTS";
            indicadorAux.calculo = ICPMITTS.CalcularIndicadorICPM();
            indicadorAux.descripcion = ICPMITTS.ToString();
            indicadores.Add(indicadorAux);

            //ICPM MTTO
            indicadorAux = new IndicadoresEntity();
            ICPMEntity ICPMMTTO = icpm.ICPM_MTTO(fechaInicio, fechaFin);
            indicadorAux.nombre = "ICPM MTTO";
            indicadorAux.calculo = ICPMMTTO.CalcularIndicadorICPM();
            indicadorAux.descripcion = ICPMMTTO.ToString();
            indicadores.Add(indicadorAux);

            //ICPM Puertas
            indicadorAux = new IndicadoresEntity();
            ICPMEntity ICPMPuertas = icpm.ICPM_PUERTAS(fechaInicio, fechaFin);
            indicadorAux.nombre = "ICPM PUERTAS";
            indicadorAux.calculo = ICPMPuertas.CalcularIndicadorICPM();
            indicadorAux.descripcion = ICPMPuertas.ToString();
            indicadores.Add(indicadorAux);

            //ICPM RFID
            indicadorAux = new IndicadoresEntity();
            ICPMEntity ICPMRFID = icpm.ICPM_RFID(fechaInicio, fechaFin);
            indicadorAux.nombre = "ICPM RFID";
            indicadorAux.calculo = ICPMRFID.CalcularIndicadorICPM();
            indicadorAux.descripcion = ICPMRFID.ToString();
            indicadores.Add(indicadorAux);

            //IEPM GENERAL
            indicadorAux = new IndicadoresEntity();
            IEPMEntity IEPM_GENERAL = iepm.IEPM_GENERAL(fechaInicio, fechaFin);
            indicadorAux.nombre = "IEPM GENERAL";
            indicadorAux.calculo = IEPM_GENERAL.CalcularIndicadorIEPM();
            indicadorAux.descripcion = IEPM_GENERAL.ToString();
            indicadores.Add(indicadorAux);

            //IEPM CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IEPMEntity IEPM_CONTRATISTA = iepm.IEPM_CONTRATISTA(fechaInicio, fechaFin);
            indicadorAux.nombre = "IEPM CONTRATISTA";
            indicadorAux.calculo = IEPM_CONTRATISTA.CalcularIndicadorIEPM();
            indicadorAux.descripcion = IEPM_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

            //IEPM NO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IEPMEntity IEPM_NO_CONTRATISTA = iepm.IEPM_NO_CONTRATISTA(fechaInicio, fechaFin);
            indicadorAux.nombre = "IEPM NO CONTRATISTA";
            indicadorAux.calculo = IEPM_NO_CONTRATISTA.CalcularIndicadorIEPM();
            indicadorAux.descripcion = IEPM_NO_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

            //RAIO GENERAL
            indicadorAux = new IndicadoresEntity();
            RAIOEntity RAIO_GENERAL = raio.RAIOGeneral(fechaInicio, fechaFin);
            indicadorAux.nombre = "RAIO GENERAL";
            indicadorAux.calculo = RAIO_GENERAL.CacularIndicadorRAIO();
            indicadorAux.descripcion = RAIO_GENERAL.ToString();
            indicadores.Add(indicadorAux);

            //RAIO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            RAIOEntity RAIO_CONTRATISTA = raio.RAIOContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "RAIO CONTRATISTA";
            indicadorAux.calculo = RAIO_CONTRATISTA.CacularIndicadorRAIO();
            indicadorAux.descripcion = RAIO_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

            //RAIO NO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            RAIOEntity RAIO_NO_CONTRATISTA = raio.RAIONoContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "RAIO NO CONTRATISTA";
            indicadorAux.calculo = RAIO_NO_CONTRATISTA.CacularIndicadorRAIO();
            indicadorAux.descripcion = RAIO_NO_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

            //RANO GENERAL
            indicadorAux = new IndicadoresEntity();
            RANOEntity RANO_GENERAL = rano.RANOGeneral(fechaInicio, fechaFin);
            indicadorAux.nombre = "RANO GENERAL";
            indicadorAux.calculo = RANO_GENERAL.CalcularIndicadorRANO();
            indicadorAux.descripcion = RANO_GENERAL.ToString();
            indicadores.Add(indicadorAux);

            //RANO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            RANOEntity RANO_CONTRATISTA = rano.RANOContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "RANO CONTRATISTA";
            indicadorAux.calculo = RANO_CONTRATISTA.CalcularIndicadorRANO();
            indicadorAux.descripcion = RANO_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

            //RANO NO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            RANOEntity RANO_NO_CONTRATISTA = rano.RANONoContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "RANO NO CONTRATISTA";
            indicadorAux.calculo = RANO_NO_CONTRATISTA.CalcularIndicadorRANO();
            indicadorAux.descripcion = RANO_NO_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

<<<<<<< Updated upstream
            //IRF GENERAL
            indicadorAux = new IndicadoresEntity();
            IRFEntity IRF_GENERAL = IRF.IRFGeneral(fechaInicio, fechaFin);
            indicadorAux.nombre = "IRF GENERAL";
            indicadorAux.calculo = IRF_GENERAL.calculoIRF();
            indicadorAux.descripcion = IRF_GENERAL.ToString();
            indicadores.Add(indicadorAux);

            //IRF CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IRFEntity IRF_CONTRATISTA = IRF.IRFContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "IRF CONTRATISTA";
            indicadorAux.calculo = IRF_CONTRATISTA.calculoIRF();
            indicadorAux.descripcion = IRF_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);

            //IRF NO CONTRATISTA
            indicadorAux = new IndicadoresEntity();
            IRFEntity IRF_NO_CONTRATISTA = IRF.IRFNoContratista(fechaInicio, fechaFin);
            indicadorAux.nombre = "IRF NO CONTRATISTA";
            indicadorAux.calculo = IRF_NO_CONTRATISTA.calculoIRF();
            indicadorAux.descripcion = IRF_NO_CONTRATISTA.ToString();
            indicadores.Add(indicadorAux);


=======
            Console.WriteLine("indicadores. " + indicadores);
            
>>>>>>> Stashed changes
            return indicadores;


        }
    }
}
