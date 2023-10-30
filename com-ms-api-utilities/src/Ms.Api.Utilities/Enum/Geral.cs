namespace Ms.Api.Utilities.Enum
{
    public class Enums
    {
        public enum Provider
        {
            //TODOS = 0,
            //NOVAPONTOCOM = 1,
            //FASTSHOP = 2,
            //SBox = 3,
            //Giftty = 4,
            //Unicef = 5,
            //Multiplus = 6,
            //Decolar = 7,
            //BK = 8,
            //NetShoes = 9,
            //Cargill = 10,
            //NetShoesOnLine = 66,
            //ITLog = 12,
            //MagaLu = 13,
            //Petite = 14,
            //Total = 99,
            //Tray = 15,
            //RamStore = 16
            Nenhum,
            TODOS,
            NOVAPONTOCOM,
            FASTSHOP,
            SBox,
            Giftty,
            Unicef,
            Multiplus,
            Decolar,
            BK,
            NetShoes,
            Cargill,
            NetShoesOnLine,
            ITLog,
            MagaLu,
            Petite,
            Total,
            Tray,
            RamStore
        }

        public enum Campaign
        {
            Nenhum = 0,
            Samsung_MaisSamsung = 350,
            Samsung_ConexaoAzul = 351,
            SempTCL_ClubeS = 360
        }

        public enum Store
        {
            Nenhum,
            NetShoes,
            Extra,
            Ponto,
            CasasBahia,
            Magalu,
            Giftty
        }

        //public enum TipoDeErro
        //{
        //    NaoEspecificado,
        //    Excecao,
        //    ResultadoVazio,
        //    Validacao,
        //    Autorizacao,
        //    Negocios
        //}

        public enum TypeRequest
        {
            Produto,
            Boleto,
            Recarga
        }
    }
}
