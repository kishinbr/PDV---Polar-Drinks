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

    function calcularPrecoComDesconto(p) {
        let preco = p.produtoPrecoVenda;

        if (p.produtoPromocao > 0) {
            preco = preco - (preco * (p.produtoPromocao / 100));
        }

        return preco;
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

            let preco = p.produtoPrecoVenda;
            let precoFinal = calcularPrecoComDesconto(p);

            item.innerHTML = `
            <div style="display:flex; justify-content:space-between;">
                <span>
                    ${p.produtoNome} [${p.produtoCodBarra}]
                    ${p.produtoPromocao > 0 ? `<span class="badge bg-danger ms-2">-${p.produtoPromocao}%</span>` : ""}
                </span>
                <span>
                    ${p.produtoPromocao > 0
                    ? `<small style="text-decoration:line-through;">R$ ${preco.toFixed(2)}</small> 
                           <strong class="text-success">R$ ${precoFinal.toFixed(2)}</strong>`
                    : `R$ ${preco.toFixed(2)}`
                }
                </span>
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

        let preco = calcularPrecoComDesconto(p);

        valorUnitario.value = formatarMoeda(preco);

        // salva preço com desconto
        produtoSelecionado.precoCalculado = preco;

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

        const preco = produtoSelecionado
            ? (produtoSelecionado.precoCalculado || produtoSelecionado.produtoPrecoVenda)
            : 0;

        valorTotal.value = formatarMoeda(qtd * preco);
    }

    qtdInput.addEventListener("input", calcularTotal);

    // ==================== ADICIONAR ITEM ====================
    document.getElementById("btnAdicionar").addEventListener("click", () => {

        let msgAdicionar = document.getElementById("mensagemErroAdicionar");
        if (!msgAdicionar) {
            msgAdicionar = document.createElement("div");
            msgAdicionar.id = "mensagemErroAdicionar";
            msgAdicionar.classList.add("alert", "alert-danger", "mt-2");
            msgAdicionar.style.display = "none";
            document.getElementById("formVenda").prepend(msgAdicionar);
        }

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
                }, 500);
            }, 2000);
        }

        msgAdicionar.style.display = "none";
        msgAdicionar.innerText = "";

        if (!produtoSelecionado) {
            mostrarErro("Selecione um produto antes de adicionar!");
            return;
        }

        let qtd = parseInt(qtdInput.value) || 0;
        if (qtd <= 0) {
            mostrarErro("Quantidade inválida!");
            return;
        }

        let qtdJaAdicionada = 0;
        tabela.querySelectorAll("tr").forEach(linha => {
            if (parseInt(linha.getAttribute("data-id")) === produtoSelecionado.produtoID) {
                qtdJaAdicionada += parseInt(linha.children[1].innerText);
            }
        });

        if (qtd + qtdJaAdicionada > produtoSelecionado.produtoQtdEstoque) {
            mostrarErro(`Estoque insuficiente! Disponível: ${produtoSelecionado.produtoQtdEstoque - qtdJaAdicionada}`);
            return;
        }

        const preco = produtoSelecionado.precoCalculado || produtoSelecionado.produtoPrecoVenda;
        const total = qtd * preco;

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

        const index = tabela.children.length - 1;
        itensHidden.insertAdjacentHTML('beforeend', `
        <input type="hidden" name="Itens[${index}].ProdutoID" value="${produtoSelecionado.produtoID}" />
        <input type="hidden" name="Itens[${index}].ItemVendaQtd" value="${qtd}" />
        <input type="hidden" name="Itens[${index}].ItemVendaPreco" value="${preco}" />
        <input type="hidden" name="Itens[${index}].ItemVendaTotal" value="${total}" />
        `);

        totalVenda += total;
        totalVendaLabel.innerText = formatarMoeda(totalVenda);

        produtoSelecionado = null;
        buscaInput.value = "";
        qtdInput.value = 0;
        valorUnitario.value = "";
        valorTotal.value = "";

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

        const inputs = itensHidden.querySelectorAll(`[name^="Itens[${index}]"]`);
        inputs.forEach(i => i.remove());

        atualizarBotaoConfirmar();
    });

    // ==================== RADIO PAGAMENTO ====================
    radiosPagamento.forEach(radio => {
        radio.addEventListener("change", atualizarBotaoConfirmar);
    });

    atualizarBotaoConfirmar();
});