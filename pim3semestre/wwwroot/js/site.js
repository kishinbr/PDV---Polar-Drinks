$(document).ready(function () {

    $('#Categoria').DataTable({
        pageLength: 10,
        lengthMenu: [10, 20, 30],
        scrollY: "50vh", 
        scrollCollapse: true,
        paging: true,
        language: {
            "decimal": "",
            "emptyTable": "Nenhuma categoria cadastrada",
            "info": "Mostrando de _START_ a _END_ de um total de _TOTAL_ categorias",
            "infoEmpty": "Mostrando de 0 a 0 de 0 Categorias",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ categorias",
            "loadingRecords": "Carregando...",
            "processing": "",
            "search": "Procurar:",
            "zeroRecords": "Categoria não encontrada",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            },
            "aria": {
                "orderable": "Order by this column",
                "orderableReverse": "Reverse order this column"
            }
        }
    });

    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).alert('close');
        })
    }, 3000);

});

$(document).ready(function () {

    $('#Produtos').DataTable({
        pageLength: 10,
        lengthMenu: [10, 20, 30],
        scrollY: "50vh",
        scrollCollapse: true,
        paging: true,

        columnDefs: [
            { orderable: false, targets: [6] }, // desabilita ordenação na coluna Ações
            { className: "text-end", targets: [3, 4] }, // preço e quantidade alinhados à direita
            { className: "text-center", targets: [5, 6] } // status e ações centralizados
        ],

        language: {
            "decimal": "",
            "emptyTable": "Nenhum produto cadastrado",
            "info": "Mostrando de _START_ a _END_ de um total de _TOTAL_ produtos",
            "infoEmpty": "Mostrando de 0 a 0 de 0 produtos",
            "infoFiltered": "(filtrado de _MAX_ produtos no total)",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ produtos",
            "loadingRecords": "Carregando...",
            "search": "Procurar:",
            "zeroRecords": "Produto não encontrado",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            },
            "aria": {
                "orderable": "Ordenar por esta coluna",
                "orderableReverse": "Ordem reversa desta coluna"
            }
        }
    });

    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).alert('close');
        });
    }, 3000);

});

$(document).ready(function () {

    $('#Fornecedores').DataTable({
        pageLength: 10,
        lengthMenu: [10, 20, 30],
        scrollY: "50vh",
        scrollCollapse: true,
        paging: true,

        columnDefs: [
            { orderable: false, targets: [9] }, // desabilita ordenação na coluna Ações
            { className: "text-start", targets: [1, 2, 3,4] },
            { className: "text-center", targets: [9] } // centraliza os botões de Ações
        ],

        language: {
            "decimal": "",
            "emptyTable": "Nenhum fornecedor cadastrado",
            "info": "Mostrando de _START_ a _END_ de um total de _TOTAL_ fornecedores",
            "infoEmpty": "Mostrando de 0 a 0 de 0 fornecedores",
            "infoFiltered": "(filtrado de _MAX_ fornecedores no total)",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ fornecedores",
            "loadingRecords": "Carregando...",
            "search": "Procurar:",
            "zeroRecords": "Fornecedor não encontrado",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            },
            "aria": {
                "orderable": "Ordenar por esta coluna",
                "orderableReverse": "Ordem reversa desta coluna"
            }
        }
    });

    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).alert('close');
        });
    }, 3000);

});
document.addEventListener("DOMContentLoaded", function () {

    // ================= CEP =================
    const cepInput = document.getElementById("FornecedorCEP");

    if (cepInput) {
        cepInput.addEventListener("input", function (e) {
            let value = e.target.value.replace(/\D/g, "").substring(0, 8);

            if (value.length > 5) {
                value = value.replace(/^(\d{5})(\d{0,3})$/, "$1-$2");
            }

            e.target.value = value;
        });
    }

    // ================= TELEFONE =================
    const telefoneInput = document.getElementById("FornecedorTelefone");

    if (telefoneInput) {
        telefoneInput.addEventListener("input", function (e) {
            let value = e.target.value.replace(/\D/g, "").substring(0, 11);

            if (value.length > 6) {
                value = value.replace(/^(\d{2})(\d{5})(\d{0,4})$/, "($1) $2-$3");
            } else if (value.length > 2) {
                value = value.replace(/^(\d{2})(\d{0,5})$/, "($1) $2");
            } else {
                value = value.replace(/^(\d*)$/, "($1");
            }

            e.target.value = value;
        });
    }

    // ================= CNPJ =================
    const cnpjInput = document.getElementById("FornecedorCNPJ");

    if (cnpjInput) {
        cnpjInput.addEventListener("input", function (e) {
            let value = e.target.value.replace(/\D/g, "").substring(0, 14);

            value = value.replace(/^(\d{2})(\d)/, "$1.$2");
            value = value.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3");
            value = value.replace(/\.(\d{3})(\d)/, ".$1/$2");
            value = value.replace(/(\d{4})(\d)/, "$1-$2");

            e.target.value = value;
        });
    }

});