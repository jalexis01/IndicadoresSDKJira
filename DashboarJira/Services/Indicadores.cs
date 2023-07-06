using DashboarJira.Controller;
using DashboarJira.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DashboarJira.Services
{
    public class Indicadores
    {
        private readonly DbConnector con;
        private readonly JiraAccess jira;
        private readonly IAIOController iaio;
        private readonly IANOController iano;
        private readonly ICPMController icpm;
        private readonly IEPMController iepm;
        private readonly RAIOController raio;
        private readonly RANOController rano;

        public Indicadores()
        {
            con = new DbConnector();
            jira = new JiraAccess();
            iaio = new IAIOController(jira);
            iano = new IANOController(jira);
            icpm = new ICPMController(jira);
            iepm = new IEPMController(jira);
            raio = new RAIOController(jira);
            rano = new RANOController(jira);
        }

        public List<IndicadoresEntity> ObtenerIndicadores(string fechaInicio, string fechaFin)
        {
            List<Task<IndicadoresEntity>> tareas = new List<Task<IndicadoresEntity>>();

            //IAIO
            tareas.Add(Task.Run(() => CalcularIndicadorIAIOGeneral(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularIndicadorIAIOContratista(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularIndicadorIAIONoContratista(fechaInicio, fechaFin)));
            //IANO
            tareas.Add(Task.Run(() => CalcularIndicadorIANOGeneral(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularIndicadorIANOContratista(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularIndicadorIANONoContratista(fechaInicio, fechaFin)));
            //ICPM
            tareas.Add(Task.Run(() => CalcularICPMRFID(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularICPMITTS(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularICPMMTTO(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularICPMPuertas(fechaInicio, fechaFin)));
            //IEPM
            tareas.Add(Task.Run(() => CalcularIEPMContratista(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularIEPMNoContratista(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularIEPM(fechaInicio, fechaFin)));
            //RAIO
            tareas.Add(Task.Run(() => CalcularRAIO(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularRAIOContratista(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularRAIONoContratista(fechaInicio, fechaFin)));
            //RANO
            tareas.Add(Task.Run(() => CalcularRANO(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularRANOContratista(fechaInicio, fechaFin)));
            tareas.Add(Task.Run(() => CalcularRANONoContratista(fechaInicio, fechaFin)));

            // **************************************************************************************************************************************************************************************************************************************************************************************************
            Task.WaitAll(tareas.ToArray());

            List<IndicadoresEntity> indicadores = new List<IndicadoresEntity>();

            foreach (var tarea in tareas)
            {
                indicadores.Add(tarea.Result);
            }

            return indicadores;
        }

        // **************************************************************************************************************************************************************************************************************************************************************************************************
        //IAIO


        private IndicadoresEntity CalcularIndicadorIAIOGeneral(string fechaInicio, string fechaFin)
        {
            IAIOEntity IAIOGeneral = iaio.IAIOGeneral(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IAIO GENERAL", IAIOGeneral.CalcularIndicadorIAIO(), IAIOGeneral.ToString());
        }

        private IndicadoresEntity CalcularIndicadorIAIOContratista(string fechaInicio, string fechaFin)
        {
            IAIOEntity IAIOContratista = iaio.IAIOContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IAIO CONTRATISTA", IAIOContratista.CalcularIndicadorIAIO(), IAIOContratista.ToString());
        }

        private IndicadoresEntity CalcularIndicadorIAIONoContratista(string fechaInicio, string fechaFin)
        {
            IAIOEntity IAIONoContratista = iaio.IAIONoContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IAIO NO CONTRATISTA", IAIONoContratista.CalcularIndicadorIAIO(), IAIONoContratista.ToString());
        }
        
        // **************************************************************************************************************************************************************************************************************************************************************************************************
        //IANO


        private IndicadoresEntity CalcularIndicadorIANOGeneral(string fechaInicio, string fechaFin)
        {
            IANOEntity IANOGeneral = iano.IANOGeneral(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IANO GENERAL", IANOGeneral.CalcularIndicadorIANO(), IANOGeneral.ToString());
        }


        private IndicadoresEntity CalcularIndicadorIANOContratista(string fechaInicio, string fechaFin)
        {
            IANOEntity IANOContratista = iano.IANOContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IANO CONTRATISTA", IANOContratista.CalcularIndicadorIANO(), IANOContratista.ToString());
        }

        private IndicadoresEntity CalcularIndicadorIANONoContratista(string fechaInicio, string fechaFin)
        {
            IANOEntity IANONoContratista = iano.IANO_NO_Contratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IANO NO CONTRATISTA", IANONoContratista.CalcularIndicadorIANO(), IANONoContratista.ToString());
        }


        // **************************************************************************************************************************************************************************************************************************************************************************************************
        //ICPM
        private IndicadoresEntity CalcularICPMITTS(string fechaInicio, string fechaFin)
        {
            ICPMEntity ICPMITTS = icpm.ICPM_ITTS(fechaInicio, fechaFin);
            double calculoITTS = ICPMITTS.CalcularIndicadorICPM();
            string descripcion = ICPMITTS.ToString();
           

            return CrearIndicadorEntity("ICPM ITTS ", calculoITTS, descripcion);
        }
        private IndicadoresEntity CalcularICPMMTTO(string fechaInicio, string fechaFin)
        {
           ICPMEntity ICPMMTTO = icpm.ICPM_MTTO(fechaInicio, fechaFin);
            double calculoMTTO = ICPMMTTO.CalcularIndicadorICPM();
            string descripcion = ICPMMTTO.ToString();

            return CrearIndicadorEntity("ICPM MTTO", calculoMTTO, descripcion);

        }
        private IndicadoresEntity CalcularICPMPuertas(string fechaInicio, string fechaFin)
        {
            ICPMEntity ICPMPuertas = icpm.ICPM_PUERTAS(fechaInicio, fechaFin);
            double calculoPuertas = ICPMPuertas.CalcularIndicadorICPM();
            string descripcion = ICPMPuertas.ToString();
            return CrearIndicadorEntity("ICPM Puertas", calculoPuertas, descripcion);
        }

        private IndicadoresEntity CalcularICPMRFID(string fechaInicio, string fechaFin)
        {

            ICPMEntity ICPMRFID = icpm.ICPM_RFID(fechaInicio, fechaFin);
            double calculoRFID = ICPMRFID.CalcularIndicadorICPM();
            string descripcion = ICPMRFID.ToString();

            return CrearIndicadorEntity("ICPM RFID", calculoRFID, descripcion);
        }


        // **************************************************************************************************************************************************************************************************************************************************************************************************
        //IEMPM
        private IndicadoresEntity CalcularIEPM(string fechaInicio, string fechaFin)
        {
            IEPMEntity IEPM_GENERAL = iepm.IEPM_GENERAL(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IEPM GENERAL", IEPM_GENERAL.CalcularIndicadorIEPM(), IEPM_GENERAL.ToString());
        }

        private IndicadoresEntity CalcularIEPMContratista(string fechaInicio, string fechaFin)
        {
            IEPMEntity IEPM_CONTRATISTA = iepm.IEPM_CONTRATISTA(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IEPM Contratista", IEPM_CONTRATISTA.CalcularIndicadorIEPM(), IEPM_CONTRATISTA.ToString());
        }

        private IndicadoresEntity CalcularIEPMNoContratista(string fechaInicio, string fechaFin)
        {
            IEPMEntity IEPM_NO_CONTRATISTA = iepm.IEPM_NO_CONTRATISTA(fechaInicio, fechaFin);
            return CrearIndicadorEntity("IEPM No Contratista", IEPM_NO_CONTRATISTA.CalcularIndicadorIEPM(), IEPM_NO_CONTRATISTA.ToString());
        }


        // **************************************************************************************************************************************************************************************************************************************************************************************************
        //RAIO
        private IndicadoresEntity CalcularRAIO(string fechaInicio, string fechaFin)
        {
            RAIOEntity RAIO_GENERAL = raio.RAIOGeneral(fechaInicio, fechaFin);
            return CrearIndicadorEntity("RAIO GENERAL", RAIO_GENERAL.CacularIndicadorRAIO(), RAIO_GENERAL.ToString());
        }
        private IndicadoresEntity CalcularRAIOContratista(string fechaInicio, string fechaFin)
        {
            RAIOEntity RAIOContratista = raio.RAIOContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("RAIO GENERAL", RAIOContratista.CacularIndicadorRAIO(), RAIOContratista.ToString());
        }
        private IndicadoresEntity CalcularRAIONoContratista(string fechaInicio, string fechaFin)
        {
            RAIOEntity RAIONoContratista = raio.RAIONoContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("RAIO GENERAL", RAIONoContratista.CacularIndicadorRAIO(), RAIONoContratista.ToString());
        }
        // **************************************************************************************************************************************************************************************************************************************************************************************************
        //RANO
        private IndicadoresEntity CalcularRANO(string fechaInicio, string fechaFin)
        {
            RANOEntity RANO_GENERAL = rano.RANOGeneral(fechaInicio, fechaFin);
            return CrearIndicadorEntity("RANO GENERAL", RANO_GENERAL.CalcularIndicadorRANO(), RANO_GENERAL.ToString());
        }
        private IndicadoresEntity CalcularRANOContratista(string fechaInicio, string fechaFin)
        {
            RANOEntity RANOContratista = rano.RANOContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("RANO GENERAL", RANOContratista.CalcularIndicadorRANO(), RANOContratista.ToString());
        }
        private IndicadoresEntity CalcularRANONoContratista(string fechaInicio, string fechaFin)
        {
            RANOEntity RANONoContratista = rano.RANONoContratista(fechaInicio, fechaFin);
            return CrearIndicadorEntity("RANO GENERAL", RANONoContratista.CalcularIndicadorRANO(), RANONoContratista.ToString());
        }


        // **************************************************************************************************************************************************************************************************************************************************************************************************
        private IndicadoresEntity CrearIndicadorEntity(string nombre, double calculo, string descripcion)
        {
            return new IndicadoresEntity
            {
                nombre = nombre,
                calculo = calculo,
                descripcion = descripcion
            };
        }
    }
}
