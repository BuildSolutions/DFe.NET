using System.ComponentModel;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators
{
    public enum EImpostoReformaTributariaTipoSimplificado : uint
    {
        [Description("CBS")] CBS = 0,
        [Description("IBS")] IBS = 1,
    }
}