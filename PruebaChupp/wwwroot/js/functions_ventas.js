var table = $('#tableVentas').DataTable();
table = $("#tableVentas").dataTable().fnDestroy()
const hoy = new Date();
$(document).ready(function () {
    $.ajax({
        url: 'personas/listar',
        type: 'POST',
        dataType: 'json',
        success: function (json) {
            $("#id_persona").html('');
            $("#id_persona").append('<option value="">--Seleccione una persona--</option>');
            $.each(JSON.parse(json.result), function (k, v) {
                $("#id_persona").append("<option value=\"" + v.id_persona + "\">" + v.nombre_persona + ' ' + v.apellido_persona + "</option>");
            })
        }
    });
    $.ajax({
        url: 'seguros/listar',
        type: 'POST',
        dataType: 'json',
        success: function (json) {
            $("#id_seguro").html('');
            $("#id_seguro").append('<option value="">--Seleccione un seguro--</option>');
            $.each(JSON.parse(json.result), function (k, v) {
                $("#id_seguro").append("<option value=\"" + v.id_seguro + "\">" + v.nombre_seguro + "</option>");
            })
        }
    });
    $.ajax({
        url: 'seguros/porcentajes',
        type: 'POST',
        dataType: 'json',
        success: function (json) {
            $("#id_seguro").html('');
            $("#id_seguro").append('<option value="">--Seleccione un porcentaje--</option>');
            $.each(JSON.parse(json.result), function (k, v) {
                $("#id_porcentaje").append("<option value=\"" + v.id_porcentaje + "\">" + v.porcentaje + "</option>");
            })
        }
    });

    table = $('#tableVentas').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        orderMulti: false,
        "ajax": {
            "url": "ventas/consultar",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                className: "text-center",
                render(mData, type, row, meta) {
                    return '<a href="javascript:;" onclick="eliminar(\'' + row.id_venta + '\')">Eliminar</a>';
                }
            },
            { "data": "codigo_seguro" },
            { "data": "nombre_seguro" },
            {
                render(mData, type, row, meta) {
                    return row.nombre_persona + ' ' + row.apellido_persona;
                }
            },
            { "data": "rango" },
            { "data": "porcentaje" },
            {
                className: "text-center",
                render(mData, type, row, meta) { return convertDateFormat(row.fecha_creacion); }
            },
        ]
    });
    $('#tableVentas tbody').on('click', 'tr', function () {
        var data = table.row(this).data();
        console.log(data);
        if ($('#eliminar').val() > 0) {
            table.row($(this).closest("tr").get(0)).remove().draw();
            $.ajax({
                url: 'ventas/eliminar',
                type: 'post',
                data: { id: data.id_venta },
                dataType: 'json',
                success: function (json) {
                    if (json.result) {
                        Swal.fire({
                            text: 'Mensaje del sistema',
                            title: json.mns,
                            icon: 'info'
                        }).then((result => {
                            $('#tableVentas').DataTable().ajax.reload();
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
            $('#edad').val(data.edad);
            $('#id_persona').val(data.id_persona);
            $('#id_seguro').val(data.id_seguro);
            $('#id_porcentaje').val(data.id_porcentaje);
            $('#valor_base').val(parseFloat(data.valor_base).toFixed(2));
            $('#valor_seguro').val(parseFloat(data.valor_venta).toFixed(2));
            $('#rango_seguro').val(data.rango);
            $('#porcentaje').val(data.porcentaje);
            $('#prima').val(parseFloat(data.prima).toFixed(2));
            $('#id_rango').val(data.id_rango);
            $('#edad').val(calcularEdad(data.fecha_nacimiento));
            $('#modalVentas').modal('toggle');
        }
    });
    $('#btnagregar').click(function () {
        vaciarmodal();
        if ($('#id_venta').val() == '') {
            $('#cheestado').attr({
                'checked': true,
                'disabled': true
            });
        }
    });
    $('#id_seguro').change(function () {
        if ($('#id_seguro').val != '') {
            $.ajax({
                url: 'seguros/llenardatos',
                type: 'post',
                data: {
                    id_persona: $('#id_persona').val(),
                    id_seguro: $('#id_seguro').val(),
                },
                dataType: 'json',
                success: function (json) {
                    var dato = JSON.parse(json.result);
                    dato = dato[0];
                    $('#edad').val(dato.edad);
                    $('#valor_base').val(parseFloat(dato.valor_base).toFixed(2));
                    $('#valor_seguro').val(parseFloat(dato.valor_seguro).toFixed(2));
                    $('#rango_seguro').val(parseFloat(dato.rango).toFixed(2));
                    $('#porcentaje').val(dato.porcentaje);
                    $('#prima').val(parseFloat(dato.prima).toFixed(2));
                    $('#id_rango').val(dato.id_rango);
                }
            });
        }
    });
    $('#btnguardar').click(function () {
        var mns = '';
        if ($('#id_persona').val() == '') {
            mns = 'Seleccione a una persona';
        }
        else if ($('#id_seguro').val() == '') {
            mns = 'Seleccione un seguro';
        }
        if (mns != '')
            mensaje(mns, 'Mensaje del sistema', 'warning');
        else {
            $.ajax({
                url: 'ventas/guardar',
                type: 'post',
                data: { datos: JSON.stringify($('#frm_').serializeArray()) },
                dataType: 'json',
                async: true,
                success: function (json) {
                    console.log(json);
                    if (json.result) {
                        $('#tableVentas').DataTable().ajax.reload();
                        Swal.fire({
                            text: 'Mensaje del sistema',
                            title: json.mns,
                            icon: 'info'
                        }).then((result => {
                            $('#modalVentas').modal('toggle');
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
    $('#edad').val('');
    $('#valor_base').val('');
    $('#valor_seguro').val('');
    $('#rango_seguro').val('');
    $('#porcentaje').val('');
    $('#prima').val('');
    $('#id_rango').val('');
    $('#id_venta').val('');
    $('#id_porcentaje').val('');
    $('#id_seguro').val('');
}