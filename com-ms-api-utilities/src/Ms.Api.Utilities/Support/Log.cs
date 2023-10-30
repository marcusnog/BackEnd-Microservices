namespace Ms.Api.Utilities.Support
{
    public static class LogTrace
    {
        public enum TipoTrace
        {
            Normal,
            Inicio,
            Fim
        }

        //public static String ArquivoCriarPedido = "ControleCriarPedido";

        public static void Gravar(string nomeClasse, string texto, TipoTrace tipoTrace)
        {
            //String pastaLog = String.Empty;

            //if (ConfigurationManager.AppSettings["pastaLogTrace"] != null)
            //{
            //    pastaLog = ConfigurationManager.AppSettings["pastaLogTrace"];
            //}

            //if (!String.IsNullOrEmpty(pastaLog))
            //{
            //    if (!Directory.Exists(pastaLog))
            //        Directory.CreateDirectory(pastaLog);

            //    var nomeArquivo = pastaLog + nomeClasse + " - " + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            //    #region Tipo Trace

            //    String textoTipoTrace = String.Empty;
            //    switch (tipoTrace)
            //    {
            //        case TipoTrace.Inicio:
            //            textoTipoTrace = "[INÍCIO] - ";
            //            break;
            //        case TipoTrace.Fim:
            //            textoTipoTrace = "[FIM] - ";
            //            break;
            //    }

            //    #endregion Tipo Trace

            //    using (StreamWriter writer = new StreamWriter(nomeArquivo, true))
            //    {
            //        writer.Write(String.Format("{0} => {1}{2}", DateTime.Now.ToString(), textoTipoTrace, texto));
            //        writer.WriteLine();
            //    }

            //}
        }

        public static class Arquivo
        {
            public static String CriarPedido = "ControleCriarPedido";
        }

        //public static class Parceiro
        //{
        //    public static String Giftty = "Giftty";
        //    public static String Magalu = "Magalu";
        //    public static String NetShoes = "NetShoes";
        //    public static String ViaVarejo = "ViaVarejo";

        //}
    }

}
