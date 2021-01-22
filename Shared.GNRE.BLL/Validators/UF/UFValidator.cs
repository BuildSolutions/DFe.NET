using System;
using System.Collections.Generic;
using DFe.Classes.Entidades;
using FluentValidation;
using GNRE.BLL.Validators.UF.Acre;
using GNRE.BLL.Validators.UF.Alagoas;
using GNRE.BLL.Validators.UF.Amapa;
using GNRE.BLL.Validators.UF.Amazonas;
using GNRE.BLL.Validators.UF.Bahia;
using GNRE.BLL.Validators.UF.Ceara;
using GNRE.BLL.Validators.UF.DistritoFederal;
using GNRE.BLL.Validators.UF.Goias;
using GNRE.BLL.Validators.UF.Maranhao;
using GNRE.BLL.Validators.UF.MatoGrosso;
using GNRE.BLL.Validators.UF.MinasGerais;
using GNRE.BLL.Validators.UF.Para;
using GNRE.BLL.Validators.UF.Paraiba;
using GNRE.BLL.Validators.UF.Parana;
using GNRE.BLL.Validators.UF.Pernambuco;
using GNRE.BLL.Validators.UF.Piaui;
using GNRE.BLL.Validators.UF.RioDeJaneiro;
using GNRE.BLL.Validators.UF.RioGrandeDoNorte;
using GNRE.BLL.Validators.UF.RioGrandeDoSul;
using GNRE.BLL.Validators.UF.Rondonia;
using GNRE.BLL.Validators.UF.Roraima;
using GNRE.BLL.Validators.UF.SantaCatarina;
using GNRE.BLL.Validators.UF.Sergipe;
using GNRE.BLL.Validators.UF.Tocantins;

namespace GNRE.BLL.Validators.UF
{
    public class UFValidator : AbstractValidator<Configuracao.Entidades.GNRE>
    {
        public UFValidator()
        {
            RuleFor(uf => uf.Emitente).NotNull().WithMessage("Dados do emitente não informado!").DependentRules(() => RuleFor(nfe => nfe.Emitente).SetValidator(new EmitenteValidator()));
            RuleFor(uf => uf.ValorST).GreaterThan(0).WithMessage("Valor total da ST é inválido!");
            RuleFor(uf => uf.ValorTotal).Equal(uf => uf.ValorST + uf.ValorFECP.GetValueOrDefault()).WithMessage("Valor total da GNRE não confere com o valor da ST + valor FECP!");
            RuleFor(uf => uf.UF).Must(uf => EstadosComGNRE.Contains(uf)).WithMessage(uf => $"Não é possível emitir gnre para a UF: {uf.UF}")
                .DependentRules(() =>
                {
                    When(uf => uf.UF == Estado.AC, () => RuleFor(uf => uf).SetValidator(new AcreValidator()));
                    When(uf => uf.UF == Estado.AL, () => RuleFor(uf => uf).SetValidator(new AlagoasValidator()));
                    When(uf => uf.UF == Estado.AM, () => RuleFor(uf => uf).SetValidator(new AmazonasValidator()));
                    When(uf => uf.UF == Estado.AP, () => RuleFor(uf => uf).SetValidator(new AmapaValidator()));
                    When(uf => uf.UF == Estado.BA, () => RuleFor(uf => uf).SetValidator(new BahiaValidator()));
                    When(uf => uf.UF == Estado.CE, () => RuleFor(uf => uf).SetValidator(new CearaValidator()));
                    When(uf => uf.UF == Estado.DF, () => RuleFor(uf => uf).SetValidator(new DistritoFederalValidator()));
                    When(uf => uf.UF == Estado.GO, () => RuleFor(uf => uf).SetValidator(new GoiasValidator()));
                    When(uf => uf.UF == Estado.MA, () => RuleFor(uf => uf).SetValidator(new MaranhaoValidator()));
                    When(uf => uf.UF == Estado.MG, () => RuleFor(uf => uf).SetValidator(new MinasGeraisValidator()));
                    When(uf => uf.UF == Estado.MT, () => RuleFor(uf => uf).SetValidator(new MatoGrossoValidator()));
                    When(uf => uf.UF == Estado.PA, () => RuleFor(uf => uf).SetValidator(new ParaValidator()));
                    When(uf => uf.UF == Estado.PB, () => RuleFor(uf => uf).SetValidator(new ParaibaValidator()));
                    When(uf => uf.UF == Estado.PE, () => RuleFor(uf => uf).SetValidator(new PernambucoValidator()));
                    When(uf => uf.UF == Estado.PI, () => RuleFor(uf => uf).SetValidator(new PiauiValidator()));
                    When(uf => uf.UF == Estado.PR, () => RuleFor(uf => uf).SetValidator(new ParanaValidator()));
                    When(uf => uf.UF == Estado.RJ, () => RuleFor(uf => uf).SetValidator(new RioDeJaneiroValidator()));
                    When(uf => uf.UF == Estado.RN, () => RuleFor(uf => uf).SetValidator(new RioGrandeDoNorteValidator()));
                    When(uf => uf.UF == Estado.RO, () => RuleFor(uf => uf).SetValidator(new RondoniaValidator()));
                    When(uf => uf.UF == Estado.RR, () => RuleFor(uf => uf).SetValidator(new RoraimaValidator()));
                    When(uf => uf.UF == Estado.RS, () => RuleFor(uf => uf).SetValidator(new RioGrandeDoSulValidator()));
                    When(uf => uf.UF == Estado.SC, () => RuleFor(uf => uf).SetValidator(new SantaCatarinaValidator()));
                    When(uf => uf.UF == Estado.SE, () => RuleFor(uf => uf).SetValidator(new SergipeValidator()));
                    When(uf => uf.UF == Estado.TO, () => RuleFor(uf => uf).SetValidator(new TocantinsValidator()));
                });
        }

        public static bool IsDateTime(string valor)
        {
            return DateTime.TryParse(valor, out DateTime dataAux)
                && dataAux != DateTime.MinValue;
        }

        public static bool IsNumber(string valor)
        {
            return double.TryParse(valor, out double numberAux)
                && numberAux > 0;
        }

        /// <summary>
        /// Devolve a lista de enums do tipo <see cref="Estado"/> que são Tributados/>
        /// </summary>
        public static ISet<Estado> EstadosComGNRE = new HashSet<Estado>()
        {
            Estado.AC,
            Estado.AL,
            Estado.AM,
            Estado.AP,
            Estado.BA,
            Estado.CE,
            Estado.DF,
            Estado.GO,
            Estado.MA,
            Estado.MG,
            Estado.MT,
            Estado.PA,
            Estado.PB,
            Estado.PE,
            Estado.PI,
            Estado.PR,
            Estado.RJ,
            Estado.RN,
            Estado.RO,
            Estado.RR,
            Estado.RS,
            Estado.SC,
            Estado.SE,
            Estado.TO
        };
    }
}
