using DashboarJira.Controller;

namespace DashboarJira.Services
{
    public class Indicadores
    {
        public dynamic indicadores(string fechaInicio, string fechaFin) {
            IAIOController iaio = new IAIOController();
            IANOController iano = new IANOController();
            ICPMController icpm = new ICPMController();
            IEPMController iepm = new IEPMController();
            RAIOController raio = new RAIOController();
            RANOController rano = new RANOController();

            // Realizar operaciones y guardar resultados en un objeto
            var resultados = new
            {
                IAIO = new
                {
                    General = iaio.IAIOGeneral(fechaInicio, fechaFin).CalcularIndicadorIAIO(),
                    Contratista = iaio.IAIOContratista(fechaInicio, fechaFin).CalcularIndicadorIAIO(),
                    NoContratista = iaio.IAIONoContratista(fechaInicio, fechaFin).CalcularIndicadorIAIO()
                },
                IANO = new
                {
                    General = iano.IANOGeneral(fechaInicio, fechaFin).CalcularIndicadorIANO(),
                    Contratista = iano.IANOContratista(fechaInicio, fechaFin).CalcularIndicadorIANO(),
                    NoContratista = iano.IANO_NO_Contratista(fechaInicio, fechaFin).CalcularIndicadorIANO()
                },
                ICPM = new
                {
                    ITTS = icpm.ICPM_ITTS(fechaInicio, fechaFin).CalcularIndicadorICPM(),
                    MTTO = icpm.ICPM_MTTO(fechaInicio, fechaFin).CalcularIndicadorICPM(),
                    Puertas = icpm.ICPM_PUERTAS(fechaInicio, fechaFin).CalcularIndicadorICPM(),
                    RFID = icpm.ICPM_RFID(fechaInicio, fechaFin).CalcularIndicadorICPM()
                },
                IEPM = new
                {
                    General = iepm.IEPM_GENERAL(fechaInicio, fechaFin).CalcularIndicadorIEPM(),
                    Contratista = iepm.IEPM_CONTRATISTA(fechaInicio, fechaFin).CalcularIndicadorIEPM(),
                    NoContratista = iepm.IEPM_NO_CONTRATISTA(fechaInicio, fechaFin).CalcularIndicadorIEPM()
                },
                RAIO = new
                {
                    General = raio.RAIOGeneral(fechaInicio, fechaFin).CacularIndicadorRAIO(),
                    Contratista = raio.RAIOContratista(fechaInicio, fechaFin).CacularIndicadorRAIO(),
                    NoContratista = raio.RAIONoContratista(fechaInicio, fechaFin).CacularIndicadorRAIO()
                },
                RANO = new
                {
                    General = rano.RAIOGeneral(fechaInicio, fechaFin).CacularIndicadorRAIO(),
                    Contratista = rano.RAIOContratista(fechaInicio, fechaFin).CacularIndicadorRAIO(),
                    NoContratista = rano.RAIONoContratista(fechaInicio, fechaFin).CacularIndicadorRAIO()
                }
            };

            // Serializar objeto a JSON
            return resultados;

        }
    }
}
