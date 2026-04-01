document.addEventListener("DOMContentLoaded", () => {

    const buscaInput = document.getElementById("buscaProduto");
    const lista = document.getElementById("listaSugestoes");

    const qtdInput = document.getElementById("quantidade");
    const valorUnitario = document.getElementById("valorUnitario");
    const valorTotal = document.getElementById("valorTotal");
    const tabela = document.getElementById("tabelaItens");
    const totalVendaLabel = document.getElementById("totalVenda");
    const itensHidden = document.getElementById("itensHidden");

    let totalVenda = 0;
    let produtoSelecionado = null;
    let indiceSelecionado = -1;

    function renderLista(filtro = "") {
        lista.innerHTML = "";
        indiceSelecionado = -1;

        let filtrados = produtos
            .filter(p => p.produtoNome.toLowerCase().includes(filtro) || p.produtoCodBarra.toLowerCase().includes(filtro))
            .slice(0, 8);

        filtrados.forEach(p => {
            const item = document.createElement("a");
            item.classList.add("list-group-item", "list-group-item-action");
            item.textContent = `${p.produtoNome} (${p.produtoCodBarra})`;
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

    qtdInput.addEventListener("input", calcularTotal);

    function calcularTotal() {
        let qtd = parseFloat(qtdInput.value) || 0;
        let preco = produtoSelecionado ? produtoSelecionado.produtoPrecoVenda : 0;
        valorTotal.value = formatarMoeda(qtd * preco);
    }

    document.getElementById("btnAdicionar").addEventListener("click", () => {
        if (!produtoSelecionado) { alert("Selecione um produto!"); return; }

        let qtd = parseInt(qtdInput.value) || 0;
        if (qtd <= 0) { alert("Quantidade inválida!"); return; }

        let qtdJaAdicionada = 0;
        tabela.querySelectorAll("tr").forEach(linha => {
            if (parseInt(linha.getAttribute("data-id")) === produtoSelecionado.produtoID) {
                qtdJaAdicionada += parseInt(linha.children[1].innerText);
            }
        });

        if (qtd + qtdJaAdicionada > produtoSelecionado.produtoQtdEstoque) {
            alert(`Estoque insuficiente! Disponível: ${produtoSelecionado.produtoQtdEstoque - qtdJaAdicionada}`);
            return;
        }

        const preco = produtoSelecionado.produtoPrecoVenda;
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
        qtdInput.value = 1;
        valorUnitario.value = "";
        valorTotal.value = "";
    });

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
    });

    function formatarMoeda(valor) {
        return "R$ " + parseFloat(valor).toFixed(2).replace(".", ",");
    }


    //document.getElementById("formVenda").addEventListener("submit", function (e) {
    //    const msg = document.getElementById("mensagemErro");
    //    msg.classList.add("d-none");
    //    msg.innerText = "";

    //    // Verifica se há itens adicionados (olhando os inputs hidden)
    //    const itens = document.querySelectorAll('#itensHidden input[name$=".ProdutoID"]');
    //    if (itens.length === 0) {
    //        e.preventDefault();
    //        msg.innerText = "Adicione pelo menos um item à venda!aaaaaa";
    //        msg.classList.remove("d-none");
    //        return false;
    //    }

    //    // Verifica se o pagamento foi selecionado
    //    const pagamentoSelecionado = document.querySelector('input[name="VendaTipoPagamento"]:checked');
    //    if (!pagamentoSelecionado) {
    //        e.preventDefault();
    //        msg.innerText = "Selecione um tipo de pagamento!aaaaa";
    //        msg.classList.remove("d-none");
    //        return false;
    //    }
    //});
});