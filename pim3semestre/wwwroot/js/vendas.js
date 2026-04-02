document.addEventListener("DOMContentLoaded", () => {

    // ==================== ELEMENTOS ====================
    const buscaInput = document.getElementById("buscaProduto");
    const lista = document.getElementById("listaSugestoes");

    const qtdInput = document.getElementById("quantidade");
    qtdInput.addEventListener("input", () => {
        if (qtdInput.value < 0) {
            qtdInput.value = "";
        }
    });
    const valorUnitario = document.getElementById("valorUnitario");
    const valorTotal = document.getElementById("valorTotal");
    const tabela = document.getElementById("tabelaItens");
    const totalVendaLabel = document.getElementById("totalVenda");
    const itensHidden = document.getElementById("itensHidden");

    const btnConfirmar = document.getElementById("btnConfirmar");
    const radiosPagamento = document.querySelectorAll('input[name="VendaTipoPagamento"]');

    let totalVenda = 0;
    let produtoSelecionado = null;
    let indiceSelecionado = -1;

    // ==================== FUNÇÕES AUXILIARES ====================
    function formatarMoeda(valor) {
        return "R$ " + parseFloat(valor).toFixed(2).replace(".", ",");
    }

    function atualizarBotaoConfirmar() {
        const pagamentoSelecionado = document.querySelector('input[name="VendaTipoPagamento"]:checked');
        const itens = itensHidden.querySelectorAll('input[name$=".ProdutoID"]');
        btnConfirmar.disabled = !(pagamentoSelecionado && itens.length > 0);
    }

    // ==================== BUSCA DE PRODUTOS ====================
    function renderLista(filtro = "") {
        lista.innerHTML = "";
        indiceSelecionado = -1;

        const filtrados = produtos
            .filter(p => p.produtoNome.toLowerCase().includes(filtro) || p.produtoCodBarra.toLowerCase().includes(filtro))
            .slice(0, 8);

        filtrados.forEach(p => {
            const item = document.createElement("a");
            item.classList.add("list-group-item", "list-group-item-action");

            // Flex: nome/código à esquerda, estoque à direita
            item.innerHTML = `
            <div style="display:flex; justify-content:space-between;">
                <span>${p.produtoNome} [${p.produtoCodBarra}]</span>
                <span>${p.produtoQtdEstoque} QTD</span>
            </div>
        `;

            item.onclick = () => selecionarProduto(p);
            lista.appendChild(item);
        });
    }

    function selecionarProduto(p) {
        produtoSelecionado = p;
        buscaInput.value = p.produtoNome;
        lista.innerHTML = "";
        valorUnitario.value = formatarMoeda(p.produtoPrecoVenda);
        calcularTotal();
    }

    buscaInput.addEventListener("focus", () => renderLista(buscaInput.value.toLowerCase()));
    buscaInput.addEventListener("input", () => renderLista(buscaInput.value.toLowerCase()));

    document.addEventListener("click", e => {
        if (!buscaInput.contains(e.target) && !lista.contains(e.target)) lista.innerHTML = "";
    });

    buscaInput.addEventListener("keydown", e => {
        let itens = lista.querySelectorAll(".list-group-item");
        if (!itens.length) return;

        if (e.key === "ArrowDown") { e.preventDefault(); indiceSelecionado = (indiceSelecionado + 1) % itens.length; }
        if (e.key === "ArrowUp") { e.preventDefault(); indiceSelecionado = (indiceSelecionado - 1 + itens.length) % itens.length; }
        if (e.key === "Enter") { e.preventDefault(); if (indiceSelecionado >= 0) itens[indiceSelecionado].click(); return; }

        itens.forEach(i => i.classList.remove("active"));
        if (indiceSelecionado >= 0) itens[indiceSelecionado].classList.add("active");
    });

    // ==================== CÁLCULO DE TOTAL ====================
    function calcularTotal() {
        const qtd = parseFloat(qtdInput.value) || 0;
        const preco = produtoSelecionado ? produtoSelecionado.produtoPrecoVenda : 0;
        valorTotal.value = formatarMoeda(qtd * preco);
    }

    qtdInput.addEventListener("input", calcularTotal);

    // ==================== ADICIONAR ITEM ====================
    document.getElementById("btnAdicionar").addEventListener("click", () => {
        // nova div de erro específica
        let msgAdicionar = document.getElementById("mensagemErroAdicionar");
        if (!msgAdicionar) {
            msgAdicionar = document.createElement("div");
            msgAdicionar.id = "mensagemErroAdicionar";
            msgAdicionar.classList.add("alert", "alert-danger", "mt-2");
            msgAdicionar.style.display = "none";
            document.getElementById("formVenda").prepend(msgAdicionar);
        }

        // função para mostrar erro com fade
        function mostrarErro(texto) {
            msgAdicionar.innerText = texto;
            msgAdicionar.style.opacity = 1;
            msgAdicionar.style.display = "block";
            msgAdicionar.style.transition = "opacity 0.5s";

            setTimeout(() => {
                msgAdicionar.style.opacity = 0;
                setTimeout(() => {
                    msgAdicionar.style.display = "none";
                    msgAdicionar.innerText = "";
                }, 500); // tempo do fade
            }, 2000);
        }

        // limpa mensagem anterior
        msgAdicionar.style.display = "none";
        msgAdicionar.innerText = "";

        // valida produto selecionado
        if (!produtoSelecionado) {
            mostrarErro("Selecione um produto antes de adicionar!");
            return;
        }

        // valida quantidade
        let qtd = parseInt(qtdInput.value) || 0;
        if (qtd <= 0) {
            mostrarErro("Quantidade inválida!");
            return;
        }

        // Verifica estoque já adicionado
        let qtdJaAdicionada = 0;
        tabela.querySelectorAll("tr").forEach(linha => {
            if (parseInt(linha.getAttribute("data-id")) === produtoSelecionado.produtoID) {
                qtdJaAdicionada += parseInt(linha.children[1].innerText);
            }
        });

        // valida estoque
        if (qtd + qtdJaAdicionada > produtoSelecionado.produtoQtdEstoque) {
            mostrarErro(`Estoque insuficiente! Disponível: ${produtoSelecionado.produtoQtdEstoque - qtdJaAdicionada}`);
            return;
        }

        const preco = produtoSelecionado.produtoPrecoVenda;
        const total = qtd * preco;

        // Adiciona linha na tabela
        const linha = document.createElement("tr");
        linha.setAttribute("data-id", produtoSelecionado.produtoID);
        linha.innerHTML = `
        <td>${produtoSelecionado.produtoNome}</td>
        <td class="text-end">${qtd}</td>
        <td class="text-end">${formatarMoeda(preco)}</td>
        <td class="text-end">${formatarMoeda(total)}</td>
        <td class="text-center">
            <button type="button" class="btn btn-danger btn-sm btn-remover">
                <i class="bi bi-trash"></i>
            </button>
        </td>
    `;
        tabela.appendChild(linha);

        // Adiciona hidden inputs
        const index = tabela.children.length - 1;
        itensHidden.insertAdjacentHTML('beforeend', `
        <input type="hidden" name="Itens[${index}].ProdutoID" value="${produtoSelecionado.produtoID}" />
        <input type="hidden" name="Itens[${index}].ItemVendaQtd" value="${qtd}" />
        <input type="hidden" name="Itens[${index}].ItemVendaPreco" value="${preco}" />
        <input type="hidden" name="Itens[${index}].ItemVendaTotal" value="${total}" />
    `);

        totalVenda += total;
        totalVendaLabel.innerText = formatarMoeda(totalVenda);

        // Limpa seleção
        produtoSelecionado = null;
        buscaInput.value = "";
        qtdInput.value = 0;
        valorUnitario.value = "";
        valorTotal.value = "";

        // Atualiza botão confirmar
        atualizarBotaoConfirmar();
    });

    // ==================== REMOVER ITEM ====================
    tabela.addEventListener("click", e => {
        const btn = e.target.closest(".btn-remover");
        if (!btn) return;

        const row = btn.closest("tr");
        const index = Array.from(tabela.children).indexOf(row);

        const total = parseFloat(row.children[3].innerText.replace("R$", "").replace(/\./g, "").replace(",", "."));
        totalVenda -= total;
        totalVendaLabel.innerText = formatarMoeda(totalVenda);

        row.remove();

        // Remove hidden inputs
        const inputs = itensHidden.querySelectorAll(`[name^="Itens[${index}]"]`);
        inputs.forEach(i => i.remove());

        // Atualiza botão confirmar
        atualizarBotaoConfirmar();
    });

    // ==================== RADIO PAGAMENTO ====================
    radiosPagamento.forEach(radio => {
        radio.addEventListener("change", atualizarBotaoConfirmar);
    });



    // ==================== INICIALIZAÇÃO ====================
    atualizarBotaoConfirmar(); // garante botão correto ao carregar
});