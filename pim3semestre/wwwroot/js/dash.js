let graficoPagamentos;
let graficoVendas;

document.addEventListener("DOMContentLoaded", () => {

    // 🔥 pega dados da view
    const pagamentos = window.dashData.pagamentos;
    const vendas = window.dashData.vendas;

    // ================== PAGAMENTOS ==================
    graficoPagamentos = new Chart(document.getElementById('graficoPagamentos'), {
        type: 'pie',
        data: {
            labels: ['Pix', 'Cartão', 'Dinheiro'],
            datasets: [{
                data: pagamentos.hoje,
                backgroundColor: ['#198754', '#0D6EFD', '#f59e0b']
            }]
        }
    });

    window.atualizarPagamentos = function (tipo) {

        const dados = pagamentos[tipo];

        graficoPagamentos.data.datasets[0].data = dados;
        graficoPagamentos.update();

        const total = dados.reduce((a, b) => a + b, 0);
        document.getElementById("qtdPagamentosLabel").innerText =
            `Qtd vendas: ${total}`;
    };

    // ================== VENDAS ==================
    graficoVendas = new Chart(document.getElementById('graficoVendas'), {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: 'Vendas',
                data: [],
                backgroundColor: '#4f8ef7'
            }]
        },

        options: {
            scales: {

                // 🔵 EIXO X (embaixo das colunas)
                x: {
                    ticks: {
                        color: '#ffffff' // 👈 TEXTO BRANCO
                    },
                    grid: {
                        color: 'rgba(255,255,255,0.05)' // opcional (linhas suaves)
                    }
                },

                // 🔵 EIXO Y (lado esquerdo)
                y: {
                    ticks: {
                        color: '#ffffff' // 👈 TEXTO BRANCO
                    },
                    grid: {
                        color: 'rgba(255,255,255,0.05)' // opcional
                    }
                }
            },

            plugins: {
                legend: {
                    labels: {
                        color: '#ffffff' // 👈 "Vendas" legenda
                    }
                }
            }
        }
    });

    window.atualizarVendas = function (tipo) {

        const dados = vendas[tipo];
        let labels = [];

        if (tipo === "hoje") {
            labels = dados.map((_, i) => `${i}h`);
        }

        else if (tipo === "semana") {
            const dias = ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"];
            const hoje = new Date();

            labels = dados.map((_, i) => {
                let data = new Date();
                data.setDate(hoje.getDate() - (dados.length - 1 - i));
                return dias[data.getDay()];
            });
        }

        else if (tipo === "mes") {
            labels = dados.map((_, i) => `${i + 1}`);
        }

        else if (tipo === "ano") {
            const meses = ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"];
            const hoje = new Date();

            labels = dados.map((_, i) => {
                let data = new Date();
                data.setMonth(hoje.getMonth() - (dados.length - 1 - i));
                return meses[data.getMonth()];
            });
        }

        graficoVendas.data.labels = labels;
        graficoVendas.data.datasets[0].data = dados;
        graficoVendas.update();

        const total = dados.reduce((a, b) => a + b, 0);
        document.getElementById("totalVendasLabel").innerText =
            `Qtd vendas: ${total}`;
    };

    // inicialização
    atualizarVendas("semana");
    atualizarPagamentos("hoje");
});