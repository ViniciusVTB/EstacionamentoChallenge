function AddCondutor(event, id) {
    var url = $("#url").val();
    var urlFinalizar = $("#urlFinalizar").val();

    event.preventDefault();

    if (confirm("Deseja cadastrar ou vincular condutor?")) {
        location.href = url + "/" + id;
    } else {
        location.href = urlFinalizar + "/" + id;
    }
}

function Create(event) {

    var url = $("#url").val();
    var urlFinalizar = $("#urlFinalizar").val();

    event.preventDefault();

    var condutor = {
        Nome: $("#Nome").val(),
        Cpf: $("#Cpf").val()
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        async: true,
        url: url,
        data: condutor,
        success: function (result) {
            if (result.situacao == true) {
                alert(result.mensagem)
                location.href = urlFinalizar + "/" + parseInt(result.data);
            }
            else
                alert(result.mensagem);
        },
        error: function (error) {

        }
    });

}