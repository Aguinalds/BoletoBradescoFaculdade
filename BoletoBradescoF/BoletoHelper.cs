

using BoletoNetCore;

namespace BoletoBradescoF
{
    public static class BoletoHelper
    {
        public static byte[] ImprimirCarnePdf(this Boletos listaBoletos)
        {
            return new BoletoCarne().BoletoPdf(listaBoletos);
        }
    }
}
