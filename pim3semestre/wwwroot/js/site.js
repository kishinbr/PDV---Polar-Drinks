$(document).ready(function () {

    $('#Categoria').DataTable({
        pageLength: 5,
        lengthMenu: [5, 10, 20, 50],
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