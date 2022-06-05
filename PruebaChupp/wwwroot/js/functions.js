function mensaje(text, title, icon) {
    Swal.fire({
        text: title,
        title: text,
        icon: icon
    });
}
$('.input-number').on('input', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});
function convertDateFormat(string) {
    string = string.split(' ');
    var info = string[0].split('-');
    var dia = info[2].split('T');
    return info[0] + '-' + info[1] + '-' + dia[0];
}
function calcularEdad(fecha) {
    var hoy = new Date();
    var cumpleanos = new Date(fecha);
    var edad = hoy.getFullYear() - cumpleanos.getFullYear();
    var m = hoy.getMonth() - cumpleanos.getMonth();
    if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
        edad--;
    }
    return edad;
}
$(document).ready(function () {
    $('#archivo_excel').change(function () {
        var inputFileImage = document.getElementById('archivo_excel');
        var file = inputFileImage.files[0];
        var fd = new FormData();
        fd.append('archivo_excel', file);
        $.ajax({
            url: 'utilidad/subirarchivo',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                $('#archivo_excel').val('');
                if (data.result == false)
                    mensaje(data.mns, 'Mensaje del sistema', 'error');
                else {
                    $('#tablePersonas').DataTable().ajax.reload();
                    mensaje('Importacion correcta.', 'Mensaje del sistema', 'info');
                }
            }
        });
    });
    $('#descargar_excel').click(function () {
        /*$.ajax({
            url: 'utilidad/descargarxls',
            type: 'GET',
            success: function () {
            }
        });*/
        window.open('utilidad/descargarxls', '_new');
        mensaje('Al guardar la plantilla, por favor guardarla en formato (Libro de Excel 97-2003)','Mensaje del sistema','info');
    });
});