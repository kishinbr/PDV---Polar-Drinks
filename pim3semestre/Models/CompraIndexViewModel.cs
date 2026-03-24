using System.Collections.Generic;
using pim3semestre.Models;

namespace pim3semestre.ViewModels
{
    public class CompraIndexViewModel
    {
        public List<CompraEstoqueModel> Pendentes { get; set; } = new();
        public List<CompraEstoqueModel> Concluidas { get; set; } = new();
    }
}