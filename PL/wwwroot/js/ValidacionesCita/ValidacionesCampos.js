//Validacion para los textbox de letras
function SoloLetras(event, textbox) {
    var id = textbox.id;
    var input = event.key

    if (/^[a-zA-Z\s]*$/.test(input)) {
        if (id == "txtNombre") {
            $("#lblCandidatoNombreErrorMessage").text('')
            $("#txtNombre").css('border', '1px solid #ccc');
        }
        else
            if (id == "txtApellidoPaterno") {
                $("#lblApellidoPaternoErrorMessage").text('')
                $("#txtApellidoPaterno").css('border', '1px solid #ccc');
            }
            else
                if (id == "txtApellidoMaterno") {
                    $("#lblApellidoMaternoErrorMessage").text('')
                    $("#txtApellidoMaterno").css('border', '1px solid #ccc');
                }
        return true
    }
    else {
        if (id == "txtNombre") {
            $("#lblCandidatoNombreErrorMessage").text('Solo se permiten letras')
            $("#txtNombre").css('border', '2px solid #a94442');
        }
        else
            if (id == "txtApellidoPaterno") {
                $("#lblApellidoPaternoErrorMessage").text('Solo se permiten letras')
                $("#txtApellidoPaterno").css('border', '2px solid #a94442');
            }
            else
                if (id == "txtApellidoMaterno") {
                    $("#lblApellidoMaternoErrorMessage").text('Solo se permiten letras')
                    $("#txtApellidoMaterno").css('border', '2px solid #a94442');
                }
        return false
    }
}

//Validacion para textbox de numeros
function SoloNumeros(event, textbox) {
    var id = textbox.id;
    var input = event.key

    if (/^[0-9]+$/.test(input)) {
        if (id == "txtTelefono") {
            $("#lblTelefonoErrorMessage").text('')
            $("#txtTelefono").css('border', '1px solid #ccc');
        }
        return true
    }
    else {
        if (id == "txtTelefono") {
            $("#lblTelefonoErrorMessage").text('Solo se permiten numeros')
            $("#txtTelefono").css('border', '2px solid #a94442');
        }
        return false
    }
}

//Validacion para email
function checkEmail(str) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!re.test(str)) {
        $("#lblEmailErrorMessage").text('Ingresa un correo electronico valido')
        $("#txtEmail").css('border', '2px solid #a94442');
    }
    else {
        $("#lblEmailErrorMessage").text('')
        $("#txtEmail").css('border', '1px solid #ccc');
    }
}