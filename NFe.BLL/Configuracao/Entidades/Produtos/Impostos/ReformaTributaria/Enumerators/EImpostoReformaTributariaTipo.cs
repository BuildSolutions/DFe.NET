using System.ComponentModel;

namespace NFe.BLL.Configuracao.Entidades.Produtos.Impostos.ReformaTributaria.Enumerators
{
    public enum EImpostoReformaTributariaTipo : uint
    {
        [Description("CBS")] CBS = 0,
        [Description("IBSUF")] IBSUF = 1,
        [Description("IBSMUN")] IBSMUN = 2,
    }
}