using BoletoNetCore;


namespace BoletoBradescoF
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var denovo = true;
                while (denovo)
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    Console.Write("Informe o diretório para gerar o pdf: ");
                    currentDir = Console.ReadLine();
                    if (!Directory.Exists(currentDir))
                        throw new Exception("O diretório informado não existe: " + currentDir);

                    Console.WriteLine("Aguarde, gerando Boleto e Remessa");

                    var contaBancaria = new ContaBancaria
                    {
                        Agencia = "0156",
                        Conta = "85305",
                        DigitoConta = "4",
                        CarteiraPadrao = "09",
                        TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                        VariacaoCarteiraPadrao = "",
                        TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                        TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa,
                        OperacaoConta = "05"

                    };
                    var banco = Banco.Instancia(Bancos.Bradesco);
                    banco.Beneficiario = Utils.GerarBeneficiario("85305", "", "", contaBancaria);
                    banco.FormataBeneficiario();
                    var boletos = Utils.GerarBoletos(banco, 1, "N", 10);

                    var bytesremessa = boletos.GerarRemessa(1, TipoArquivo.CNAB240);
                    Console.WriteLine("Remessa gerado, salvando arquivo...");
                    var fileNameRemessa = Path.Combine(currentDir, "remessa.txt");
                    using (var memStream = new MemoryStream())
                    {
                        bytesremessa.CopyTo(memStream);
                        File.WriteAllBytes(fileNameRemessa, memStream.ToArray());
                    }
                    Console.WriteLine("Arquivo de remessa gerado com sucesso: " + fileNameRemessa);

                    var bytes = boletos.ImprimirCarnePdf();
                    Console.WriteLine("Pdf gerado, salvando arquivo...");
                    var fileName = Path.Combine(currentDir, "boleto.pdf");
                    File.WriteAllBytes(fileName, bytes);
                    Console.WriteLine("Pdf gerado com sucesso: " + fileName);

                    Console.Write("Digite 'N'/'Não' para sair! \n");
                    var contiuar = Console.ReadLine();
                    if (contiuar == "N" || contiuar == "n" || contiuar == "NAO" || contiuar == "nao" || contiuar == "NÃO" || contiuar == "não" || contiuar == "Não")
                    {
                        denovo = false;
                    }
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
    }
    }
}
