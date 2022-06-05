var table = $('#tablePersonas').DataTable();
table = $("#tablePersonas").dataTable().fnDestroy()
const hoy = new Date();
$(document).ready(function () {
    table = $('#tablePersonas').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        orderMulti: false,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'copy' },
            { extend: 'csv' },
            { extend: 'excel', title: 'Personas | ' + hoy },
            { extend: 'pdf', title: 'Personas | ' + hoy },
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
            "url": "personas/consultar",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                className: "text-center",
                render(mData, type, row, meta) {
                    return '<a href="javascript:;" onclick="eliminar(\'' + row.id_persona + '\')">Eliminar</a>';
                }
            },
            { "data": "id_persona" },
            { "data": "cedula_persona" },
            { "data": "nombre_persona" },
            { "data": "apellido_persona" },
            {
                className: "text-center",
                render(mData, type, row, meta) { return calcularEdad(row.fecha_nacimiento_persona); }
            },
            { "data": "email_persona" },
            { "data": "telefono_persona" },
            { "data": "celular_persona" },
            { "data": "direccion_persona" },
        ]
    });
    $('#tablePersonas tbody').on('click', 'tr', function () {
        var data = table.row(this).data();
        if ($('#eliminar').val() > 0) {
            table.row($(this).closest("tr").get(0)).remove().draw();
            $.ajax({
                url: 'personas/eliminar',
                type: 'post',
                data: { id: data.id_persona },
                dataType: 'json',
                success: function (json) {
                    if (json.result) {
                        Swal.fire({
                            text: 'Mensaje del sistema',
                            title: json.mns,
                            icon: 'info'
                        }).then((result => {
                            $('#tablePersonas').DataTable().ajax.reload();
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
            $('#id_persona').val(data.id_persona);
            $('#cedula').val(data.cedula_persona);
            $('#nombre').val(data.nombre_persona);
            $('#apellido').val(data.apellido_persona);
            $('#email').val(data.email_persona);
            $('#telefono').val(data.telefono_persona);
            $('#celular').val(data.celular_persona);
            $('#direccion').val(data.direccion_persona);
            if (data.estado_persona == 1)
                $('#cheestado').prop('checked', true);
            else
                $('#cheestado').prop('checked', false);

            $('#fecha_nacimiento').val(convertDateFormat(data.fecha_nacimiento_persona));
            //$('#fecha_nacimiento').val('1983-09-10');
            $('#modalPersonas').modal('toggle');
        }
    });
    $('#btnguardar').click(function () {
        var mns = '';
        if ($('#cedula').val() == '') {
            mns = 'Ingrese el número de cedula de la persona';
        }
        else if ($('#nombre').val() == '') {
            mns = 'Ingrese el nombre de la persona';
        }
        else if ($('#apellido').val() == '') {
            mns = 'Ingrese el apellido de la persona';
        }
        else if ($('#celular').val() == '') {
            mns = 'Ingrese el número de celular de la persona';
        }
        else if ($('#direccion').val() == '') {
            mns = 'Ingrese la dirección de la persona';
        }
        else if ($('#fecha_nacimiento').val() == '') {
            mns = 'Ingrese la fecha de nacimiento de la persona';
        }
        if (mns != '')
            mensaje(mns, 'Mensaje del sistema', 'warning');
        else {
            $.ajax({
                url: 'personas/guardar',
                type: 'post',
                data: { datos: JSON.stringify($('#frm_').serializeArray()) },
                dataType: 'json',
                async: true,
                success: function (json) {
                    console.log(json);
                    if (json.result) {
                        $('#tablePersonas').DataTable().ajax.reload();
                        Swal.fire({
                            text: 'Mensaje del sistema',
                            title: json.mns,
                            icon: 'info'
                        }).then((result => {
                            $('#modalPersonas').modal('toggle');
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
    $('#btnagregar').click(function () {
        vaciarmodal();
        if ($('#id_persona').val() == '') {
            $('#cheestado').attr({
                'checked': true,
                'disabled': true
            });
        }
    });
});
function eliminar(id) {
    $('#eliminar').val(id);
}
function vaciarmodal() {
    $('#id_persona').val('');
    $('#cedula').val('');
    $('#nombre').val('');
    $('#apellido').val('');
    $('#email').val('');
    $('#telefono').val('');
    $('#celular').val('');
    $('#direccion').val('');
    $('#fecha_nacimiento').val('');
}