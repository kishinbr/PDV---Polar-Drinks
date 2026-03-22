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