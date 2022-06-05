var table = $('#tableSeguros').DataTable();
table = $("#tableSeguros").dataTable().fnDestroy()
const hoy = new Date();
$(document).ready(function () {
    $.ajax({
        url: '/seguros/tipos',
        type: 'POST',
        dataType: 'json',
        success: function (json) {
            $("#tipo_seguro").html('');
            $("#tipo_seguro").append('<option value="">--Tipos de seguros--</option>');
            $.each(JSON.parse(json.result), function (k, v) {
                $("#tipo_seguro").append("<option value=\"" + v.id_tipo_seguro + "\">" + v.tipo_seguro + "</option>");
            })
        }
    });
    table = $('#tableSeguros').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        orderMulti: false,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'copy' },
            { extend: 'csv' },
            { extend: 'excel', title: 'Seguros | ' + hoy },
            { extend: 'pdf', title: 'Seguros | ' + hoy },
            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');
                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ],
        "ajax": {
            "url": "/seguros/consultar",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                className: "text-center",
                render(mData, type, row, meta) {
                    return '<a href="javascript:;" onclick="eliminar(\'' + row.id_seguro + '\')">Eliminar</a>';
                }
            },
            { "data": "codigo_seguro" },
            { "data": "nombre_seguro" },
            { "data": "tipo_seguro" },
            {
                className: "text-right", render(mData, type, row, meta) {
                    return parseFloat(row.valor_seguro).toFixed(2);
                }
            },
            {
                className: 'text-center',
                render(mData, type, row, meta) {
                    var d = new Date(row.fecha_creacion_seguro);
                    var day = d.getDate();
                    var month = d.getMonth() + 1;
                    var year = d.getFullYear();
                    if (day < 10) { day = "0" + day; }
                    if (month < 10) { month = "0" + month; }
                    return day + "-" + month + "-" + year;
                }
            }
        ]
    });
    $('#tableSeguros tbody').on('click', 'tr', function () {
        var data = table.row(this).data();
        if ($('#eliminar').val() > 0) {
            table.row($(this).closest("tr").get(0)).remove().draw();
            $.ajax({
                url: '/seguros/eliminar',
                type: 'post',
                data: { id: data.id_seguro },
                dataType: 'json',
                success: function (json) {
                    if (json.result) {
                        Swal.fire({
                            text: 'Mensaje del sistema',
                            title: json.mns,
                            icon: 'info'
                        }).then((result => {
                            $('#tableSeguros').DataTable().ajax.reload();
                        }));
                    }
                    else {
                        mensaje(json.mns, 'mensaje del sistema', 'error');
                    }
                }
            });
            $('#eliminar').val(0);
        }
        else {
            $('#cheestado').attr({ 'disabled': false });
            $('#id_seguro').val(data.id_seguro);
            $('#codigo_seguro').val(data.codigo_seguro);
            $('#nombre').val(data.nombre_seguro);
            $('#fecha_creacion').val(data.fecha_creacion_seguro);
            $('#tipo_seguro').val(data.id_tipo_seguro);
            $('#valor').val(parseFloat(data.valor_seguro).toFixed(2));
            if (data.estado_seguro == 1)
                $('#cheestado').prop('checked', true);
            else
                $('#cheestado').prop('checked', false);
            $('#modalSeguros').modal('toggle');
        }
    });
    $('#btnagregar').click(function () {
        vaciarmodal();
        if ($('#id_seguro').val() == '') {
            $('#cheestado').attr({
                'checked': true,
                'disabled': true
            });
        }
    });
    $('#btnguardar').click(function () {
        var mns = '';
        if ($('#nombre').val() == '') {
            mns = 'Ingrese el nombre del seguro';
        }
        else if ($('#tipo_seguro').val() == '') {
            mns = 'Seleccione el tipo del seguro';
        }
        if (mns != '')
            mensaje(mns, 'Mensaje del sistema', 'warning');
        else {
            $.ajax({
                url: '/seguros/guardar',
                type: 'post',
                data: { datos: JSON.stringify($('#frm_').serializeArray()) },
                dataType: 'json',
                async: true,
                success: function (json) {
                    console.log(json);
                    if (json.result) {
                        $('#tableSeguros').DataTable().ajax.reload();
                        Swal.fire({
                            text: 'Mensaje del sistema',
                            title: json.mns,
                            icon: 'info'
                        }).then((result => {
                            $('#modalSeguros').modal('toggle');
                            vaciarmodal();
                        }));
                    }
                    else {
                        mensaje(json.mns, 'mensaje del sistema', 'error');
                    }
                }
            });
        }
    });
});
function eliminar(id) {
    $('#eliminar').val(id);
}
function vaciarmodal() {
    $('#id_seguro').val('');
    $('#codigo_seguro').val('');
    $('#nombre').val('');
    $('#tipo_seguro').val('');
    $('#valor').val('');
}